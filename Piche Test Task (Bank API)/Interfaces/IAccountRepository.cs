using Server.Entities;

namespace Server.Interfaces
{
    public interface IAccountRepository
    {
        Task AddAsync(Account account, CancellationToken ct = default);
        Task<Account?> GetByNumberAsync(string accountNumber, CancellationToken ct = default);
        Task<IEnumerable<Account>> ListAsync(CancellationToken ct = default);
        Task UpdateAsync(Account account, CancellationToken ct = default);
    }
}
