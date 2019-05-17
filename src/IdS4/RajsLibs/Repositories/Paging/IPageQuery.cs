using System;
using System.Linq.Expressions;

namespace RajsLibs.Repositories.Paging
{
    public interface IPageQuery<TEntity>
        where TEntity : class
    {
        Expression<Func<TEntity, bool>> Predicate { get; set; }

        int CurrentPage { get; set; } 

        int PageSize { get; set; }

        string OrderBy { get; set; }

        bool IsDesc { get; set; }

        int Skip { get; }

        int Take { get; }
    }
}
