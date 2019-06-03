using System.Collections.Generic;

namespace IdS4.CoreApi.Models.Paging
{
    public class Paged<T>
    {
        public int Total { get; set; }
        public List<T> List { get; set; }

        public static Paged<T> From(List<T> list, int total)
        {
            return new Paged<T>
            {
                List = list,
                Total = total
            };
        }
    }
}
