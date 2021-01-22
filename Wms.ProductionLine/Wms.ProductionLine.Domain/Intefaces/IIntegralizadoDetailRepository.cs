using System;
using System.Threading.Tasks;
using Hbsis.Wms.Infra.Domain.Interfaces.Data;
using System;
using Wms.ProductionLine.Domain.Entities;

namespace Wms.ProductionLine.Domain.Intefaces
{
    public interface IIntegralizadoDetailRepository : IRepository<IntegralizadoDetail>, IAsyncRepository<IntegralizadoDetail>
    {
        Task Delete(Guid[] detailsToRemove);
    }
}
