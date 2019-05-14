using System;
using System.Linq.Expressions;

namespace RajsLibs.Repositories.Paging
{
    public interface IPageQuery<TEntity>
        where TEntity : class
    {
        Expression<Func<TEntity, bool>> Predicate { get; set; }

        int Skip { get; set; }

        int Take { get; set; }

        string Order { get; set; }

        bool IsDesc { get; set; }
    }
}
