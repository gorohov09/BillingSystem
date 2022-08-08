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

        public async Task<ResponseViewModel> CoinsEmission(long emissionAmount)
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
            return new ResponseViewModel { StatusOperation = true, StatusMessage = "Эмиссия денег выполнена успешно"};
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

        public async Task<ResponseViewModel> MoveCoins(string srcUser, string dstUser, long amount)
        {
            var srcUserEntity = await GetUserProfileByName(srcUser);
            var dstUserEntity = await GetUserProfileByName(dstUser);

            if (srcUserEntity == null || dstUserEntity == null)
                return new ResponseViewModel { StatusOperation = false, StatusMessage = "Один из пользователей не найден в БД"};
            if (srcUserEntity.Amount < amount)
                return new ResponseViewModel { StatusOperation = false, StatusMessage = $"У{srcUser} на балансе меньше {amount}м." };

            var coinsTransaction = await GetCoinsUserProfile(srcUserEntity.Name, amount);
            if (coinsTransaction == null)
                return new ResponseViewModel { IsStatusUnspecified = true };

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

            return new ResponseViewModel { StatusOperation = true, 
                StatusMessage = $"Перемещение {amount} мон. от {srcUser} к {dstUser} выполнено успешно" };
        }

        public async Task<CoinViewModel> LongestHistoryCoin()
        {
            var coinsEntities = await GetCoins();
            var historyEntities = coinsEntities.Select(x => new CoinViewModel { Id = x.Id, History = x.History}).ToList();
            var maxId = GetIdCoinByLongestHistory(historyEntities);
            if (maxId == 0)
                return null;
            var coin = await GetCoinById(maxId);
            return coin != null ? new CoinViewModel { Id = coin.Id, History = coin.History} : null;
        }

        private int GetIdCoinByLongestHistory(List<CoinViewModel> coinsVm)
        {
            var maxHistory = coinsVm.Max(c => c.History.Split("-").Length);
            var coinVm = coinsVm.FirstOrDefault(c => c.History.Split("-").Length == maxHistory);
            return coinVm == null ? 0 : coinVm.Id;
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

        private async Task<List<UserProfileDomain>> GetUserProfilies() => 
            await _db.UserProfiles
            .ToListAsync()
            .ConfigureAwait(false);

        private async Task<List<CoinDomain>> GetCoins() => 
            await _db.Coins
            .ToListAsync()
            .ConfigureAwait(false);

        private async Task<double> GetTotalRaiting() => 
            await _db.UserProfiles
            .SumAsync(p => p.Rating)
            .ConfigureAwait(false);

        private async Task<UserProfileDomain?> GetUserProfileByName(string name) => 
            await _db.UserProfiles
                .Include(p => p.Coins)
                .FirstOrDefaultAsync(p => p.Name == name).ConfigureAwait(false);
        private async Task<CoinDomain?> GetCoinById(int id) => 
            await _db.Coins
            .FirstOrDefaultAsync(c => c.Id == id)
            .ConfigureAwait(false);

        private async Task<IEnumerable<CoinViewModel>?> GetCoinsUserProfile(string nameUserProfile, long amount)
        {
            var userEntity = await GetUserProfileByName(nameUserProfile);
            return userEntity?.Coins.Take((int)amount)
                .Select(c => new CoinViewModel { Id = c.Id, History = c.History });
        } 
    }
}
