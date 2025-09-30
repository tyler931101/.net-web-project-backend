using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using backend.Controllers;

namespace backend.Routers
{
    public static class TicketRouter
    {
        public static void RegisterTicketRoutes(IEndpointRouteBuilder app)
        {
            app.MapControllerRoute(
                name: "ticket",
                pattern: "api/ticket/{action=Index}/{id?}",
                defaults: new { controller = "Ticket" });
        }
    }
}