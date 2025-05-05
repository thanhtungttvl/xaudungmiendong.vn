using AppServer.Business.Core;
using AppServer.Business.Middleware;

namespace AppServer
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            AddSerilog(builder);

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllers();

            AddDatabase(builder);
            AddAuthentication(builder);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            AddMvc(builder);
            AddSwagger(builder);
            AddHub(builder);
            AddCors(builder);
            AddService(builder);

            var app = builder.Build();

            UseSerilog(app);
            UseDatabase(app);
            UseSwagger(app);
            UseAuthentication(app);
            UseCors(app, builder);
            UseHub(app);

            app.UseMiddleware<ErrorHandlerMiddleware>();

            UseMvc(app);

            app.Run();
        }
    }
}
