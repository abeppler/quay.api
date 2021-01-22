using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Wms.ProductionLine.Globalization;

namespace Wms.ProductionLine.Domain.Entities.History
{
    public enum ConfigurationHistoryType
    {
        [Display(Name = "AddNewConfiguration", ResourceType = typeof(Resources))]
        NewConfiguration = 0,

        [Display(Name = "StatusChanged", ResourceType = typeof(Resources))]
        StatusChanged = 1,
    }
}
