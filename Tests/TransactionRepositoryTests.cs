using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Entities;
using Server.Repositories;

namespace Tests
{
    public class TransactionRepositoryTests
    {
        private AppDbContext CreateDb()
        {
            var opt = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(opt);
        }

        [Fact]
        public async Task AddAsync_Saves_Transaction()
        {
            using var db = CreateDb();
            db.Accounts.Add(new Account { AccountNumber = "A", Owner = "O", Balance = 50 });
            await db.SaveChangesAsync();

            var repo = new TransactionRepository(db);
            var tx = new Transaction { AccountNumber = "A", Amount = 25m, Type = "Deposit" };
            await repo.AddAsync(tx);

            var saved = await db.Transactions.FirstOrDefaultAsync();
            Assert.NotNull(saved);
            Assert.Equal(25m, saved!.Amount);
        }

        [Fact]
        public async Task ListByAccountAsync_Returns_Only_For_Account()
        {
            using var db = CreateDb();
            db.Accounts.Add(new Account { AccountNumber = "A", Owner = "O", Balance = 50 });
            db.Transactions.Add(new Transaction { AccountNumber = "A", Amount = 10m, Type = "Deposit" });
            db.Transactions.Add(new Transaction { AccountNumber = "B", Amount = 5m, Type = "Deposit" });
            await db.SaveChangesAsync();

            var repo = new TransactionRepository(db);
            var list = await repo.ListByAccountAsync("A");

            Assert.Single(list);
            Assert.Equal("A", list.First().AccountNumber);
        }
    }
}
