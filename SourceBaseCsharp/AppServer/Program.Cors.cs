namespace AppServer
{
    public partial class Program
    {
        private static void AddCors(WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "ClientHostsCORS",
                policy =>
                {
                    policy.WithOrigins(builder.Configuration["ClientHosts:AppAdmin"]!)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
        }

        private static void UseCors(WebApplication app, WebApplicationBuilder builder)
        {
            app.UseCors("ClientHostsCORS");
            app.UseHsts();
            app.UseHttpsRedirection();
            // Middleware để phục vụ Static Files từ wwwroot
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    var request = context.Context.Request;
                    var response = context.Context.Response;

                    // Chỉ cho phép CORS nếu request đến từ danh sách WithOrigins
                    var allowedOrigins = new[] { builder.Configuration["ClientHosts:AppAdmin"]! };
                    var origin = request.Headers["Origin"].ToString();

                    if (allowedOrigins.Contains(origin))
                    {
                        response.Headers.Append("Access-Control-Allow-Origin", origin);
                        response.Headers.Append("Access-Control-Allow-Methods", "GET");
                        response.Headers.Append("Access-Control-Allow-Headers", "Content-Type");
                    }
                }
            });
        }
    }
}
