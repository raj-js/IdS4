using System;
using System.Linq.Expressions;

namespace IdS4.Application.Paging
{
    public class PageQuery<TEntity>
    {
        public Expression<Func<TEntity, bool>> Filters { get; set; } = _ => true;

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }      

        public string OrderBy { get; set; }

        public int Skip => (PageSize - 1) * CurrentPage;
    }
}
