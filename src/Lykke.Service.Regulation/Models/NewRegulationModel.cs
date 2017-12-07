using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Regulation.Models
{
    public class NewRegulationModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string ProfileType { get; set; }
    }
}
