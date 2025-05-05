using AppShare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

namespace AppServer.ApiControllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesApiController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<FilesApiController> _logger;

        public FilesApiController(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, ILogger<FilesApiController> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [AllowAnonymous]
        [SwaggerOperation(Summary = "Lấy tất cả files trong thư mục wwwroot", Description = "API này dùng để lấy tất cả files trong thư mục wwwroot")]
        [HttpGet("")]
        public IActionResult GetFiles([FromQuery] string folderPath = "", [FromQuery] string searchPattern = "*")
        {
            var path = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);

            if (!Directory.Exists(path))
            {
                return NotFound("Thư mục không tồn tại.");
            }

            // Lấy base URL từ HttpContext
            var request = _httpContextAccessor.HttpContext!.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            // Lấy danh sách file và chuyển đổi thành URL đầy đủ
            var fileUrls = Directory.GetFiles(path, searchPattern)
                                    .Select(file => string.IsNullOrEmpty(folderPath) ?
                                        Path.GetFileName(file) :
                                        $"{folderPath}/{Path.GetFileName(file)}")
                                    .ToList();

            return Ok(fileUrls);
        }

        [AllowAnonymous]
        [SwaggerOperation(Summary = "Ghi log", Description = "API này dùng để ghi log từ Client<br/> Các level: Verbose, Debug, Information, Warning, Error, Fatal")]
        [HttpPut("write-log")]
        public IActionResult WriteLog([FromBody] WriteLogModel model)
        {
            switch (model.Level)
            {
                // Chi tiết nhất, thường bị tắt trong môi trường sản xuất.
                // Đây là mức độ thấp nhất.
                // Dùng để ghi lại mọi chi tiết có thể về một ứng dụng, kể cả những thông tin ít quan trọng nhất.
                // Thường được dùng cho mục đích gỡ lỗi(debugging) chuyên sâu hoặc khi bạn cần kiểm tra toàn bộ hoạt động của hệ thống.
                // Ví dụ: "Starting method X with parameters: {param1}, {param2}."
                case "Verbose":
                    Log.ForContext("Client", true).Verbose(model.Message);
                    break;
                // Dành cho gỡ lỗi trong môi trường phát triển.
                // Dùng để ghi log trong quá trình phát triển và kiểm tra ứng dụng.
                // Nó không chi tiết như Verbose, nhưng vẫn chứa đủ thông tin cho các nhà phát triển.
                // Ví dụ: "Successfully connected to the database."
                case "Debug":
                    Log.ForContext("Client", true).Debug(model.Message);
                    break;
                // Biểu thị hoạt động bình thường của ứng dụng.
                // Mức log này thường được sử dụng để ghi lại những sự kiện bình thường và quan trọng trong ứng dụng.
                // Dùng để ghi nhận các sự kiện giúp bạn theo dõi ứng dụng đang hoạt động như thế nào.
                // Ví dụ: "User JohnDoe has logged in."
                case "Information":
                    Log.ForContext("Client", true).Information(model.Message);
                    break;
                // Cảnh báo về các vấn đề tiềm tàng.
                // Cấp độ này biểu thị cảnh báo hoặc các tình huống không mong muốn nhưng chưa gây ra lỗi.
                // Bạn có thể sử dụng nó để ghi nhận các vấn đề mà nếu không xử lý, có thể dẫn đến lỗi trong tương lai.
                // Ví dụ: "API response time is slower than expected."
                case "Warning":
                    Log.ForContext("Client", true).Warning(model.Message);
                    break;
                // Lỗi đã xảy ra nhưng ứng dụng vẫn hoạt động.
                // Dùng để ghi lại các lỗi trong ứng dụng, những lỗi đã xảy ra nhưng không dẫn đến việc dừng hoàn toàn của hệ thống.
                // Lỗi ở cấp độ này cần được kiểm tra và sửa chữa sớm.
                // Ví dụ: "Failed to save data to the database. Exception: {ex}."
                case "Error":
                    Log.ForContext("Client", true).Error(model.Message);
                    break;
                // Sự cố nghiêm trọng khiến hệ thống không thể tiếp tục.
                // Đây là mức độ cao nhất, thường biểu thị các sự cố nghiêm trọng khiến ứng dụng không thể tiếp tục hoạt động.
                // Dùng khi hệ thống hoặc một phần quan trọng của nó ngừng hoạt động.
                // Ví dụ: "Critical failure: Out of memory. Application is shutting down."
                case "Fatal":
                    Log.ForContext("Client", true).Fatal(model.Message);
                    break;

                default:
                    Log.ForContext("Client", true).Information("UNKNOWN LEVEL: {Level}, {Message}", model.Level, model.Message);
                    break;
            }

            return Ok();
        }
    }
}
