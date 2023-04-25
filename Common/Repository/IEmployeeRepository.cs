namespace Common
{
    public interface IEmployeeRepository : IDisposable
    {
        Task AddEmployeeAsync(Employee employee);
        Task<IEnumerable<Employee>> GetEmployeesAsync();
        Task<IEnumerable<Employee>> GetEmployeesAsync(int pageNumber,int pageSize);
        Task<Employee?> GetEmployeeByIdAsync(int employeeId);
        Task<Employee?> GetEmployeeByNameAsync(string name);
        Task UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int employeeId);
        Task DeleteEmployeeByNameAsync(string name);
        Task SaveAsync();
    }
}
