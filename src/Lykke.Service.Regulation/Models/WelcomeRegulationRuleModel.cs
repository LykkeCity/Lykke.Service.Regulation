using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Regulation.Models
{
    public class WelcomeRegulationRuleModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public List<string> Countries { get; set; }

        [Required]
        public string RegulationId { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public int Priority { get; set; }
    }
}
