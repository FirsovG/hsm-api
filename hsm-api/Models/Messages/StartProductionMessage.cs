using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hsm_api.Models.Messages
{
    public class StartProductionMessage : Message
    {
        public string CoilId { get; set; }
        public DateTime ProductionStartDate { get; set; }
    }
}
