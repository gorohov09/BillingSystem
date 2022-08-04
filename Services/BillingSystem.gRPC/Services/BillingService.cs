using Billing;
using BillingSystem.DAL.Context;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace BillingSystem.gRPC.Services
{
    public class BillingService : Billing.Billing.BillingBase
    {
        private readonly ILogger<BillingService> _logger;
        private readonly BillingSystemDB _db;

        public BillingService(ILogger<BillingService> logger, BillingSystemDB db)
        {
            _logger = logger;
            _db = db;
        }

        public override async Task ListUsers(None request, IServerStreamWriter<UserProfile> responseStream, ServerCallContext context)
        {
            var userProfilies = await _db.UserProfiles.ToListAsync();
            foreach (var profile in userProfilies)
            {
                await responseStream.WriteAsync(new UserProfile
                {
                    Name = profile.Name,
                    Amount = profile.Amount ?? 0,
                });
            }
        }

        public override Task<Response> CoinsEmission(EmissionAmount request, ServerCallContext context)
        {
            return base.CoinsEmission(request, context);
        }

        public override Task<Response> MoveCoins(MoveCoinsTransaction request, ServerCallContext context)
        {
            return base.MoveCoins(request, context);
        }

        public override Task<Coin> LongestHistoryCoin(None request, ServerCallContext context)
        {
            return base.LongestHistoryCoin(request, context);
        }
    }
}
