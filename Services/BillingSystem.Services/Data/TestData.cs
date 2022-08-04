using BillingSystem.Domain.Entities;

namespace BillingSystem.Services.Data
{
    public static class TestData
    {
        public static IEnumerable<UserProfileDomain> UserProfiles { get; } = new List<UserProfileDomain>()
        {
            new UserProfileDomain() {Name = "boris", Rating = 5000},
            new UserProfileDomain() {Name = "maria", Rating = 1000},
            new UserProfileDomain() {Name = "oleg", Rating = 800},
        };
    }
}
