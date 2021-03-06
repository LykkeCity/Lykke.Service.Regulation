﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;

namespace Lykke.Service.Regulation.Core.Services
{
    public interface IWelcomeRegulationRuleService
    {
        Task<IWelcomeRegulationRule> GetAsync(string regulationRuleId);

        Task<IEnumerable<IWelcomeRegulationRule>> GetAllAsync();

        Task<IEnumerable<IWelcomeRegulationRule>> GetByCountryAsync(string country);

        Task<IEnumerable<IWelcomeRegulationRule>> GetByRegulationIdAsync(string regulationId);

        Task AddAsync(IWelcomeRegulationRule welcomeRegulationRule);

        Task UpdateAsync(IWelcomeRegulationRule welcomeRegulationRule);

        Task DeleteAsync(string regulationRuleId);
    }
}
