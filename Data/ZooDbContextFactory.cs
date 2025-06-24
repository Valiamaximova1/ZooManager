using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;



namespace Data
{
    public class ZooDbContextFactory : IDesignTimeDbContextFactory<ZooDbContext>
    {
        public ZooDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ZooDbContext>();
            var connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=ZooDb;Trusted_Connection=True;";
            optionsBuilder.UseSqlServer(connectionString);

            return new ZooDbContext(optionsBuilder.Options);
        }
    }
}

