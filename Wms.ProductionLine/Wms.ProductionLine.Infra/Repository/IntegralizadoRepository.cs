using Hbsis.Wms.Infra.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wms.ProductionLine.Domain.Entities;
using Wms.ProductionLine.Domain.Intefaces;

namespace Wms.ProductionLine.Infra.Repository
{
    public class IntegralizadoRepository : Repository<Integralizado>, IIntegralizadoRepository
    {
        public IntegralizadoRepository(DbContext context) : base(context)
        {
        }

        public Task<Integralizado> GetWithDetails(Guid id)
        {
            return _set.Include(x => x.Details).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Delete(Integralizado integralizado)
        {
            _set.Remove(integralizado);
        }
    }
}
