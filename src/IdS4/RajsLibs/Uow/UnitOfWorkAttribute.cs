using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using AspectCore.Extensions.AspectScope;
using Microsoft.Extensions.DependencyInjection;

namespace RajsLibs.Uow
{
    public class UnitOfWorkAttribute : AbstractInterceptorAttribute, IScopeInterceptor
    {
        public Scope Scope { get; set; } = Scope.Aspect;

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            await next(context);

            var manager = context.ServiceProvider.GetService<IUnitOfWorkManager>();
            await manager.CommitAsync();
        }
    }
}
