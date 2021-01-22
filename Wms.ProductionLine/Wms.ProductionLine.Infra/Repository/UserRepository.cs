using Hbsis.Wms.Infra.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Wms.ProductionLine.Domain.Entities;
using Wms.ProductionLine.Domain.Intefaces;

namespace Wms.ProductionLine.Infra.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }
    }
}
