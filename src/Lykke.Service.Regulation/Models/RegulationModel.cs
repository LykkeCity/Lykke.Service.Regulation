﻿using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Regulation.Models
{
    public class RegulationModel
    {
        [Required]
        public string Id { get; set; }
    }
}
