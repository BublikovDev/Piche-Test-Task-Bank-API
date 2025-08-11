using Server.Entities;

namespace Server.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction, CancellationToken ct = default);
        Task<IEnumerable<Transaction>> ListByAccountAsync(string accountNumber, CancellationToken ct = default);
    }
}
