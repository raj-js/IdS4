using Autofac;
using IdS4.Application.Queries;

namespace IdS4.CoreApi.Configuration
{
    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ResourceQueries>()
                .As<IResourceQueries>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ClientQueries>()
                .As<IClientQueries>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserQueries>()
                .As<IUserQueries>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RoleQueries>()
                .As<IRoleQueries>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ScopeQueries>()
                .As<IScopeQueries>()
                .InstancePerLifetimeScope();
        }
    }
}
