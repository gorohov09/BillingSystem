using Billing;
using Grpc.Core;

namespace BillingSystem.gRPC.Services
{
    public class BillingService : Billing.Billing.BillingBase
    {
        private readonly ILogger<BillingService> _logger;

        public BillingService(Logger<BillingService> logger)
        {
            _logger = logger;
        }

        public override Task ListUsers(None request, IServerStreamWriter<UserProfile> responseStream, ServerCallContext context)
        {
            return base.ListUsers(request, responseStream, context);
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
