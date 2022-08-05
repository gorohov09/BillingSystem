using BillingSystem.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Interfaces
{
    public interface IBillingRepository
    {
        Task<IEnumerable<UserProfileViewModel>> GetUserProfiliesVm();
    }
}
