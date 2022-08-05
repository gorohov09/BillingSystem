using BillingSystem.Domain.ViewModels;

namespace BillingSystem.Interfaces
{
    public interface IBillingRepository
    {
        Task<IEnumerable<UserProfileViewModel>> GetUserProfiliesVm();

        Task
    }
}
