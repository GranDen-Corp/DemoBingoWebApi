using GranDen.Game.ApiLib.Bingo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BingoDb.Migrations
{
    public class BingoDbContextDesignTimeFactory : IDesignTimeDbContextFactory<BingoGameDbContext>
    {
        public BingoGameDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BingoGameDbContext>();
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DemoBingoDb",
                x => x.MigrationsAssembly(typeof(BingoDbContextDesignTimeFactory).Assembly.GetName().Name));

            return new BingoGameDbContext(optionsBuilder.Options);
        }
    }
}