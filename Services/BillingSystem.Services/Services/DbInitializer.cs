using BillingSystem.DAL.Context;
using BillingSystem.Interfaces;
using BillingSystem.Services.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BillingSystem.Services.Services
{
    public class DbInitializer : IDbInitializer
    {
        private readonly BillingSystemDB _db;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(BillingSystemDB db, ILogger<DbInitializer> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task InitializeAsync(bool RemoveBefore = false, CancellationToken cancel = default)
        {
            _logger.LogInformation("Инициализация БД");

            if (RemoveBefore)
                await RemoveAsync(cancel).ConfigureAwait(false);

            await _db.Database.MigrateAsync(cancel).ConfigureAwait(false);

            await InitializeUserProfiliesAsync(cancel).ConfigureAwait(false);
            _logger.LogInformation("Инициализация БД выполнена успешно");
        }

        public async Task<bool> RemoveAsync(CancellationToken cancel = default)
        {
            _logger.LogInformation("Удаление БД...");

            var result = await _db.Database.EnsureDeletedAsync(cancel).ConfigureAwait(false);

            if (result)
                _logger.LogInformation("Удаление БД выполнено успешно");
            else
                _logger.LogInformation("Удаление БД не требуется");

            return result;
        }

        private async Task InitializeUserProfiliesAsync(CancellationToken cancel)
        {
            if (_db.UserProfiles.Any())
            {
                _logger.LogInformation("Инициализация БД пользовательскими профилями не требуется");
                return;
            }

            await using (await _db.Database.BeginTransactionAsync())
            {
                await _db.AddRangeAsync(TestData.UserProfiles, cancel);

                await _db.SaveChangesAsync(cancel);

                await _db.Database.CommitTransactionAsync(cancel);
            }
            _logger.LogInformation("Инициализация БД пользовательскими профилями выполнена успешно");
        }
    }
}
