using System.Collections.Generic;

namespace IdS4.Application.Paging
{
    public class Paged<TEntity>
    {
        public List<TEntity> Data { get; set; }

        public int TotalCount { get; set; }
    }
}
