﻿using System.Collections.Generic;

namespace Lykke.Service.Regulation.Client.Models
{
    public class WelcomeRegulationRuleModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IList<string> Countries { get; set; }

        public string RegulationId { get; set; }

        public bool Active { get; set; }

        public int Priority { get; set; }
    }
}
