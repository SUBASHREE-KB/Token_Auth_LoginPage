using Microsoft.EntityFrameworkCore;
using LoginAPI.Models;

namespace LoginAPI
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<user> users { get; set; }
    }
}
