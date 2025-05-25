namespace ObserverNetLite.API.Endpoints;

public static class ConnectionEndpoint
{
    public static void MapConnectionEndpoint(this WebApplication app)
    {
        app.MapGet("/connection", () =>
        {
            return "ONLINE";
        })
        .WithName("connection")
        .WithOpenApi();
    }
}