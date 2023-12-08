using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TatooStore.Models;
using TatooStore.Models.DataModels;
using TatooStore.Models.ViewModels.Account.Registration;

namespace TatooStore.Services.Repository.AccountRepository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountRepository(AppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> CreateEmployeeAsync(EmployeeRegistration model)
        {
            var user = new AppUser()
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                IsActiveUser = true,
            };
            var userResult = await _userManager.CreateAsync(user, model.Password);
            if (userResult.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, RoleList.employeeRole);
                if (roleResult.Succeeded)
                {
                    var employee = new Employee()
                    {
                        DateOfEmployment = DateTime.Now,
                        PassportNumber = model.PassportNumber,
                        User = user
                    };
                    var employeeResult = await AddEmployee(employee);
                    if (!employeeResult.Succeeded)
                    { 
                        await _userManager.DeleteAsync(user);
                    }
                    return employeeResult;
                }
                await _userManager.DeleteAsync(user);
                return roleResult;
            }
            return userResult;
        }
        private async Task<IdentityResult> AddEmployee(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            int result = await _context.SaveChangesAsync();
            return (result > 0)
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public string GetNameUserMenu(string userName)
        {
            AppUser user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            return user.FirstName + " " + user.LastName;
        }

        public async Task<IdentityResult> CreateCustomerAsync(CustomerRegistration model)
        {
            var user = new AppUser()
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                IsActiveUser = true,
            };
            var userResult = await _userManager.CreateAsync(user, model.Password);
            if (userResult.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, RoleList.customerRole);
                if (roleResult.Succeeded)
                {
                    var customer = new Customer()
                    {
                        DateOfBirth = model.DateOfBirth,
                        User = user
                    };
                    var customerResult = await AddCustomer(customer);
                    if (customerResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
                    }
                    else
                    {
                        await _userManager.DeleteAsync(user);
                    }
                    return customerResult;
                }
                await _userManager.DeleteAsync(user);
                return roleResult;
            }
            return userResult;
        }
        private async Task<IdentityResult> AddCustomer(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            int result = await _context.SaveChangesAsync();
            return (result > 0)
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<List<Employee>> GetAllEmployeeAsync() 
        {
            return await _context.Employees.Include(s => s.User).OrderByDescending(s => s.User.IsActiveUser).ThenBy(s => s.DateOfEmployment).ToListAsync();
        }

        public async Task<int> DeleteEmployeeAsync(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            user.IsActiveUser = false;
            _context.Users.Update(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> IsActiveUserAsync(string userName)
        {
            return await _context.Users.Where(u => u.UserName == userName).Select(u => u.IsActiveUser).FirstOrDefaultAsync();
        }

        public async Task<Employee> GetEmployeeAsync(string id)
        {
            return await _context.Employees.Include(u => u.User).FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<int> UpdateEmployeeAsync(EmployeeRegistration employeeView, Employee employeeModel)
        {
            if (employeeView.Password != null)
            {
                await _userManager.RemovePasswordAsync(employeeModel.User);
                await _userManager.AddPasswordAsync(employeeModel.User, employeeView.Password);
            }
            await _userManager.SetUserNameAsync(employeeModel.User, employeeView.Email);
            await _userManager.SetEmailAsync(employeeModel.User, employeeView.Email);
            employeeModel.User.FirstName = employeeView.FirstName;
            employeeModel.User.MiddleName = employeeView.MiddleName;
            employeeModel.User.LastName = employeeView.LastName;
            employeeModel.User.Email = employeeView.Email;
            employeeModel.User.UserName = employeeView.Email;
            employeeModel.User.PhoneNumber = employeeView.PhoneNumber;
            employeeModel.PassportNumber = employeeView.PassportNumber;
            _context.Employees.Update(employeeModel);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> GetCustomerIdAsync(string userName)
        {
            AppUser appUser = await _userManager.FindByNameAsync(userName);
            return await _context.Customers.Where(c => c.UserId == appUser.Id).Select(c => c.CustomerId).FirstOrDefaultAsync();
        }

        public async Task<int> GetEmployeeIdAsync(string userName)
        {
            AppUser appUser = await _userManager.FindByNameAsync(userName);
            return await _context.Employees.Where(c => c.UserId == appUser.Id).Select(c => c.EmployeeId).FirstOrDefaultAsync();
        }
    }
}
