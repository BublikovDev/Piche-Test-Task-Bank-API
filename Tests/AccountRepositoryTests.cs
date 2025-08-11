using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Entities;
using Server.Repositories;

namespace Tests
{
    public class AccountRepositoryTests
    {
        private AppDbContext CreateDb()
        {
            var opt = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(opt);
        }

        [Fact]
        public async Task Add_And_GetByNumber_Works()
        {
            using var db = CreateDb();
            var repo = new AccountRepository(db);

            var acc = new Account { AccountNumber = "ACC100", Owner = "User1", Balance = 500m };
            await repo.AddAsync(acc);

            var found = await repo.GetByNumberAsync("ACC100");
            Assert.NotNull(found);
            Assert.Equal("User1", found!.Owner);
        }

        [Fact]
        public async Task ListAsync_Returns_All()
        {
            using var db = CreateDb();
            db.Accounts.Add(new Account { AccountNumber = "A", Owner = "O", Balance = 1 });
            db.Accounts.Add(new Account { AccountNumber = "B", Owner = "P", Balance = 2 });
            await db.SaveChangesAsync();

            var repo = new AccountRepository(db);
            var list = await repo.ListAsync();
            Assert.Equal(2, list.Count());
        }

        [Fact]
        public async Task UpdateAsync_Changes_Balance()
        {
            using var db = CreateDb();
            var acc = new Account { AccountNumber = "ACC200", Owner = "User2", Balance = 100m };
            db.Accounts.Add(acc);
            await db.SaveChangesAsync();

            var repo = new AccountRepository(db);
            acc.Balance = 300m;
            await repo.UpdateAsync(acc);

            var updated = await db.Accounts.FindAsync("ACC200");
            Assert.Equal(300m, updated!.Balance);
        }
    }
}
