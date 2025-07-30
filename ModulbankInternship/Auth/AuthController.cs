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
    /// <summary>
    /// Вход в сервис
    /// </summary>
    /// <param name="contract">Данные для входа по номеру телефона и паролю</param>
    /// <returns>Returns 200 Успешный вход.</returns>
    /// <response code="200">Успешно. Вход в аккаунт</response>
    /// <response code="404">Пользователь не найден</response>
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Login(AuthContract contract)
    {
        var user = await _mediator.Send(new LoginUserCommand(contract));
        WriteToCookie(user.Id);
        return Ok($"Выполнен вход в аккаунт ID: {user.Id}");
    }
    
    /// <summary>
    /// Выйти из аккаунта
    /// </summary>
    /// <returns>Returns 200 Успешный выход.</returns>
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

    /// <summary>
    /// Регистрация в сервисе
    /// </summary>
    /// <param name="contract">Данные для регистрации</param>
    /// <returns>Returns 200 Успешная регистрация.</returns>
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