﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac;
using Autofac.Integration.WebApi;
 

namespace Microcomm.Web.Http.Autofac
{
    public static class AutofacExtension
    {

        public static IContainer RegistComponentsWithSpecifiedSuffix(this ContainerBuilder builder,string filter, params string[] typeSuffixs)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            var amblys = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.ManifestModule.Name.Contains(filter)).ToList();

            typeSuffixs.ToList().ForEach(suffix =>
            {
                var ambly = amblys.FirstOrDefault(x => x.GetTypes().Where(t => t.Name.EndsWith(suffix) && t.IsClass && !t.IsAbstract).Count() > 0);
                if (ambly != null)
                    builder.RegisterAssemblyTypes(ambly).Where(t => t.Name.EndsWith(suffix)).AsImplementedInterfaces();
            });

        
            return builder.Build();
        }
    }
}