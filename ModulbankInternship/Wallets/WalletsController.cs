using System.Web.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Auth;
using ModulbankInternship.Auth.Exceptions;
using ModulbankInternship.Wallets.Requests;

namespace ModulbankInternship.Account;

[ApiController]
[Route("wallets")]
public class WalletsController(IMediator _mediator): ControllerBase
{
    [Microsoft.AspNetCore.Mvc.HttpGet]
    [Route("{id:guid}")]
    public async Task<WalletModel> GetWalletById([FromUri] Guid id)
    {
        if (!Request.Cookies.TryGetValue(CookieConstants.UserId, out var executorId))
        {
            throw new UnauthorizedException();
        }
        var wallet = await _mediator.Send(new GetWalletByIdQuery(id, Guid.Parse(executorId)));
        return wallet;
    }
    
    [Microsoft.AspNetCore.Mvc.HttpPost]
    [Route("new")]
    public async Task<Guid> CreateWallet([System.Web.Http.FromBody] NewWalletRequest request)
    {
        if (!Request.Cookies.TryGetValue(CookieConstants.UserId, out var executorId))
        {
            throw new UnauthorizedException();
        }
        var newWalletId = await _mediator.Send(new CreateWalletCommand(request, Guid.Parse(executorId)));
        return newWalletId;
    }

    [System.Web.Http.HttpPatch]
    [Route("{id:guid}")]
    public async Task<IActionResult> ModifyWalletParameters([FromUri] Guid id, [System.Web.Http.FromBody] ModifyWalletRequest request)
    {
        if (!Request.Cookies.TryGetValue(CookieConstants.UserId, out var executorId))
        {
            throw new UnauthorizedException();
        }

        var modifiedParameters =
            await _mediator.Send(new ModifyWalletParametersCommand(id, request, Guid.Parse(executorId)));
        var changesRecord = string.Join(", ", modifiedParameters.Select(x => $"{x.Key}: {x.Value}"));
        return Ok($"У счета id: {id} были изменены следующие параметры {changesRecord}");
    }

    [Microsoft.AspNetCore.Mvc.HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> CloseWallet([FromUri] Guid id)
    {
        if (!Request.Cookies.TryGetValue(CookieConstants.UserId, out var executorId))
        {
            throw new UnauthorizedException();
        }

        await _mediator.Send(new CloseWalletCommand(id, Guid.Parse(executorId)));
        return Ok($"Закрытие счета id:{id} успешно завершено");
    }

    [System.Web.Http.HttpGet]
    [Route("{id:guid}/statement")]
    public async Task<WalletStatementResponse> GetWalletStatement([FromUri] Guid id, [FromUri] DateTime startDate,
        [FromUri] DateTime finishDate)
    {
        if (!Request.Cookies.TryGetValue(CookieConstants.UserId, out var executorId))
        {
            throw new UnauthorizedException();
        }
        var response = await _mediator.Send(new GetWalletStatementQuery(id, startDate, finishDate, Guid.Parse(executorId)));
        return response;
    }

    [System.Web.Http.HttpPost]
    [Route("make_transfer")]
    public async Task<IActionResult> MakeTransfer([System.Web.Http.FromBody] TransferRequest request)
    {
        if (!Request.Cookies.TryGetValue(CookieConstants.UserId, out var executorId))
        {
            throw new UnauthorizedException();
        }

        await _mediator.Send(new MakeTransferCommand(request, Guid.Parse(executorId)));
        return Ok(
            $"Перевод со счета id: {request.WalletId} на счет id: {request.CounterpartyWalletId} на сумму {request.Amount} проведет успешно");
    }
}