using Microsoft.EntityFrameworkCore;

namespace EmployeesApp.Models
{
    public class HRDataBaseContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"data source=DESKTOP-BGFGUNG; initial catalog=EmployeesDB ; integrated security=SSPI;") ;
        }

    }
}
