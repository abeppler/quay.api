using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hbsis.Wms.Infra.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Wms.ProductionLine.Domain.Entities;
using Wms.ProductionLine.Domain.Intefaces;

namespace Wms.ProductionLine.Infra.Repository
{
    public class IntegralizadoDetailRepository : Repository<IntegralizadoDetail>, IIntegralizadoDetailRepository
    {
        public IntegralizadoDetailRepository(DbContext context) : base(context)
        {
        }

        public async Task Delete(Guid[] detailsToRemove)
        {
            var details = GetAll().Where(d => detailsToRemove.Contains(d.Id));

            foreach (var detail in details)            
                _set.Remove(detail);            
        }

        public Task<List<IntegralizadoDetail>> GetByIntegralizadoId(Guid intergralizadoId)
        {
            return GetAll().Where(d => d.IntegralizadoId == intergralizadoId).ToListAsync();
        }
    }
}
