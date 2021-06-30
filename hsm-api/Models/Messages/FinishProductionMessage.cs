using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hsm_api.Models.Messages
{
    public class FinishProductionMessage : Message
    {
        public string CoilId { get; set; }
        public DateTime ProductionFinishDate { get; set; } = DateTime.Now.AddSeconds(-95);
        public float Width { get; set; }
        public float Thickness { get; set; }
        public float Weight { get; set; }
    }
}
