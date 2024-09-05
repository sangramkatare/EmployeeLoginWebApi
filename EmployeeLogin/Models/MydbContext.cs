using Microsoft.EntityFrameworkCore;

namespace EmployeeLogin.Models
{
    public class MydbContext : DbContext
    {
        public MydbContext(DbContextOptions<MydbContext> options): base(options)
        {

        }
        public DbSet<Employee> Employeees{ get; set; }
    }
}
