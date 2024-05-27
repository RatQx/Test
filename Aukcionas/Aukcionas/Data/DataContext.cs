using Aukcionas.Auth.Model;
using Aukcionas.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Aukcionas.Data
{

    public class DataContext : IdentityDbContext<ForumRestUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Auction> Auctions => Set<Auction>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Report> Reports => Set<Report>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Recommendations> Recommendations => Set<Recommendations>();
        public DbSet<UserRecommendations> UserRecommendations => Set<UserRecommendations>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<Chat> Chats => Set<Chat>();
        public DbSet<PaymentLinks> PaymentLinks => Set<PaymentLinks>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PaymentLinks>()
                .HasOne(p => p.User)
                .WithMany(u => u.Payment_Links)
                .HasForeignKey(p => p.UserId);
        }
    }
}
