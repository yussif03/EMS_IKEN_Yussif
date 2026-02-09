namespace EMS.API.Endpoints
{
    public static class EmployeeEndpoints
    {
        public static RouteGroupBuilder MapEmployeeEndpoints(this RouteGroupBuilder group)
        {
            // Default endpoint
            group.MapGet("/", () => "EMS API is running...");

            // Health Check Endpoint
            group.MapGet("/health", () => Results.Ok("Ok"));

            return group;
        }
    }
}
