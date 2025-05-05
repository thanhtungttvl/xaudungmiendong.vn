using AppServer.Business.Authentication;
using AppServer.Business.Core;
using AppServer.Business.Entities;
using AppShare.Models.Auth;
using AppShare.Models.User;
using AutoMapper;

namespace AppServer.Business.Service
{
    public interface IAuthService
    {
        Task<UserModel> Login(AuthLoginRequestModel model);
        UserModel LoginGuest(string name);
        Task<UserModel> Login(string token);
        Task<Guid> Register(AuthRegisterRequestModel model);
        Task RecoverPassword(string email);
        Task ResetPassword(AuthResetPasswordRequestModel model);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly JwtManager _jwtManager;
        private readonly IMapper _mapper;

        public AuthService(IUserService userService, JwtManager jwtManager, IMapper mapper)
        {
            _userService = userService;
            _jwtManager = jwtManager;
            _mapper = mapper;
        }

        public async Task<UserModel> Login(AuthLoginRequestModel model)
        {
            var user = await _userService.FindAsync(x => x.Email.Equals(model.Email) && x.Password.Equals(model.Password));

            if (user is null)
            {
                throw new AppException("Email hoặc Password không đúng.");
            }

            if (user.Status == UserEnum.StatusOption.Locked)
            {
                throw new AppException("Tài khoản của bạn đã bị khóa.");
            }

            return _jwtManager.GenerateJwtToken(_mapper.Map<UserModel>(user));
        }

        public async Task<UserModel> Login(string token)
        {
            var jwtToken = _jwtManager.ValidateJwtToken(token);
            if (jwtToken is null)
            {
                throw new AppException("Token không đúng hoặc đã hết hạn.");
            }

            var id = jwtToken.Claims.ToList().Where(x => x.Type is "id").Select(x => x.Value).FirstOrDefault();

            var user = await _userService.FindAsync(x => x.Id.Equals(new Guid(id!)));

            if (user is null)
            {
                throw new AppException("Không tìm thấy data.");
            }

            if (user.Status == UserEnum.StatusOption.Locked)
            {
                throw new AppException("Tài khoản của bạn đã bị khóa.");
            }

            return _mapper.Map<UserModel>(user);
        }

        public UserModel LoginGuest(string name)
        {
            return _jwtManager.GenerateJwtToken(new UserModel
            {
                Name = name,
                Role = UserEnum.RoleOption.Guest,
            });
        }

        public Task RecoverPassword(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> Register(AuthRegisterRequestModel model)
        {
            return await _userService.CreateAsync(new UserEntity
            {
                Email = model.Email,
                Name = model.Email,
                Avatar = "_content/MudThemeLibrary/images/default-girl.png",
                Password = model.Password,
                Role = UserEnum.RoleOption.User,
                Status = UserEnum.StatusOption.Active,
            });
        }

        public Task ResetPassword(AuthResetPasswordRequestModel model)
        {
            throw new NotImplementedException();
        }
    }
}
