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
    public static class AutofacWebapiConfig
    {
        public static IContainer Container { get;  private set; }

        public static void Initialize(HttpApplication application,  HttpConfiguration config, string filter, string[] registTypeSuffixs)
        {
            var container = RegisterServices(application, new ContainerBuilder(), filter, registTypeSuffixs);        
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

 

        private static IContainer RegisterServices(HttpApplication application, ContainerBuilder builder, string filter, string[] registTypeSuffixs)
        {

            Container = builder.RegistComponentsWithSpecifiedSuffix(application, filter, registTypeSuffixs);
            return Container;
        }

        

    }
}