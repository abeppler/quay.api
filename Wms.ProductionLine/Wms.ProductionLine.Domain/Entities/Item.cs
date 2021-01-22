using Hbsis.Wms.Infra.Domain.Entities;
using System;
using Wms.ProductionLine.Domain.Dto;

namespace Wms.ProductionLine.Domain.Entities
{
    public class Item : Entity
    {
        public string Description { get; private set; }
        public string Code { get; private set; }
        public int? ItemType { get; private set; }

        protected Item()
        {
        }

        public Item(Guid id, string description, string code, int? itemType)
        {
            SetId(id);
            Description = description;
            Code = code;
            ItemType = itemType;
        }

        public void UpdateProperties(ItemDto itemDto)
        {
            Description = itemDto.Description;
            Code = itemDto.Code;
            ItemType = itemDto.ItemType;
        }
    }
}
