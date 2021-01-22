using Hbsis.Wms.Infra.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Wms.ProductionLine.Domain.Entities;
using Wms.ProductionLine.Domain.Intefaces;

namespace Wms.ProductionLine.Infra.Repository
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(DbContext context) : base(context)
        {
        }
    }
}
