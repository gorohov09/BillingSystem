using BillingSystem.DAL.Context;
using BillingSystem.Domain.Entities;
using BillingSystem.Domain.ViewModels;
using BillingSystem.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        public async Task<IEnumerable<UserProfileViewModel>> GetUserProfiliesVm()
        {
            var userProfiliesVm = await GetUserProfilies();
            return userProfiliesVm.Select(userProfile => new UserProfileViewModel
            {
                Name = userProfile.Name,
                Amount = userProfile.Amount ?? 0,
            });
        }

        private async Task<IEnumerable<UserProfileDomain>> GetUserProfilies()
            => await _db.UserProfiles.ToListAsync();
    }
}
