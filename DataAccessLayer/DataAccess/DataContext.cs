namespace DataAccess
{
    using DataAccess.Entities;
    using Microsoft.EntityFrameworkCore;

    public class DataContext : DbContext
    {
        public DbSet<Log> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\ProjectsV13;Initial Catalog=parser_db;Integrated Security=True;Connection Timeout=90;MultipleActiveResultSets=true;");
        }
    }
}
