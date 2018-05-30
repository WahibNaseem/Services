using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApiContrib.Formatting.Jsonp;

namespace EmpService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            //1 Approach to handle cross origin domain
            /*Need to Install Nuget package as well for jsonp json padding */
            //var jsonpformatter = new JsonpMediaTypeFormatter(config.Formatters.JsonFormatter);
            //config.Formatters.Insert(0, jsonpformatter);

            // 2 Approach to handle cross origin domain
            /* Install Nuget Package Microsoft.Aspnet.webapi.cors*/
            /*EnableCorsAttribute cors = new EnableCorsAttribute("*","*","*");
             */ /*Its enable cors globally in the application
               but if we want to enable the cors to a specific controller */

            //config.EnableCors(cors);
            config.EnableCors();




            /*Format the Code and convert Pascal Case to Camel Case */
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //Register the CustomJsonFormatter class
            config.Formatters.Add(new CustomJsonFormatter());
        }
    }

    /* 2 Approach */ /* We need to register this class as well */
    public class CustomJsonFormatter : JsonMediaTypeFormatter
    {
        public CustomJsonFormatter()
        {
            this.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
    }


}
