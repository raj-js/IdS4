using System;
using System.Linq;
using System.Linq.Expressions;

namespace RajsLibs.Repositories.Paging
{
    public partial class PageQuery<TEntity> : IPageQuery<TEntity>
        where TEntity : class
    {
        public class Builder
        {
            private readonly IPageQuery<TEntity> _query = null;

            public Builder()
            {
                _query = new PageQuery<TEntity>
                {
                    Predicate = s => true
                };
            }

            public Builder Filter(Expression<Func<TEntity, bool>> predicate)
            {
                var invoke = Expression.Invoke(predicate, _query.Predicate.Parameters.Cast<Expression>());

                _query.Predicate = Expression.Lambda<Func<TEntity, bool>>                    (Expression.And(_query.Predicate.Body, invoke), _query.Predicate.Parameters);                return this;            }

            public Builder Filter(bool condition, Expression<Func<TEntity, bool>> predicate)
            {
                if (!condition) return this;

                return Filter(predicate);
            }

            public Builder OrderBy(string order)
            {
                _query.Order = order;
                return this;
            }

            /// <summary>
            /// 升序
            /// </summary>
            /// <returns></returns>
            public Builder Ascending()
            {
                _query.IsDesc = false;
                return this;
            }

            /// <summary>
            /// 降序
            /// </summary>
            /// <returns></returns>
            public Builder Descending()
            {
                _query.IsDesc = true;
                return this;
            }

            public Builder Skip(int skip)
            {
                _query.Skip = skip;
                return this;
            }

            public Builder Take(int take)
            {
                _query.Take = take;
                return this;
            }

            public IPageQuery<TEntity> Build()
            {
                return _query;
            }
        }
    }
}
