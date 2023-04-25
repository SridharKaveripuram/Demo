
using Microsoft.EntityFrameworkCore;

namespace Common
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _dbContext;

        public EmployeeRepository(EmployeeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            await _dbContext.Employees.AddAsync(employee);
        }
        
        public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
        {   
            return await _dbContext.Employees.FirstOrDefaultAsync(emp => employeeId == emp.Id); 
        }

        public async Task<Employee?> GetEmployeeByNameAsync(string name)
        {
            return await _dbContext.Employees.FirstOrDefaultAsync(emp => string.Equals(name, emp.Name));
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {            
            return  await Task.Run(() =>
            {
                return _dbContext.Employees.AsEnumerable();
            });
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(int pageNumber, int pageSize)
        {
            return await _dbContext.Employees.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            var emp = await GetEmployeeByIdAsync(employee.Id);
            if (emp != null)
            {
                emp.HourlyRate = employee.HourlyRate;
                emp.HoursWorked = employee.HoursWorked;
                emp.Name = employee.Name;
            }
        }

        public async Task DeleteEmployeeAsync(int employeeId)
        {
            var employee = await GetEmployeeByIdAsync(employeeId);
            if (employee != null)
            {
                _dbContext.Employees.Remove(employee);
            }
        }


        public async Task DeleteEmployeeByNameAsync(string name)
        {
            var employee = await GetEmployeeByNameAsync(name);
            if (employee != null)
            {
                _dbContext.Employees.Remove(employee);
            }
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
