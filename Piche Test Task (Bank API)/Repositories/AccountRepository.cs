using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Entities;
using Server.Interfaces;

namespace Server.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _db;
        public AccountRepository(AppDbContext db) => _db = db;

        public async Task AddAsync(Account account, CancellationToken ct = default)
        {
            _db.Accounts.Add(account);
            await _db.SaveChangesAsync(ct);
        }

        public Task<Account?> GetByNumberAsync(string accountNumber, CancellationToken ct = default) =>
            _db.Accounts.Include(a => a.Transactions).FirstOrDefaultAsync(a => a.AccountNumber == accountNumber, ct);

        public Task<IEnumerable<Account>> ListAsync(CancellationToken ct = default) =>
            Task.FromResult<IEnumerable<Account>>(_db.Accounts.AsNoTracking().ToList());

        public async Task UpdateAsync(Account account, CancellationToken ct = default)
        {
            _db.Accounts.Update(account);
            await _db.SaveChangesAsync(ct);
        }
    }
}
