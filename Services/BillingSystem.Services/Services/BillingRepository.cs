using BillingSystem.DAL.Context;
using BillingSystem.Domain.Entities;
using BillingSystem.Domain.ViewModels;
using BillingSystem.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;

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

            var tableCoins = await CountNumberCoinsForUserAsync(userProfiliesVm, (int)emissionAmount);
            foreach (var item in tableCoins)
            {
                var userProfile = await GetUserProfileByName(item.Key);
                if (userProfile == null)
                    continue;
                for (int i = 0; i < item.Value; i++)
                    userProfile.Coins.Add(new CoinDomain
                    {
                        History = $"Эмиссия-{userProfile.Name}"
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

        public async Task<bool> MoveCoins(string srcUser, string dstUser, long amount)
        {
            var srcUserEntity = await GetUserProfileByName(srcUser);
            var dstUserEntity = await GetUserProfileByName(dstUser);

            if (srcUserEntity == null || dstUserEntity == null)
                return false;
            if (srcUserEntity.Amount < amount)
                return false;

            var coinsTransaction = await GetCoinsUserProfile(srcUserEntity.Name, amount);
            if (coinsTransaction == null)
                return false;

            var amountCoins = coinsTransaction.Count();
            await using (await _db.Database.BeginTransactionAsync())
            {
                foreach (var coin in coinsTransaction)
                {
                    var coinEntity = await GetCoinById(coin.Id);
                    if (coinEntity == null)
                        continue;
                    coinEntity.UserProfile = dstUserEntity;
                    StringBuilder sb = new StringBuilder(coin.History);
                    sb.Append($"-{dstUser}");
                    coinEntity.History = sb.ToString();
                }
                srcUserEntity.Amount -= amountCoins;
                dstUserEntity.Amount += amountCoins;
                await _db.SaveChangesAsync();
                await _db.Database.CommitTransactionAsync();
            }

            return true;
        }

        private async Task<Dictionary<string, int>> CountNumberCoinsForUserAsync(List<UserProfileViewModel> userProfilies, int emissionCount)
        {
            var result = new Dictionary<string, int>();
            var totalRaiting = await GetTotalRaiting();
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

        private async Task<double> GetTotalRaiting()
            => await _db.UserProfiles.SumAsync(p => p.Rating);

        private async Task<UserProfileDomain?> GetUserProfileByName(string name)
            => await _db.UserProfiles
                .Include(p => p.Coins)
                .FirstOrDefaultAsync(p => p.Name == name);
        private async Task<CoinDomain?> GetCoinById(int id)
            => await _db.Coins.FirstOrDefaultAsync(c => c.Id == id).ConfigureAwait(false);

        private async Task<IEnumerable<CoinViewModel>?> GetCoinsUserProfile(string nameUserProfile, long amount)
        {
            var userEntity = await GetUserProfileByName(nameUserProfile);
            return userEntity?.Coins.Take((int)amount)
                .Select(c => new CoinViewModel { Id = c.Id, History = c.History });
        }
    }
}
