using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Regulation.Models
{
    public class NewRegulationModel
    {
        [Required]
        public string Id { get; set; }

        public string ProfileType { get; set; }

        public string TermsOfUseUrl { get; set; }

        public string RiskDescriptionUrl { get; set; }

        public string MarginTradingConditions { get; set; }
    }
}
