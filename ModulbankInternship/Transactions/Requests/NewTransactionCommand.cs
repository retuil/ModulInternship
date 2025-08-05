using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.Interfaces;
using ModulbankInternship.Transactions.DTO;
using ModulbankInternship.Transactions.Models;

namespace ModulbankInternship.Transactions.Requests;

public record NewTransactionCommand(TransactionModel Model): ICommand<Guid>;