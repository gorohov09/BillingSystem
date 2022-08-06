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
            if (response)
                return await Task.FromResult(new Response { Status = Response.Types.Status.Ok, Comment = "Эмиссия выполнена успешно"}).ConfigureAwait(false);
            else
                return await Task.FromResult(new Response { Status = Response.Types.Status.Failed, Comment = "Эмиссия не выполнена" }).ConfigureAwait(false);
        }

        public override async Task<Response> MoveCoins(MoveCoinsTransaction request, ServerCallContext context)
        {
            var response = await _billingRepository.MoveCoins(request.SrcUser, request.DstUser, request.Amount);
            if (response)
                return await Task.FromResult(new Response { Status = Response.Types.Status.Ok, 
                    Comment = string.Format("Перемещение {0} мон. от {1} к {2} выполнено успешно", request.Amount, request.SrcUser, 
                    request.DstUser) }).ConfigureAwait(false);
            else
                return await Task.FromResult(new Response { Status = Response.Types.Status.Failed, 
                    Comment = string.Format("Перемещение {0} мон. от {1} к {2} не выполнено", request.Amount, request.SrcUser,
                    request.DstUser)
                }).ConfigureAwait(false);
        }

        public override Task<Coin> LongestHistoryCoin(None request, ServerCallContext context)
        {
            return base.LongestHistoryCoin(request, context);
        }
    }
}
