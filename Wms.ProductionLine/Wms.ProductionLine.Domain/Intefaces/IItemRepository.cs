using Hbsis.Wms.Infra.Domain.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Wms.ProductionLine.Domain.Entities;

namespace Wms.ProductionLine.Domain.Intefaces
{
    public interface IItemRepository: IRepository<Item>, IAsyncRepository<Item>
    {
    }
}
