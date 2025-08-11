using Server.DTOs;

namespace Server.Interfaces
{
    public interface IAccountService
    {
        Task<AccountDto> CreateAsync(CreateAccountDto dto, CancellationToken ct = default);
        Task<AccountDto?> GetAsync(string accountNumber, CancellationToken ct = default);
        Task<IEnumerable<AccountDto>> ListAsync(CancellationToken ct = default);
        Task DepositAsync(DepositDto dto, CancellationToken ct = default);
        Task WithdrawAsync(WithdrawDto dto, CancellationToken ct = default);
        Task TransferAsync(TransferDto dto, CancellationToken ct = default);
    }
}
