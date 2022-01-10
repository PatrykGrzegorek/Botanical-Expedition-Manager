using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Extensions.Selectors
{
    public static class SpeciesSort
    {
        public static IQueryable<Species> ApplySort(this IQueryable<Species> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Species, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.LatinName;
                    break;
                case 2:
                    orderSelector = d => d.IsEndemic;
                    break;
                case 3:
                    orderSelector = d => d.IsAutochthonous;
                    break;
                case 4:
                    orderSelector = d => d.IsWeed;
                    break;
                case 5:
                    orderSelector = d => d.IsInvasive;
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
