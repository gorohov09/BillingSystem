using BillingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillingSystem.DAL.Context
{
    public class BillingSystemDB : DbContext
    {
        public DbSet<CoinDomain> Coins { get; set; }

        public DbSet<UserProfileDomain> UserProfiles { get; set; }

        public BillingSystemDB(DbContextOptions<BillingSystemDB> options)
            : base(options)
        {
        }
    }
}
