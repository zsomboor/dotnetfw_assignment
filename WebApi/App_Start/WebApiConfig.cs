using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApi.Data;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            JsonSerializerSettings settings = config.Formatters.JsonFormatter.SerializerSettings;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            using(MedStationContext dbContext = new MedStationContext())
            {
                dbContext.Database.EnsureCreated();
            }
            //IsoDateTimeConverter dateConverter = new IsoDateTimeConverter
            //{
            //    DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffff'Z'"
            //};
            //settings.Converters.Add(dateConverter);

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
        }
    }
}
