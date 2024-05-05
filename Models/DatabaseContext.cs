using Microsoft.EntityFrameworkCore;

namespace CrudAppliction.Models
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) 
        {
            
        }
        public DbSet<CustomerTB> CustomerTBs { get; set; }
        public DbSet<Registration> Register { get; set; }
    }
}
