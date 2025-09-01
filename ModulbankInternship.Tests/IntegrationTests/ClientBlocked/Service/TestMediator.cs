using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ModulbankInternship.Accounts.Domain.Exceptions;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.Rabbit.Outbox;
using ModulbankInternship.Transactions.Models;

namespace Tests.Helpers
{
    public class TestMediator : IMediator
    {
        private readonly ApplicationDbContext _db;

        public TestMediator(ApplicationDbContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var reqTypeName = request.GetType().Name;

            // ---- CheckExecutorAccessCommand ----
            if (reqTypeName == "CheckExecutorAccessCommand")
            {
                // Если тест ожидает Unit
                if (typeof(TResponse) == typeof(Unit))
                    return (TResponse)(object)Unit.Value;

                // Если тест/реальный handler ожидает boolean (true = доступ разрешён)
                if (typeof(TResponse) == typeof(bool))
                    return (TResponse)(object)true;

                // Если ожидают что-то ещё — выбросим понятное исключение, чтобы не маскировать ошибки
                throw new InvalidOperationException($"TestMediator: cannot return response of type {typeof(TResponse).FullName} for {reqTypeName}");
            }

            // ---- AddTransactionToRepositoryCommand ----
            if (reqTypeName == "AddTransactionToRepositoryCommand")
            {
                // Попытаемся получить свойство Transaction
                var txProp = request.GetType().GetProperty("TransactionModel", BindingFlags.Public | BindingFlags.Instance);
                if (txProp == null)
                    throw new InvalidOperationException("TestMediator: AddTransactionToRepositoryCommand must have Transaction property");

                var txObj = txProp.GetValue(request);
                if (txObj == null)
                    throw new InvalidOperationException("TestMediator: Transaction is null");

                // Приводим к вашему TransactionModel (подставьте нужное имя/пространство, если у вас другой)
                var tx = (TransactionModel)txObj;

                // Поведение репозитория: проверяем account exist / IsFrozen
                var account = await _db.Accounts.FirstOrDefaultAsync(a => a.Id == tx.AccountId, cancellationToken);
                if (account == null || !account.IsExist)
                    throw new AccountNotFoundException(tx.AccountId);

                if (account.IsFrozen)
                    throw new AccountBlockedException(tx.AccountId);

                // Успешный путь: добавляем транзакцию и outbox (если нужно)
                tx.Id = Guid.NewGuid();
                await _db.Transactions.AddAsync(tx, cancellationToken);

                var outbox = OutboxMessageCreator.NewTransaction(tx);
                outbox.MessageId = Guid.NewGuid();
                await _db.OutboxMessages.AddAsync(outbox, cancellationToken);

                await _db.SaveChangesAsync(cancellationToken);

                // Ожидается Guid
                if (typeof(TResponse) == typeof(Guid))
                    return (TResponse)(object)tx.Id;

                // Если ожидают Unit вместо Guid — тоже поддержим
                if (typeof(TResponse) == typeof(Unit))
                    return (TResponse)(object)Unit.Value;

                throw new InvalidOperationException($"TestMediator: AddTransactionToRepositoryCommand cannot produce response of type {typeof(TResponse).FullName}");
            }

            throw new InvalidOperationException($"TestMediator cannot handle request of type {request.GetType().FullName}");
        }

        public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = new CancellationToken()) where TRequest : IRequest
        {
            return Task.CompletedTask;
        }

        // Non-generic Send(object)
        public async Task<object?> Send(object request, CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var reqIface = request.GetType()
                                 .GetInterfaces()
                                 .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>));

            if (reqIface == null)
                throw new InvalidOperationException("TestMediator: request does not implement IRequest<T>");

            var responseType = reqIface.GetGenericArguments()[0];

            var sendMethod = typeof(TestMediator).GetMethod(nameof(Send), BindingFlags.Public | BindingFlags.Instance);
            var genericSend = sendMethod!.MakeGenericMethod(responseType);

            var taskObj = genericSend.Invoke(this, new object[] { request, cancellationToken });
            if (taskObj is Task t)
            {
                await t.ConfigureAwait(false);
                var resultProp = t.GetType().GetProperty("Result");
                return resultProp?.GetValue(t);
            }

            return null;
        }

        // Publish noop
        public Task Publish(object notification, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification => Task.CompletedTask;

        // Stream (not used in tests) -> empty
        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
            => EmptyAsync<TResponse>();

        public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default)
            => EmptyAsync<object?>();

        private static async IAsyncEnumerable<T> EmptyAsync<T>()
        {
            yield break;
        }
    }
}
