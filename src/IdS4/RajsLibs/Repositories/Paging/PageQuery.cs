using System;
using System.Linq.Expressions;

namespace RajsLibs.Repositories.Paging
{
    public partial class PageQuery<TEntity> : IPageQuery<TEntity>
        where TEntity : class
    {
        public Expression<Func<TEntity, bool>> Predicate { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

        public string Order { get; set; }

        public bool IsDesc { get; set; }
    }
}
