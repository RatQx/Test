using Aukcionas.Data;
using Aukcionas.Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;


namespace Aukcionas.Utils;

public class GetAucReq
{
    public string? name { get; set; }

    public async Task<List<Auction>> GetFilteredAuctionsAsync(DataContext _dataContext)
    {
        var query = _dataContext.Auctions.AsQueryable();
        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(x => x.name.Contains(name));
        }
        var auctions = await query
            .Where(x => x.auction_ended == null && x.auction_end_time >= DateTime.UtcNow)
            .ToListAsync();

        return auctions;
    }
}



