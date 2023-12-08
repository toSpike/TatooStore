using Microsoft.AspNetCore.Identity;
using TatooStore.Models.DataModels;
using TatooStore.Models.ViewModels.Account.Registration;

namespace TatooStore.Services.Repository.AccountRepository
{
    public interface IAccountRepository 
    {
        Task<IdentityResult> CreateEmployeeAsync(EmployeeRegistration model);
        Task<IdentityResult> CreateCustomerAsync(CustomerRegistration model);
        string GetNameUserMenu(string userName);
        Task<List<Employee>> GetAllEmployeeAsync();
        Task<int> DeleteEmployeeAsync(string id);
        Task<bool> IsActiveUserAsync(string userName);
        Task<Employee> GetEmployeeAsync(string id);
        Task<int> UpdateEmployeeAsync(EmployeeRegistration employeeView, Employee employeeModel);
        Task<int> GetCustomerIdAsync(string userName);
        Task<int> GetEmployeeIdAsync(string userName);
    }
}
