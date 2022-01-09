using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Extensions.Selectors
{
    public static class HerbariumSort
    {
        public static IQueryable<Herbarium> ApplySort(this IQueryable<Herbarium> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Herbarium, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.InventoryNumber;
                    break;
                case 2:
                    orderSelector = d => d.YearOfCollection;
                    break;
            }
            if (orderSelector != null)
            {
                query = ascending ?
                       query.OrderBy(orderSelector) :
                       query.OrderByDescending(orderSelector);
            }

            return query;
        }
    }
}
