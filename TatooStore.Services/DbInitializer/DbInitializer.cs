using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TatooStore.Models;
using TatooStore.Models.DataModels;

namespace TatooStore.Services.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;

        }

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception)
            {

            }

            if (!_roleManager.RoleExistsAsync(RoleList.adminRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(RoleList.adminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(RoleList.employeeRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(RoleList.customerRole)).GetAwaiter().GetResult();

                var userResult = _userManager.CreateAsync(new AppUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                    FirstName = "Иван",
                    LastName = "Иванов",
                    MiddleName = "Иванович",
                    PhoneNumber = "+79999999999",
                    IsActiveUser = true,
                }, "Admin_01").GetAwaiter().GetResult();

                if (userResult.Succeeded)
                {
                    AppUser user = _context.Users.FirstOrDefault(u => u.UserName == "admin@gmail.com");
                    IEnumerable<string> roles = new string[]
                    {
                        RoleList.adminRole,
                        RoleList.employeeRole,
                    };
                    var roleResult = _userManager.AddToRolesAsync(user, roles).GetAwaiter().GetResult();
                    if (roleResult.Succeeded)
                    {
                        var employee = new Employee()
                        {
                            DateOfEmployment = DateTime.Now,
                            PassportNumber = "1234123456",
                            User = user
                        };
                        _context.Employees.Add(employee);
                        _context.SaveChanges();
                    }
                }
            }
            return;
        }
    }
}
