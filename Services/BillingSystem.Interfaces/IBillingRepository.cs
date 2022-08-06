using BillingSystem.Domain.ViewModels;

namespace BillingSystem.Interfaces
{
    public interface IBillingRepository
    {
        Task<IEnumerable<UserProfileViewModel>> GetUserProfiliesVm();

        Task<bool> CoinsEmission(long emissionAmount);

        Task<bool> MoveCoins(string srcUser, string dstUser, long amount);
    }
}
