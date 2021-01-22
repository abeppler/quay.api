using Hbsis.Wms.Infra.Domain.Entities;
using System;
using Wms.ProductionLine.Domain.Dto;

namespace Wms.ProductionLine.Domain.Entities
{
    public class User : Entity
    {
        public Guid? WarehouseId { get; private set; }
        public string Name { get; private set; }
        public string Login { get; private set; }

        protected User()
        {
        }

        public User(Guid id, Guid warehouseId, string name, string login)
        {
            SetId(id);
            WarehouseId = warehouseId;
            Name = name;
            Login = login;
        }

        public void UpdateProperties(UserDto userDto)
        {
            WarehouseId = userDto.WarehouseId;
            Name = userDto.Name;
            Login = userDto.Login;
        }
    }
}
