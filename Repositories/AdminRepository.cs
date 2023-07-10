using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using book_store_server_side.data;
using book_store_server_side.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace book_store_server_side.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly BooksContext _context;

        public AdminRepository(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager, BooksContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IdentityResult> SignUp(SignupModel signupModel)
        {
            AppUser user = new()
            {
                Name = signupModel.Name,
                Email = signupModel.Email,
                UserName = signupModel.Email,

            };
            var result = await _userManager.CreateAsync(user, signupModel.Password);
            if (result.Succeeded)
            {

                var roleExists = await _roleManager.RoleExistsAsync("Admin");

                if (!roleExists)
                {
                    var role = new IdentityRole("Admin");
                    await _roleManager.CreateAsync(role);
                }

                await _userManager.AddToRoleAsync(user, "Admin");
            }
            return result;


        }

        public async Task<int> SetDiscount(int value)
        {

            Discount discount = new()
            {
                value = value
            };
            _context.Add(discount);
            await _context.SaveChangesAsync();
            return discount.value;

        }

        public async Task<int> GetDiscount()
        {
            var lastDiscount = await _context.discounts.OrderByDescending(d => d.Id).FirstOrDefaultAsync();
            if (lastDiscount == null) return 0;
            return lastDiscount.value;
        }

        public async Task<bool> DeleteUser(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded ? true : false;
            }
            return false;
        }
    }
}