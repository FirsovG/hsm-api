using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hsm_api.Models.Messages
{
    public abstract class Message
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MessageId { get; set; }
        public DateTime MessageCreationDate { get; set; } = DateTime.Now.AddMinutes(-1);
    }
}
