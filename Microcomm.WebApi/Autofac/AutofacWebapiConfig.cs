using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
 

namespace Microcomm.Web.Http.Autofac
{
    public class AutofacWebapiConfig
    {
        public static IContainer Container { get; private set; }

        public static void Initialize(HttpConfiguration config,string filter,string[] registTypeSuffixs)
        {
            Initialize(config, RegisterServices(new ContainerBuilder(),filter,registTypeSuffixs));  
        }


        private static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder, string filter, string[] registTypeSuffixs)
        {
           
            Container=  builder.RegistComponentsWithSpecifiedSuffix(filter,registTypeSuffixs);
            return Container;
        }

       

    }
}