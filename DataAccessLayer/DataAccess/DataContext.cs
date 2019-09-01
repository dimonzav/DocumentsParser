namespace DataAccess
{
    using DataAccess.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public class DataContext : DbContext
    {
        public DbSet<Log> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            string conn = configuration.GetConnectionString("ParserDatabase");

            optionsBuilder.UseSqlServer(conn);
        }
    }
}
