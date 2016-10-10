using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Autofac;
using Autofac.Integration.WebApi;
using Owin;
using Swashbuckle.Application;

namespace Valent.Inv
{
    class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            config.Filters.Add(new Valent.Inv.Filters.ValidateModelAttribute());
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Never;
            config.Services.Replace(typeof(IExceptionLogger), new ConsoleExceptionLogger());
            ConfigureAutofac(config);
            ConfigureSwagger(config);

            appBuilder.UseWebApi(config);
            config.EnsureInitialized();
        }

        private void ConfigureSwagger(HttpConfiguration config)
        {
            config.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "Valent Inventory Sample");

                var commentsFileName = Assembly.GetExecutingAssembly().GetName().Name + ".XML";
                var commentsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, commentsFileName);

                c.IncludeXmlComments(commentsFile);
            })
                .EnableSwaggerUi(c =>
                {
                    c.DocExpansion(DocExpansion.Full);
                });
        }

        private void ConfigureAutofac(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterAssemblyModules(typeof(Valent.Inv.Infrastructure.InMemoryInventoryRepository).Assembly);
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
