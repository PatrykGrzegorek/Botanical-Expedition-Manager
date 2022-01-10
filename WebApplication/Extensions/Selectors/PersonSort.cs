using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Extensions.Selectors
{
    public static class PersonSort
    {
        public static IQueryable<Person> ApplySort(this IQueryable<Person> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Person, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.PersonId;
                    break;
                case 2:
                    orderSelector = d => d.Name;
                    break;
                case 3:
                    orderSelector = d => d.LastName;
                    break;
                case 4:
                    orderSelector = d => d.Title;
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
