using Hbsis.Wms.Infra.Domain.Interfaces.Data;
using Wms.ProductionLine.Domain.Entities;

namespace Wms.ProductionLine.Domain.Intefaces
{
    public interface IUserRepository : IRepository<User>, IAsyncRepository<User>
    {
    }
}
