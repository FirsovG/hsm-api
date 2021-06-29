using hsm_api.Infrastructure.JsonConverters;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace hsm_api.Models
{
    public class Webhook
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string CallbackUrl { get; set; }
        [JsonConverter(typeof(PlantEventNameConverter))]
        public string SubscribedPlantEvent { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
