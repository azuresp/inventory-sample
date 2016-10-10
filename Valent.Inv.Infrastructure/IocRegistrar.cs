using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Valent.Inv.Domain;

namespace Valent.Inv.Infrastructure
{
    public class IocRegistrar : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryInventoryRepository>().As<IInventoryRepository>().SingleInstance();
            builder.RegisterType<ConsoleNotifyRepository>().As<INotifyRepository>().SingleInstance();
        }
    }
}
