using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.Rabbit.Models;
using ModulbankInternship.Infrastructure.Rabbit.Outbox;
using ModulbankInternship.Infrastructure.Rabbit.Outbox.publisher;
using Moq;
using RabbitMQ.Client;

namespace ModulbankInternship.Tests.IntegrationTests.Outbox;

public class OutboxPublishesAfterFailureWithMocksTests
{
    [Fact]
    public async Task OutboxPublishesAfterFailure_MockPublisher()
    {
        await using var sqliteConnection = new SqliteConnection("DataSource=:memory:;Cache=Shared");
        sqliteConnection.Open();

        var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(sqliteConnection)
            .Options;

        await using (var ctx = new ApplicationDbContext(dbOptions))
            await ctx.Database.EnsureCreatedAsync();

        Guid outboxId;
        await using (var ctx = new ApplicationDbContext(dbOptions))
        {
            var msg = new OutboxMessageModel
            {
                MessageId = Guid.NewGuid(),
                EventType = "MoneyDebited",
                RoutingKey = "money.debited",
                Payload = "{\"amount\":100}",
                CreatedAt = DateTime.UtcNow,
            };
            ctx.OutboxMessages.Add(msg);
            await ctx.SaveChangesAsync();
            outboxId = msg.MessageId;
        }

        var tcs = new TaskCompletionSource<bool>();
        var rabbitDown = true;

        var mockPublisher = new Mock<IMessagePublisher>();
        mockPublisher
            .Setup(p => p.Publish(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<IBasicProperties>(),
                It.IsAny<byte[]>()))
            .Callback(() =>
            {
                if (rabbitDown)
                {
                    throw new Exception("RabbitMQ unreachable");
                }
                tcs.TrySetResult(true);
            });

        using var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlite(sqliteConnection));
                services.AddSingleton(mockPublisher.Object);
                services.AddLogging();
                services.AddSingleton<IConnectionFactory>(_ =>
                    new ConnectionFactory
                    {
                        HostName = "localhost",
                        UserName = "guest",
                        Password = "guest"
                    });
                services.AddHostedService<OutboxDispatcher>();
            })
            .Build();

        var hostTask = host.StartAsync();

        await Task.Delay(500);
        Assert.False(tcs.Task.IsCompleted);

        rabbitDown = false;

        var completed = await Task.WhenAny(tcs.Task, Task.Delay(5000));
        await Task.Delay(500);
        
        Assert.Equal(tcs.Task.Status, completed.Status); 

        await using (var ctx = new ApplicationDbContext(dbOptions))
        {
            var stored = await ctx.OutboxMessages.FindAsync(outboxId);
            Assert.NotNull(stored);
            Assert.True(stored!.IsDispatched);
            Assert.NotNull(stored.DispatchedAt);
        }

        await host.StopAsync();
    }
}
