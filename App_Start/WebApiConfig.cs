using System.Web.Http;
using System.Web.Http.Cors;

public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        // Enable CORS globally for all controllers and actions
        var cors = new EnableCorsAttribute("*", "*", "*");  // Allows all origins, headers, and methods
        config.EnableCors(cors); // Apply the CORS policy globally

        // Other Web API configuration (e.g., routing, etc.)
        config.MapHttpAttributeRoutes();
        config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
    }
}