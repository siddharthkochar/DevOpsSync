using DevOpsSync.WebApp.API;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();

// Configure
var connection = app.Configuration.GetSection("ConnectionStrings:DefaultConnection");

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapGet("/", () => "Welcome to DevOps Sync");

await app.RunAsync();