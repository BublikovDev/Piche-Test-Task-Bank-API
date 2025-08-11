using Server.Data;
using Server.Entities;
using Server.Interfaces;

namespace Server.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _db;
        public TransactionRepository(AppDbContext db) => _db = db;

        public async Task AddAsync(Transaction transaction, CancellationToken ct = default)
        {
            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync(ct);
        }

        public Task<IEnumerable<Transaction>> ListByAccountAsync(string accountNumber, CancellationToken ct = default) =>
            Task.FromResult<IEnumerable<Transaction>>(_db.Transactions.Where(t => t.AccountNumber == accountNumber).ToList());
    }
}
