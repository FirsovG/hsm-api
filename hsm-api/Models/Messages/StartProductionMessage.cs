using hsm_api.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace hsm_api.Models.Messages
{
    public class StartProductionMessage : Message
    {
        public string CoilId { get; set; }
        public DateTime ProductionStartDate { get; set; } = DateTime.Now.AddSeconds(-95);
    }
}
