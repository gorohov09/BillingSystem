using Billing;
using BillingSystem.Interfaces;
using Grpc.Core;

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

        public override async Task<Response> CoinsEmission(EmissionAmount request, ServerCallContext context)
        {
            var response = await _billingRepository.CoinsEmission(request.Amount);
            return await Task.FromResult(new Response
            {
                Status = response.IsStatusUnspecified ? Response.Types.Status.Unspecified : 
                (response.StatusOperation ? Response.Types.Status.Ok : Response.Types.Status.Failed),
                Comment = response.StatusMessage
            }).ConfigureAwait(false);
        }

        public override async Task<Response> MoveCoins(MoveCoinsTransaction request, ServerCallContext context)
        {
            var response = await _billingRepository.MoveCoins(request.SrcUser, request.DstUser, request.Amount);
            return await Task.FromResult(new Response
            {
                Status = response.IsStatusUnspecified ? Response.Types.Status.Unspecified : 
                (response.StatusOperation ? Response.Types.Status.Ok : Response.Types.Status.Failed),
                Comment = response.StatusMessage
            }).ConfigureAwait(false);
        }

        public override async Task<Coin> LongestHistoryCoin(None request, ServerCallContext context)
        {
            var response = await _billingRepository.LongestHistoryCoin();
            return await Task.FromResult(new Coin { Id = response.Id, History = response.History}).ConfigureAwait(false);
        }
    }
}
