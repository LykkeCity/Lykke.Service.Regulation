using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Regulation.Models
{
    public class ClientAvailableRegulationModel
    {
        [Required]
        public string ClientId { get; set; }

        [Required]
        public string RegulationId { get; set; }
    }
}
