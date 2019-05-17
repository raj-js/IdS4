using System.Collections;
using System.Collections.Generic;

namespace RajsLibs.Repositories.Paging
{
    public partial class PageResult<TEntity> : IPageResult<TEntity>
    {
        public List<TEntity> Data { get; set; }

        public int TotalCount { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalPage => TotalCount % PageSize == 0 ? TotalCount / PageSize : TotalCount / PageSize + 1;
    }
}
