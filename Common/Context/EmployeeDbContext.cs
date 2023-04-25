using Microsoft.EntityFrameworkCore;


namespace Common
{
    public class EmployeeDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().Property(e => e.Id).UseIdentityColumn<int>().HasColumnName("EmployeeNumber");
            modelBuilder.Entity<Employee>().Property(e => e.Name).HasColumnName("EmployeeName").IsRequired();
            modelBuilder.Entity<Employee>().Property(p => p.HourlyRate).HasColumnName("HourlyRate").IsRequired();
            modelBuilder.Entity<Employee>().Property(p => p.HoursWorked).HasColumnName("HoursWorked").IsRequired();
            modelBuilder.Entity<Employee>().Property(p => p.Salary).HasColumnName("TotalPay").HasComputedColumnSql<float>($"{nameof(Employee.HourlyRate)}*{nameof(Employee.HoursWorked)}", true);

            base.OnModelCreating(modelBuilder);
        }

    }
}
