using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hsm_api.Models.Messages
{
    interface IMessage
    {
        public long MessageId { get; set; }
        public DateTime MessageCreationDate { get; set; }
    }
}
