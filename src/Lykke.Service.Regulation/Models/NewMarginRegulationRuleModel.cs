using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Regulation.Models
{
    public class NewMarginRegulationRuleModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(1)]
        public List<string> Countries { get; set; }

        [Required]
        public string RegulationId { get; set; }
        
        [Required]
        public int Priority { get; set; }
    }
}
