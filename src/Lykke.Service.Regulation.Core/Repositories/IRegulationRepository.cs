﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;

namespace Lykke.Service.Regulation.Core.Repositories
{
    public interface IRegulationRepository
    {
        Task<IRegulation> GetAsync(string regulationId);

        Task<IEnumerable<IRegulation>> GetAllAsync();

        Task AddAsync(IRegulation regulation);

        Task RemoveAsync(string regulationId);

        Task UpdateAsync(IRegulation regulation);
    }
}
