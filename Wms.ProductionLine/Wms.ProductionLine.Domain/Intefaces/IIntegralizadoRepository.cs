using Hbsis.Wms.Infra.Domain.Interfaces.Data;
using System;
using System.Threading.Tasks;
using Wms.ProductionLine.Domain.Entities;

namespace Wms.ProductionLine.Domain.Intefaces
{
    public interface IIntegralizadoRepository: IRepository<Integralizado>, IAsyncRepository<Integralizado>
    {
        Task<Integralizado> GetWithDetails(Guid id);
        Task Delete(Integralizado integralizado);
    }
}
