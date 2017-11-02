using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Regulation.Models
{
    public class NewClientRegulationModel
    {
        [Required]
        public string ClientId { get; set; }

        [Required]
        public string RegulationId { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}
