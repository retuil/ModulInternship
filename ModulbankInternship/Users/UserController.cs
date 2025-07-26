using Microsoft.AspNetCore.Mvc;

namespace ModulbankInternship.Users;

[ApiController]
[Route("user")]
public class UserController: ControllerBase
{
    [HttpGet]
    [Route("{id:guid}/")]
}