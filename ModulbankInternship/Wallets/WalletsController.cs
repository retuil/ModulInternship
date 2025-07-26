using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Auth;
using ModulbankInternship.Auth.Exceptions;

namespace ModulbankInternship.Account;

[ApiController]
[Route("wallets")]
public class WalletsController: ControllerBase
{
    [Microsoft.AspNetCore.Mvc.HttpGet]
    [Route("{id:guid}")]
    public WalletModel GetWalletById([FromUri] Guid id)
    {
        // TODO: DI
        if (!Request.Cookies.TryGetValue(CookieConstants.UserId, out var executorId))
        {
            throw new UnauthorizedException();
        }
        // TODO: DI
        return WalletRequestHendler.GetWalletById(id, executorId);
    }
    
    [Microsoft.AspNetCore.Mvc.HttpPost]
    [Route("new")]
    public Guid CreateWallet([System.Web.Http.FromBody] NewWalletRequest request)
    {
        // TODO: DI
        if (!Request.Cookies.TryGetValue(CookieConstants.UserId, out var executorId))
        {
            throw new UnauthorizedException();
        }
        var newWalletId = WalletRequestHendler.CreateWallet(ownerId, executorId);
        return newWalletId;
    }

    [System.Web.Http.HttpPatch]
    [Route("{id:guid}")]
    public IActionResult ModifyWalletParameters([FromUri] Guid id, [System.Web.Http.FromBody] ModifyWalletRequest request)
    {
        if (!Request.Cookies.TryGetValue(CookieConstants.UserId, out var executorId))
        {
            throw new UnauthorizedException();
        }
        // TODO: DI
        // Должен возвращать Dictionary<string, string> с новыми значениями модифицированных полей
        Dictionary<string, string> modifiedParameters = WalletRequestHendler.ModifyWalletParameters(id, request, executorId);
        var changesRecord = string.Join(", ", modifiedParameters.Select(x => $"{x.Key}: {x.Value}"));
        return Ok($"У счета id: {id} были изменены следующие параметры {changesRecord}");
    }

    [Microsoft.AspNetCore.Mvc.HttpDelete]
    [Route("{id:guid}")]
    public IActionResult CloseWallet([FromUri] Guid id)
    {
        if (!Request.Cookies.TryGetValue(CookieConstants.UserId, out var executorId))
        {
            throw new UnauthorizedException();
        }
        // TODO: DI
        WalletRequestHendler.CloseWallet(id, executorId);
        return Ok($"Закрытие счета id:{id} успешно завершено");
    }

    [System.Web.Http.HttpGet]
    [Route("{id:guid}/statement")]
    public WalletStatementResponse GetWalletStatement([FromUri] Guid id, [FromUri] DateTime startDate,
        [FromUri] DateTime finishDate)
    {
        if (!Request.Cookies.TryGetValue(CookieConstants.UserId, out var executorId))
        {
            throw new UnauthorizedException();
        }
        // TODO: DI
        var response = WalletRequestHendler.GetAccountStatement(id, startDate, finishDate, executorId);
        return response;
    }

    [System.Web.Http.HttpPost]
    [Route("make_transfer")]
    public IActionResult MakeTransfer([System.Web.Http.FromBody] TransferRequest request)
    {
        if (!Request.Cookies.TryGetValue(CookieConstants.UserId, out var executorId))
        {
            throw new UnauthorizedException();
        }
        // TODO: DI
        WalletRequestHendler.MakeTransfer(request, executorId);
        return Ok(
            $"Перевод со счета id: {request.WalletId} на счет id: {request.CounterpartyWalletId} на сумму {request.Amount} проведет успешно");
    }
}