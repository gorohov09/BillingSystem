using BillingSystem.Domain.ViewModels;

namespace BillingSystem.Interfaces
{
    public interface IBillingRepository
    {
        Task<IEnumerable<UserProfileViewModel>> GetUserProfiliesVm();

        Task<ResponseViewModel> CoinsEmission(long emissionAmount);

        Task<ResponseViewModel> MoveCoins(string srcUser, string dstUser, long amount);

        Task<CoinViewModel> LongestHistoryCoin();
    }
}
