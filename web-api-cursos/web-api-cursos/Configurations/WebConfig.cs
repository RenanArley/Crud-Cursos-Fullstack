using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_cursos.Configurations
{
    public class WebConfig
    {
        public static string GetConnectionString() 
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["curso"].ConnectionString;
        }
        public static string GetLogFullPath() 
        {
            return System.Configuration.ConfigurationManager.AppSettings["LogFullPath"];           
        }
    }
}