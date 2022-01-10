using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Extensions.Selectors
{
    public static class ExpeditionSort
    {
        public static IQueryable<Expedition> ApplySort(this IQueryable<Expedition> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Expedition, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.ExpeditionId;
                    break;
                case 2:
                    orderSelector = d => d.Name;
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
