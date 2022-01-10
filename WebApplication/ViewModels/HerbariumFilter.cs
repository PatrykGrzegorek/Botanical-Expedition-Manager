//using System;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Globalization;
//using WebApplication.Models;

//namespace WebApplication.ViewModels
//{
//    public class HerbariumFilter: IPageFilter
//    {
//        public int? CollectionId { get; set; }
//        public string CollectionName { get; set; }
//        [DataType(DataType.Date)]
//        public DateTime? DateFrom { get; set; }
//        [DataType(DataType.Date)]
//        public DateTime? DateTo { get; set; }

//        public bool IsEmpty()
//        {
//            bool active = CollectionId.HasValue
//                          || DateFrom.HasValue
//                          || DateTo.HasValue;
//            return !active;
//        }

//        public override string ToString()
//        {
//            return string.Format("{0}-{1}-{2}-{3}-{4}",
//                CollectionId,
//                DateFrom?.ToString("dd.MM.yyyy"),
//                DateTo?.ToString("dd.MM.yyyy"));
//        }

//        public static HerbariumFilter FromString(string s)
//        {
//            var filter = new HerbariumFilter();
//            if (!string.IsNullOrEmpty(s))
//            {
//                string[] arr = s.Split('-', StringSplitOptions.None);

//                if (arr.Length == 5)
//                {
//                    filter.CollectionId = string.IsNullOrWhiteSpace(arr[0]) ? new int?() : int.Parse(arr[0]);
//                    filter.DateFrom = string.IsNullOrWhiteSpace(arr[1]) ? new DateTime?() : DateTime.ParseExact(arr[1], "dd.MM.yyyy", CultureInfo.InvariantCulture);
//                    filter.DateTo = string.IsNullOrWhiteSpace(arr[2]) ? new DateTime?() : DateTime.ParseExact(arr[2], "dd.MM.yyyy", CultureInfo.InvariantCulture);
//                }
//            }
//            return filter;
//        }

//        public IQueryable<ViewHerbariumInfo> Apply(IQueryable<ViewHerbariumInfo> query)
//        {
//            if (CollectionId.HasValue)
//            {
//                query = query.Where(d => d.CollectionId == CollectionId.Value);
//            }
//            if (DateFrom.HasValue)
//            {
//                query = query.Where(d => d.YearOfCollection >= DateFrom.Value);
//            }
//            if (DateFrom.HasValue)
//            {
//                query = query.Where(d => d.YearOfCollection <= DateFrom.Value);
//            }
//            return query;
//        }
//    }
//}
