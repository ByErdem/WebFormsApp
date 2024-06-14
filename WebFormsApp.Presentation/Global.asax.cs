using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebFormsApp.Presentation.Models.Profiles;
using WebFormsApp.Service.Abstract;
using WebFormsApp.Service.Concrete;

namespace WebFormsApp.Presentation
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var builder = new ContainerBuilder();

            builder.RegisterInstance(ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["redisConnectionString"]))
                   .As<IConnectionMultiplexer>()
                   .SingleInstance();

            builder.Register(c => c.Resolve<ConnectionMultiplexer>().GetDatabase())
                   .As<IDatabase>();


            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            var mapper = config.CreateMapper();
            builder.RegisterInstance(mapper).As<IMapper>().SingleInstance();
            builder.RegisterType<ConfigurationService>().As<IConfigurationService>().AsSelf().SingleInstance();
            builder.RegisterType<EncryptionManager>().As<IEncryptionService>().AsSelf().SingleInstance();
            builder.RegisterType<TokenManager>().As<ITokenService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<RedisCacheManager>().As<IRedisCacheService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<HttpManager>().As<IHttpService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<SessionManager>().As<ISessionService>().SingleInstance();
            builder.RegisterType<DBContextEntity>().As<IDBContextEntity>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<StudentManager>().As<IStudentService>().AsSelf().InstancePerLifetimeScope();


            builder.RegisterControllers(typeof(Global).Assembly);

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                   .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic)
                   .As<Profile>();

            builder.Register(c => new MapperConfiguration(cfg =>
            {
                foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            })).AsSelf().SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve)).As<IMapper>().InstancePerLifetimeScope();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}