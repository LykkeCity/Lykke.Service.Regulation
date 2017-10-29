using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Regulation.Models
{
    public class WelcomeRegulationRuleModel
    {
        public string Id { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string RegulationId { get; set; }
    }
}
