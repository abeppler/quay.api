using System;

namespace Wms.ProductionLine.Domain.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public Guid WarehouseId { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
    }
}
