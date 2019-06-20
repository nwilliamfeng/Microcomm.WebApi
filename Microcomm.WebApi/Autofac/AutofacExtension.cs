using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using Autofac;
using Autofac.Integration.WebApi;
 

namespace Microcomm.Web.Http.Autofac
{
    public static class AutofacExtension
    {

      

        public static IContainer RegistComponentsWithSpecifiedSuffix(this ContainerBuilder builder,HttpApplication application , string filter, params string[] typeSuffixs)
        {

            builder.RegisterApiControllers(application.GetWebEntryAssembly());
            //注意用AppDomain.CurrentDomain.GetAssemblies() 在debug模式下有时会无法加载dll，这里用BuildManager.GetReferencedAssemblies()进行替换
            var amblys = BuildManager.GetReferencedAssemblies().OfType<Assembly>().Where(x => x.ManifestModule.Name.Contains(filter)).ToList();

            typeSuffixs.ToList().ForEach(suffix =>
            {
                var ambly = amblys.FirstOrDefault(x => x.GetTypes().Where(t => t.Name.EndsWith(suffix) && t.IsClass && !t.IsAbstract).Count() > 0);
                if (ambly != null)
                    builder.RegisterAssemblyTypes(ambly).Where(t => t.Name.EndsWith(suffix)).AsSelf().AsImplementedInterfaces();
            });


            return builder.Build();
        }
    }
}