using System;
using System.Collections.Generic;
using System.Text;

namespace Wms.ProductionLine.Domain.Dto.Filter
{
    public class FilterBase
    {
        public int PageIndex { get; set; }
        public int ItemsByPage { get; set; }
    }
}
