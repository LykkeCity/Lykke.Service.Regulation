using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Regulation.Models
{
    public class NewWelcomeRegulationRuleModel
    {
        [Required]
        public string Country { get; set; }

        [Required]
        public string RegulationId { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}
