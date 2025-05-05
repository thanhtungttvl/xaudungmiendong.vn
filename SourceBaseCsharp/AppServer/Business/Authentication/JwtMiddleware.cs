using AppServer.Business.Entities;
using AppServer.Business.Service;

namespace AppServer.Business.Authentication
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserService userService, JwtManager jwtManager)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var jwtToken = jwtManager.ValidateJwtToken(token);

            if (jwtToken is not null)
            {
                var id = jwtToken.Claims.ToList().Where(x => x.Type is "Id").Select(x => x.Value).FirstOrDefault();

                if (string.IsNullOrEmpty(id) is false)
                {
                    var user = await userService.FindAsync(x => x.Id.Equals(new Guid(id)))!;

                    if(user is null)
                    {
                        context.Items["User"] = null;
                    }
                    else
                    {
                        if (user.Status == UserEnum.StatusOption.Locked)
                        {
                            context.Items["User"] = null;
                        }
                        else
                        {
                            context.Items["User"] = user;
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}
