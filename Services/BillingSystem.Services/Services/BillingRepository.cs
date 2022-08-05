using BillingSystem.DAL.Context;
using BillingSystem.Domain.Entities;
using BillingSystem.Domain.ViewModels;
using BillingSystem.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static BillingSystem.Domain.ViewModels.ResponseViewModel;

namespace BillingSystem.Services.Services
{
    public class BillingRepository : IBillingRepository
    {
        private readonly ILogger<BillingRepository> _logger;
        private readonly BillingSystemDB _db;

        public BillingRepository(BillingSystemDB db, ILogger<BillingRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<bool> CoinsEmission(long emissionAmount)
        {
            var userProfilies = await GetUserProfilies();
            var userProfiliesVm = userProfilies.Select(p => new UserProfileViewModel
            {
                Name = p.Name,
                Rating = p.Rating,
            }).ToList();

            var tableCoins = CountNumberCoinsForUser(userProfiliesVm, (int)emissionAmount);
            foreach (var item in tableCoins)
            {
                var userProfile = GetUserProfileByName(item.Key);
                if (userProfile == null)
                    continue;
                for (int i = 0; i < item.Value; i++)
                    userProfile.Coins.Add(new CoinDomain
                    {
                        History = "Эмиссия"
                    });
                userProfile.Amount = item.Value;
            }
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserProfileViewModel>> GetUserProfiliesVm()
        {
            var userProfilies = await GetUserProfilies();
            return userProfilies.Select(userProfile => new UserProfileViewModel
            {
                Name = userProfile.Name,
                Amount = userProfile.Amount ?? 0,
            });
        }

        private Dictionary<string, int> CountNumberCoinsForUser(List<UserProfileViewModel> userProfilies, int emissionCount)
        {
            var result = new Dictionary<string, int>();
            var totalRaiting = GetTotalRaiting();
            foreach (var userProfile in userProfilies)
            {
                var coinsAmount = (int)Math.Floor((userProfile.Rating / totalRaiting) * emissionCount);
                if (!result.ContainsKey(userProfile.Name))
                    result.Add(userProfile.Name, coinsAmount);
            }
            return result;
        }

        private async Task<List<UserProfileDomain>> GetUserProfilies()
            => await _db.UserProfiles.ToListAsync();

        private double GetTotalRaiting()
            => _db.UserProfiles.Sum(p => p.Rating);

        private UserProfileDomain? GetUserProfileByName(string name)
            => _db.UserProfiles.FirstOrDefault(p => p.Name == name);
    }
}
