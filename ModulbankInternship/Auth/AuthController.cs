using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Auth.Exceptions;
using ModulbankInternship.Auth.Requests;
using Swashbuckle.Swagger.Annotations;

namespace ModulbankInternship.Auth;

[ApiController]
[Route("auth")]
public class AuthController(IMediator _mediator)
    : ControllerBase
{
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Login(AuthContract contract)
    {
        var user = await _mediator.Send(new LoginUserCommand(contract));
        WriteToCookie(user.Id);
        return Ok($"Выполнен вход в аккаунт ID: {user.Id}");
    }
    
    [HttpDelete]
    [Route("")]
    public IActionResult Logout()
    {
        if (!Request.Cookies.TryGetValue(CookieConstants.UserId, out var userId))
        {
            throw new UnauthorizedException();
        }
        Response.Cookies.Delete(CookieConstants.UserId);
        return Ok($"Выполнен выход из аккаунта id: {userId}");
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterContract contract)
    {
        // TODO: DI
        var user = await _mediator.Send(new RegisterUserCommand(contract));
        WriteToCookie(user.Id);
        return Ok($"Пользователь с номером: {contract.PhoneNumber} зарегистрирован под ID: {user.Id}. Выполнен вход в аккаунт ID: {user.Id}");
    }

    private void WriteToCookie(Guid userId)
    {
        Response.Cookies.Append(CookieConstants.UserId, userId.ToString(), new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.Strict
        });
    }
}