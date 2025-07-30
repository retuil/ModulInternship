using System.Web.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Auth;
using ModulbankInternship.Auth.Exceptions;
using ModulbankInternship.Wallets.Requests;

namespace ModulbankInternship.Account;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("wallets")]
public class WalletsController(IMediator _mediator): ControllerBase
{
    /// <summary>
    /// Получить данные счета по id
    /// </summary>
    /// <param name="id">Id счета</param>
    /// <returns>WalletModel: данные счета</returns>
    /// <response code="200">Успешно. Данные получены</response>
    /// <response code="403">Отсутствуют права на просмотр счета</response>
    /// <response code="404">Счет отсутствует или закрыт</response>
    [Microsoft.AspNetCore.Mvc.HttpGet]
    [Microsoft.AspNetCore.Mvc.Route("{id:guid}")]
    public async Task<WalletModel> GetWalletById([FromUri] Guid id)
    {
        if (!Request.Cookies.TryGetValue(CookieConstants.UserId, out var executorId))
        {
            throw new UnauthorizedException();
        }
        var wallet = await _mediator.Send(new GetWalletByIdQuery(id, Guid.Parse(executorId)));
        return wallet;
    }
    
    /// <summary>
    /// Создать новый счет пользователю
    /// </summary>
    /// <returns>Id созданного счета</returns>
    /// <response code="200">Успешно. Данные получены</response>
    /// <response code="403">У текущего аккаунта нет возможности создавать счет</response>
    /// <response code="404">Пользователь не найден</response>
    [Microsoft.AspNetCore.Mvc.HttpPost]
    [Microsoft.AspNetCore.Mvc.Route("new")]
    public async Task<Guid> CreateWallet([System.Web.Http.FromBody] NewWalletRequest request)
    {
        if (!Request.Cookies.TryGetValue(CookieConstants.UserId, out var executorId))
        {
            throw new UnauthorizedException();
        }
        var newWalletId = await _mediator.Send(new CreateWalletCommand(request, Guid.Parse(executorId)));
        return newWalletId;
    }

    /// <summary>
    /// Изменить параметры счета
    /// </summary>
    /// <param name="id">Id счета</param>
    /// <param name="request">Новые данные для счета</param>
    /// <returns>Список измененных полей с новыми данными в них</returns>
    /// <response code="200">Успешно. Изменения внесены</response>
    /// <response code="403">Отсутствуют права на модификацию счета</response>
    /// <response code="404">Счет отсутствует или закрыт</response>
    [Microsoft.AspNetCore.Mvc.HttpPatch]
    [Microsoft.AspNetCore.Mvc.Route("{id:guid}")]
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

    /// <summary>
    /// Закрыть счет
    /// </summary>
    /// <param name="id">Id счета</param>
    /// <returns>Return 200. Счет успешно закрыт</returns>
    /// <response code="200">Успешно. Счет закрыт</response>
    /// <response code="403">Отсутствуют права на закрытие счета</response>
    /// <response code="404">Счет отсутствует или уже закрыт</response>
    [Microsoft.AspNetCore.Mvc.HttpDelete]
    [Microsoft.AspNetCore.Mvc.Route("{id:guid}")]
    public async Task<IActionResult> CloseWallet([FromUri] Guid id)
    {
        if (!Request.Cookies.TryGetValue(CookieConstants.UserId, out var executorId))
        {
            throw new UnauthorizedException();
        }

        await _mediator.Send(new CloseWalletCommand(id, Guid.Parse(executorId)));
        return Ok($"Закрытие счета id:{id} успешно завершено");
    }

    /// <summary>
    /// Собрать выписку по счету
    /// </summary>
    /// <param name="id">Id счета</param>
    /// <param name="startDate">Дата начала выписки (включительно)</param>
    /// <param name="finishDate">Дата окончания выписки (включительно)</param>
    /// <returns>Выписка по счету за указанные период</returns>
    /// <response code="200">Успешно</response>
    /// <response code="403">Отсутствуют права на заказ выписки по счету</response>
    /// <response code="404">Счет отсутствует или закрыт</response>
    [Microsoft.AspNetCore.Mvc.HttpGet]
    [Microsoft.AspNetCore.Mvc.Route("{id:guid}/statement")]
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

    /// <summary>
    /// Создать перевод с одного счета на другой
    /// </summary>
    /// <param name="request">Данные перевода</param>
    /// <returns>Return 200. Успешный перевод</returns>
    /// <response code="200">Успешно</response>
    /// <response code="403">Отсутствуют права на создание перевода между данными счетами</response>
    /// <response code="404">Один из счетов отсутствует или закрыт</response>
    [Microsoft.AspNetCore.Mvc.HttpPost]
    [Microsoft.AspNetCore.Mvc.Route("make_transfer")]
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