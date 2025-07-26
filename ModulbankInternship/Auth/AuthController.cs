using System.Net;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;

namespace ModulbankInternship.Auth;

[ApiController]
[Route("auth")]
public class AuthController: ControllerBase
{
    [HttpPost]
    [Route("")]
    public IActionResult Login(AuthContract contract)
    {
        // TODO: DI
        var userId = AuthService.LoginUser(contract);
        Response.Cookies.Append(CookieConstants.UserId, userId.ToString(), new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.Strict
        });
        return Ok($"Выполнен вход в аккаунт ID: {userId}");
    }
    
    [HttpDelete]
    [Route("")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(CookieConstants.UserId);
        return Ok($"Выполнен выход из аккаунта");
    }

    [HttpPost]
    [Route("register")]
    public IActionResult Register(RegisterContract contract)
    {
        // TODO: DI
        var userId = AuthService.RegisterUser(contract);
        Response.Cookies.Append(CookieConstants.UserId, userId.ToString(), new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.Strict
        });
        return Ok($"Пользователь с номером: {contract.PhoneNumber} зарегистрирован под ID: {userId}. Выполнен вход в аккаунт ID: {userId}");
    }
}