using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Domain.ViewModels
{
    public class ResponseViewModel
    {
        public bool StatusOperation { get; set; }

        public bool IsStatusUnspecified { get; set; }

        public string StatusMessage { get; set; }
    }
}
