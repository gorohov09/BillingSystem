using Billing;
using BillingSystem.Interfaces;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace BillingSystem.gRPC.Services
{
    public class BillingService : Billing.Billing.BillingBase
    {
        private readonly ILogger<BillingService> _logger;
        private readonly IBillingRepository _billingRepository;

        public BillingService(ILogger<BillingService> logger, IBillingRepository billingRepository)
        {
            _logger = logger;
            _billingRepository = billingRepository;
        }

        public override async Task ListUsers(None request, IServerStreamWriter<UserProfile> responseStream, ServerCallContext context)
        {
            var userProfilies = await _billingRepository.GetUserProfiliesVm();
            foreach (var profile in userProfilies)
            {
                await responseStream.WriteAsync(new UserProfile
                {
                    Name = profile.Name,
                    Amount = (long)profile.Amount,
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
