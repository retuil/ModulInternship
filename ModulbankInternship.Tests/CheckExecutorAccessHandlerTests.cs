using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Infrastructure.Exceptions;
using ModulbankInternship.Users.Enums;
using ModulbankInternship.Users.Handlers;
using ModulbankInternship.Users.Requests;
using Xunit;

namespace ModulbankInternship.Tests;


public class CheckExecutorAccessHandlerTests
{
    private readonly CheckExecutorAccessHandler _handler;

    public CheckExecutorAccessHandlerTests()
    {
        _handler = new CheckExecutorAccessHandler();
    }

    [Fact]
    public async Task Manager_Access_To_ManagerResource_ShouldPass()
    {
        var executor = new ExecutorData { UserId = Guid.NewGuid(), Role = "Manager" };
        var cmd = new CheckExecutorAccessCommand(Guid.NewGuid(), executor, new[] { EAccessClass.Manager });

        var result = await _handler.Handle(cmd, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task Cashier_Access_To_CashierResource_ShouldPass()
    {
        var executor = new ExecutorData { UserId = Guid.NewGuid(), Role = "Cashier" };
        var cmd = new CheckExecutorAccessCommand(Guid.NewGuid(), executor, new[] { EAccessClass.Cashier });

        var result = await _handler.Handle(cmd, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task Owner_Access_To_OwnResource_ShouldPass()
    {
        var ownerId = Guid.NewGuid();
        var executor = new ExecutorData { UserId = ownerId, Role = "Client" };
        var cmd = new CheckExecutorAccessCommand(ownerId, executor, new[] { EAccessClass.Owner });

        var result = await _handler.Handle(cmd, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task Anyone_Access_ShouldPass()
    {
        var executor = new ExecutorData { UserId = Guid.NewGuid(), Role = "Client" };
        var cmd = new CheckExecutorAccessCommand(Guid.NewGuid(), executor, new[] { EAccessClass.Anyone });

        var result = await _handler.Handle(cmd, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task Cashier_Trying_To_Access_ManagerResource_ShouldFail()
    {
        var executor = new ExecutorData { UserId = Guid.NewGuid(), Role = "Cashier" };
        var cmd = new CheckExecutorAccessCommand(Guid.NewGuid(), executor, new[] { EAccessClass.Manager });

        await Assert.ThrowsAsync<ForbiddenException>(() => _handler.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task NonOwner_Trying_To_Access_OwnerResource_ShouldFail()
    {
        var executor = new ExecutorData { UserId = Guid.NewGuid(), Role = "Client" };
        var cmd = new CheckExecutorAccessCommand(Guid.NewGuid(), executor, new[] { EAccessClass.Owner });

        await Assert.ThrowsAsync<ForbiddenException>(() => _handler.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task NullRole_ShouldThrow_ForbiddenException()
    {
        var executor = new ExecutorData { UserId = Guid.NewGuid(), Role = null };
        var cmd = new CheckExecutorAccessCommand(Guid.NewGuid(), executor, new[] { EAccessClass.Manager });

        await Assert.ThrowsAsync<ForbiddenException>(() => _handler.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task UnknownRole_ShouldThrow_NotImplementedException()
    {
        var executor = new ExecutorData { UserId = Guid.NewGuid(), Role = "Alien" };
        var cmd = new CheckExecutorAccessCommand(Guid.NewGuid(), executor, new[] { EAccessClass.Manager });

        await Assert.ThrowsAsync<NotImplementedException>(() => _handler.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task UnknownAccessClass_ShouldThrow_NotImplementedException()
    {
        var executor = new ExecutorData { UserId = Guid.NewGuid(), Role = "Manager" };
        var invalidAccessClass = (EAccessClass)999;
        var cmd = new CheckExecutorAccessCommand(Guid.NewGuid(), executor, new[] { invalidAccessClass });

        await Assert.ThrowsAsync<NotImplementedException>(() => _handler.Handle(cmd, CancellationToken.None));
    }
}

