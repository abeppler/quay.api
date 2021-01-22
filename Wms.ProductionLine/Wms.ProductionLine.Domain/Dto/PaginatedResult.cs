using System.Collections.Generic;

namespace Wms.ProductionLine.Domain.Dto
{
    public class PaginatedResult<T>
    {
        public PaginatedResult(int totalItens, IList<T> itens)
        {
            TotalItems = totalItens;
            Items = itens;
        }

        public int TotalItems { get; set; }
        public IList<T> Items { get; set; }
    }
}
