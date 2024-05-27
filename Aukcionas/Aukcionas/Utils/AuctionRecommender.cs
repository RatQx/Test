﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aukcionas.Auth.Model;
using Aukcionas.Models;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Aukcionas.Data;

namespace Aukcionas.Utils
{
    public class AuctionRecommender
    {
        private readonly MLContext _mlContext;

        public AuctionRecommender()
        {
            _mlContext = new MLContext(seed: 0);
        }

        public async Task TrainModelsForUsers(DataContext dbContext)
        {
            Console.WriteLine("Training model for user predictions..");
            var distinctUserIds = dbContext.Recommendations.Select(r => r.UserId).Distinct().ToList();

            foreach (var userId in distinctUserIds)
            {
                IDataView dataView = LoadDataForUser(userId, dbContext);

                if (dataView != null)
                {
                    var dataProcessPipeline = _mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "UserId", outputColumnName: "UserIdEncoded")
                        .Append(_mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "AuctionId", outputColumnName: "AuctionIdEncoded"));

                    var options = new MatrixFactorizationTrainer.Options
                    {
                        MatrixColumnIndexColumnName = "UserIdEncoded",
                        MatrixRowIndexColumnName = "AuctionIdEncoded",
                        LabelColumnName = "InteractionType",
                        NumberOfIterations = 30,
                        ApproximationRank = 50,
                        LearningRate = 0.01f,
                        Lambda = 0.025f
                    };

                    var trainer = _mlContext.Recommendation().Trainers.MatrixFactorization(options);

                    var trainingPipeline = dataProcessPipeline.Append(trainer);

                    var model = trainingPipeline.Fit(dataView);

                    var topAuctions = SuggestAuctionsForUser(model, userId, dbContext);
                    UpdateUserRecommendations(userId, topAuctions, dbContext);

                    Console.WriteLine($"Model training completed for user {userId}.");
                }
            }
        }

        private IDataView LoadDataForUser(string userId, DataContext dbContext)
        {
            var interactionsForUser = dbContext.Recommendations.Where(r => r.UserId == userId).ToList();

            if (interactionsForUser.Any())
            {
                var data = interactionsForUser.Select(i => new
                {
                    UserId = i.UserId,
                    AuctionId = i.AuctionId,
                    InteractionType = (float)i.InteractionType
                });

                IDataView dataView = _mlContext.Data.LoadFromEnumerable(data);
                return dataView;
            }

            return null;
        }

        private IEnumerable<(int, float)> SuggestAuctionsForUser(ITransformer model, string userId, DataContext dbContext)
        {
            var currentAuctions = dbContext.Auctions
                .Where(a => a.auction_end_time > DateTime.Now)
                .ToList();

            var predictions = new List<(int, float)>();

            var predictionEngine = _mlContext.Model.CreatePredictionEngine<AuctionForPrediction, AuctionRatingPrediction>(model);

            var userInteractions = dbContext.Recommendations
                .Where(r => r.UserId == userId)
                .ToList();

            var averageInteraction = userInteractions.Average(r => (float)r.InteractionType);

            foreach (var auction in currentAuctions)
            {
                var userInteractedCategory = userInteractions.Any(r => r.AuctionId == auction.id && (float)r.InteractionType > averageInteraction);

                var auctionForPrediction = new AuctionForPrediction
                {
                    AuctionId = auction.id,
                    Category = auction.category,
                    UserId = userId
                };

                try
                {
                    var rating = predictionEngine.Predict(auctionForPrediction).Score;
                    float fallbackRating = 0.5f; 
                    predictions.Add((auction.id, float.IsNaN(rating) ? fallbackRating : rating));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error predicting rating for auction {auction.id}: {ex.Message}");
                    float fallbackRating = 0.5f;
                    predictions.Add((auction.id, fallbackRating));
                }
            }

            var topAuctions = predictions.OrderByDescending(x => x.Item2).Take(5);

            return topAuctions;
        }

        private void UpdateUserRecommendations(string userId, IEnumerable<(int, float)> topAuctions, DataContext dbContext)
        {
            var existingRecommendations = dbContext.UserRecommendations.FirstOrDefault(ur => ur.UserId == userId);

            if (existingRecommendations == null)
            {
                var newRecommendations = new UserRecommendations
                {
                    UserId = userId,
                    RecommendedAuctionIds = topAuctions.Select(a => a.Item1).ToList()
                };

                dbContext.UserRecommendations.Add(newRecommendations);
            }
            else
            {
                existingRecommendations.RecommendedAuctionIds = topAuctions.Select(a => a.Item1).ToList();
                dbContext.UserRecommendations.Update(existingRecommendations);
            }

            dbContext.SaveChanges();
        }
    }

    public class AuctionRatingPrediction
    {
        public float Score { get; set; }
    }

    public class AuctionForPrediction
    {
        public string Category { get; set; }
        public int AuctionId { get; set; }
        public string UserId { get; set; }
    }
}
