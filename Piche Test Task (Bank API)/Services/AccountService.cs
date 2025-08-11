using Server.DTOs;
using Server.Entities;
using Server.Interfaces;

namespace Server.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accounts;
        private readonly ITransactionRepository _transactions;

        public AccountService(IAccountRepository accounts, ITransactionRepository transactions)
        {
            _accounts = accounts;
            _transactions = transactions;
        }

        public async Task<AccountDto> CreateAsync(CreateAccountDto dto, CancellationToken ct = default)
        {
            var existing = await _accounts.GetByNumberAsync(dto.AccountNumber, ct);
            if (existing != null) throw new InvalidOperationException("Account already exists");

            var account = new Account
            {
                AccountNumber = dto.AccountNumber,
                Owner = dto.Owner,
                Balance = dto.InitialBalance
            };

            await _accounts.AddAsync(account, ct);

            var tx = new Transaction
            {
                AccountNumber = account.AccountNumber,
                Amount = dto.InitialBalance,
                Type = "InitialDeposit",
                Notes = "Initial balance"
            };

            await _transactions.AddAsync(tx, ct);

            return new AccountDto(account.AccountNumber, account.Owner, account.Balance);
        }

        public async Task<AccountDto?> GetAsync(string accountNumber, CancellationToken ct = default)
        {
            var a = await _accounts.GetByNumberAsync(accountNumber, ct);
            return a == null ? null : new AccountDto(a.AccountNumber, a.Owner, a.Balance);
        }

        public async Task<IEnumerable<AccountDto>> ListAsync(CancellationToken ct = default)
        {
            var list = await _accounts.ListAsync(ct);
            return list.Select(a => new AccountDto(a.AccountNumber, a.Owner, a.Balance));
        }

        public async Task DepositAsync(DepositDto dto, CancellationToken ct = default)
        {
            var account = await _accounts.GetByNumberAsync(dto.AccountNumber, ct)
                ?? throw new KeyNotFoundException("Account not found");

            if (dto.Amount <= 0) throw new ArgumentException("Amount must be positive");

            account.Balance += dto.Amount;
            await _accounts.UpdateAsync(account, ct);

            var tx = new Transaction
            {
                AccountNumber = account.AccountNumber,
                Amount = dto.Amount,
                Type = "Deposit"
            };

            await _transactions.AddAsync(tx, ct);
        }

        public async Task WithdrawAsync(WithdrawDto dto, CancellationToken ct = default)
        {
            var account = await _accounts.GetByNumberAsync(dto.AccountNumber, ct)
                ?? throw new KeyNotFoundException("Account not found");

            if (dto.Amount <= 0) throw new ArgumentException("Amount must be positive");
            if (account.Balance < dto.Amount) throw new InvalidOperationException("Insufficient funds");

            account.Balance -= dto.Amount;
            await _accounts.UpdateAsync(account, ct);

            var tx = new Transaction
            {
                AccountNumber = account.AccountNumber,
                Amount = -dto.Amount,
                Type = "Withdraw"
            };

            await _transactions.AddAsync(tx, ct);
        }

        public async Task TransferAsync(TransferDto dto, CancellationToken ct = default)
        {
            if (dto.Amount <= 0) throw new ArgumentException("Amount must be positive");
            if (dto.FromAccount == dto.ToAccount) throw new ArgumentException("Accounts must be different");

            var from = await _accounts.GetByNumberAsync(dto.FromAccount, ct)
                ?? throw new KeyNotFoundException("Source account not found");
            var to = await _accounts.GetByNumberAsync(dto.ToAccount, ct)
                ?? throw new KeyNotFoundException("Destination account not found");

            if (from.Balance < dto.Amount) throw new InvalidOperationException("Insufficient funds");

            from.Balance -= dto.Amount;
            to.Balance += dto.Amount;

            await _accounts.UpdateAsync(from, ct);
            await _accounts.UpdateAsync(to, ct);

            var txOut = new Transaction
            {
                AccountNumber = from.AccountNumber,
                Amount = -dto.Amount,
                Type = "TransferOut",
                Notes = $"To {to.AccountNumber}"
            };

            var txIn = new Transaction
            {
                AccountNumber = to.AccountNumber,
                Amount = dto.Amount,
                Type = "TransferIn",
                Notes = $"From {from.AccountNumber}"
            };

            await _transactions.AddAsync(txOut, ct);
            await _transactions.AddAsync(txIn, ct);
        }
    }
}
