using System.ComponentModel.DataAnnotations;
using Lykke.Service.Regulation.Core.Domain;

namespace Lykke.Service.Regulation.Models
{
    public class RegulationDto : IRegulation
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool RequiresKYC { get; set; }
    }
}
