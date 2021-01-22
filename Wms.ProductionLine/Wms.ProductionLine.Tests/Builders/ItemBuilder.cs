using System;
using System.Collections.Generic;
using System.Text;
using Wms.ProductionLine.Domain.Entities;

namespace Wms.ProductionLine.Tests.Builders
{
    public class ItemBuilder
    {
        private Guid _id = Guid.NewGuid();
        private string _description = "Item Default";
        private string _code = "123";
        private int _itemType;

        public Item Build()
        {
            return new Item(_id, _description, _code, _itemType);
        }

        public ItemBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public ItemBuilder WithCode(string code)
        {
            _code = code;
            return this;
        }

        public ItemBuilder WithItemType(int itemType)
        {
            _itemType = itemType;
            return this;
        }
    }
}
