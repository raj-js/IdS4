using System.Collections.Generic;

namespace RajsLibs.Repositories.Paging
{
    public partial class PageResult<TEntity>
    {
        public class Builder
        {
            private readonly IPageResult<TEntity> _result = null;

            public Builder(List<TEntity> data)
            {
                _result = new PageResult<TEntity> {Data = data};
            }

            public Builder CurrentPage(int currentPage)
            {
                _result.CurrentPage = currentPage;
                return this;
            }

            public Builder PageSize(int pageSize)
            {
                _result.PageSize = pageSize;
                return this;
            }

            public Builder TotalCount(int totalCount)
            {
                _result.TotalCount = totalCount;
                return this;
            }

            public IPageResult<TEntity> Build()
            {
                return _result;
            }
        }
    }
}
