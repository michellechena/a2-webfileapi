using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ECommunicationWebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            config.Routes.MapHttpRoute(
                name: "GetFolderWithDetails",
                routeTemplate: "api/Email/GetFolderWithDetails",
                defaults: new { controller = "Email", action = "GetFolderWithDetails", id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
               name: "GetFolderByUserMailbox",
               routeTemplate: "api/Email/GetFolderByUserMailbox",
               defaults: new { controller = "Email", action = "GetFolderByUserMailbox", id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
              name: "GetAllUserMailboxes",
              routeTemplate: "api/Email/GetAllUserMailboxes",
              defaults: new { controller = "Email", action = "GetAllUserMailboxes", id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
            name: "GetFilesByFolder",
            routeTemplate: "api/Email/GetFilesByFolder",
            defaults: new { controller = "Email", action = "GetFilesByFolder", id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
            name: "GetUserFolder",
            routeTemplate: "api/Email/GetUserFolder",
            defaults: new { controller = "Email", action = "GetUserFolder", id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
            name: "MoveFilesIntoFolder",
            routeTemplate: "api/Email/MoveFilesIntoFolder",
            defaults: new { controller = "Email", action = "MoveFilesIntoFolder", id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
           name: "SetFilesToDisable",
           routeTemplate: "api/Email/SetFilesToDisable",
           defaults: new { controller = "Email", action = "SetFilesToDisable", id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
           name: "SetFilesToEnable",
           routeTemplate: "api/Email/SetFilesToEnable",
           defaults: new { controller = "Email", action = "SetFilesToEnable", id = RouteParameter.Optional });
                  
            config.Routes.MapHttpRoute(
                       name: "DefaultApi",
                       routeTemplate: "api/{controller}/{id}",
                       defaults: new { id = RouteParameter.Optional }
                   );
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}
