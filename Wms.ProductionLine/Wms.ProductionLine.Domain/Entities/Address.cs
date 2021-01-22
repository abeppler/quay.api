using Hbsis.Wms.Infra.Domain.Entities;
using System;
using Wms.ProductionLine.Domain.Dto;

namespace Wms.ProductionLine.Domain.Entities
{
    public class Address : Entity
    {
        protected Address()
        {
        }

        public Address(AddressDto addressDto)
        {
            SetId(addressDto.Id);
            WarehouseId = addressDto.WarehouseId;
            Code = addressDto.Code;
            Description = addressDto.Description;
            Enabled = addressDto.Enabled;
            Sequence = addressDto.Sequence;
        }
        public Guid WarehouseId { get; private set; }
        public string Code { get; private set; }
        public string Description { get; private set; }
        public bool Enabled { get; private set; }
        public int Sequence { get; private set; }

        public void UpdateProperties(AddressDto addressDto)
        {
            WarehouseId = addressDto.WarehouseId;
            Code = addressDto.Code;
            Description = addressDto.Description;
            Enabled = addressDto.Enabled;
            Sequence = addressDto.Sequence;
        }
    }
}
