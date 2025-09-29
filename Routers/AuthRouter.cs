using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using backend.Controllers;

namespace backend.Routers
{
    public static class AuthRouter
    {
        public static void RegisterAuthRoutes(IEndpointRouteBuilder app)
        {
            app.MapControllerRoute(
                name: "auth",
                pattern: "api/auth",
                defaults: new { controller = "Auth" });
        }
    }
}