using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireTest1.Model
{
    public class CreateMasterPromoCodeRequest
    {
        public string Title { get; set; }
         public string Code { get; set; }
        public DateTime ActiveDateTimeFrom { get; set; }
        public DateTime ActiveDateTimeTo { get; set; }
    }
}
