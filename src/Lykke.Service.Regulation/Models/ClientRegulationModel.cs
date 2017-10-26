using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lykke.Service.Regulation.Models
{
    public class ClientRegulationModel
    {
        [Required]
        public string ClientId { get; set; }

        [Required]
        public string RegulationId { get; set; }
    }
}
