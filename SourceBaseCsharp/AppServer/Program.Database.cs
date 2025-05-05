using AppServer.Business.Database;
using Microsoft.EntityFrameworkCore;

namespace AppServer
{
    public partial class Program
    {
        private static void AddDatabase(WebApplicationBuilder builder)
        {
            // Lấy connection strings từ cấu hình
            var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");

            // Đăng ký DbContext cho DefaultConnection
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(defaultConnection));
        }

        private static void UseDatabase(WebApplication app)
        {
            // Tự động tạo database nếu chưa tồn tại
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                // Sử dụng EnsureCreated để tạo database mà không cần migration
                dbContext.Database.EnsureCreated();
                // Hoặc sử dụng Migrate nếu muốn áp dụng các migration
                // dbContext.Database.Migrate();
            }
        }
    }
}
