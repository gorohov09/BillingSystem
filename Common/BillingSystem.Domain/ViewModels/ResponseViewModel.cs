using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Domain.ViewModels
{
    public class ResponseViewModel
    {
        public enum Status
        {
            STATUS_UNSPECIFIED,
            STATUS_OK,
            STATUS_FAILED
        }

        public Status StatusOperation { get; set; }

        public string StatusMessage { get; set; }
    }
}
