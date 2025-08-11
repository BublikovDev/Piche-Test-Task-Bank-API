using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;

namespace Tests
{
    public class AccountServiceTests
    {
        private AppDbContext CreateDb()
        {
            var opt = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(opt);
        }

        [Fact]
        public async Task CreateAccount_Should_Create_And_Persist()
        {
            using var db = CreateDb();
            var repo = new Server.Repositories.AccountRepository(db);
            var txRepo = new Server.Repositories.TransactionRepository(db);
            var svc = new Server.Services.AccountService(repo, txRepo);

            var dto = new CreateAccountDto("ACC1", "User1", 100m);
            var res = await svc.CreateAsync(dto);

            Assert.Equal("ACC1", res.AccountNumber);
            var persisted = await db.Accounts.FindAsync("ACC1");
            Assert.NotNull(persisted);
            Assert.Equal(100m, persisted!.Balance);
        }

        [Fact]
        public async Task Deposit_Should_Increase_Balance()
        {
            using var db = CreateDb();
            db.Accounts.Add(new Server.Entities.Account { AccountNumber = "A", Owner = "O", Balance = 50 });
            await db.SaveChangesAsync();

            var repo = new Server.Repositories.AccountRepository(db);
            var txRepo = new Server.Repositories.TransactionRepository(db);
            var svc = new Server.Services.AccountService(repo, txRepo);

            await svc.DepositAsync(new DepositDto("A", 25m));
            var a = await db.Accounts.FindAsync("A");
            Assert.Equal(75m, a!.Balance);
        }

        [Fact]
        public async Task Withdraw_Should_Decrease_Balance()
        {
            using var db = CreateDb();
            db.Accounts.Add(new Server.Entities.Account { AccountNumber = "A", Owner = "O", Balance = 100 });
            await db.SaveChangesAsync();

            var repo = new Server.Repositories.AccountRepository(db);
            var txRepo = new Server.Repositories.TransactionRepository(db);
            var svc = new Server.Services.AccountService(repo, txRepo);

            await svc.WithdrawAsync(new WithdrawDto("A", 40m));
            var a = await db.Accounts.FindAsync("A");
            Assert.Equal(60m, a!.Balance);
        }

        [Fact]
        public async Task Transfer_Should_Move_Funds()
        {
            using var db = CreateDb();
            db.Accounts.AddRange(
                new Server.Entities.Account { AccountNumber = "A", Owner = "O", Balance = 200 },
                new Server.Entities.Account { AccountNumber = "B", Owner = "P", Balance = 50 });
            await db.SaveChangesAsync();

            var repo = new Server.Repositories.AccountRepository(db);
            var txRepo = new Server.Repositories.TransactionRepository(db);
            var svc = new Server.Services.AccountService(repo, txRepo);

            await svc.TransferAsync(new TransferDto("A", "B", 70));
            var a = await db.Accounts.FindAsync("A");
            var b = await db.Accounts.FindAsync("B");
            Assert.Equal(130m, a!.Balance);
            Assert.Equal(120m, b!.Balance);
        }
    }
}
