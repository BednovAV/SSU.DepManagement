using Models.Request;
using Models.View;

namespace SSU.DM.WebAssembly.Client.Services;

public interface IAuthorizeApi
{
    Task Login(LoginParameters loginParameters);
    Task Register(RegisterParameters registerParameters);
    Task Logout();
    Task<UserInfo> GetUserInfo();
}