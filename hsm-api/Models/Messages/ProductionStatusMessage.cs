using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hsm_api.Models.Messages
{
    public class ProductionStatusMessage : Message
    {
        public DateTime StateDate { get; set; }
        public float MillSpeed { get; set; }
        public float CoilingSpeed { get; set; }
    }
}
