using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Regulation.Models
{
    public class ClientRegulationModel
    {
        public string Id { get; set; }

        [Required]
        public string ClientId { get; set; }

        [Required]
        public string RegulationId { get; set; }

        [Required]
        public bool Kyc { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}
