using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hsm_api.Models
{
    public class Webhook
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string CallbackUrl { get; set; }
        public string SubscribedPlantEvent { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
