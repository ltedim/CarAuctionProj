using CarAuctionCommon.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionCommon.Context
{
    public class AuctionDbContext : DbContext
    {
        public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Auction> Auctions{ get; set; }
        public DbSet<Bid> Bids{ get; set; }
    }
}
