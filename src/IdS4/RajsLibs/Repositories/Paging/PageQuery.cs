using System;
using System.Linq.Expressions;

namespace RajsLibs.Repositories.Paging
{
    public partial class PageQuery<TEntity> : IPageQuery<TEntity>
        where TEntity : class
    {
        public Expression<Func<TEntity, bool>> Predicate { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public string OrderBy { get; set; }

        public bool IsDesc { get; set; }

        public int Skip => (CurrentPage - 1) * PageSize;

        public int Take => PageSize;
    }
}
