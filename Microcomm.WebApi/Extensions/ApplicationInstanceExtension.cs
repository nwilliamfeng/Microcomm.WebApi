using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microcomm.Web.Http
{
     public static class ApplicationInstanceExtension
    {
        public static  Assembly GetWebEntryAssembly(this System.Web.HttpApplication application)
        {         
            var type = application.GetType();
            while (type != null && type.Namespace == "ASP")
            {
                type = type.BaseType;
            }

            return type == null ? null : type.Assembly;
        }
    }
}
