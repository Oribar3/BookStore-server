using System;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;
using book_store_server_side.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using book_store_server_side.data;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace book_store_server_side.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountRepository(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }
        public async Task<bool> isAdmin(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user != null)
            {
                // Check if the user has the admin role
                bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                return isAdmin;
            }

            return false;
        }

        //sign up for regular users request
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
                var roleExists = await _roleManager.RoleExistsAsync("User");

                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }
                await _userManager.AddToRoleAsync(user, "User");
               
            }
  
            return result;
        }

       

        //log in for regular users request + create token and return it

        public async Task<string> Login(LoginModel loginModel)
        {
            var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, false, false);
            if (!result.Succeeded)
            {
                return null;
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == loginModel.Email);
          
            string token = await NewToken(user);
            return token;
        }


        //new token for users func and return it as string
        private async Task<string> NewToken(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            };
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
          
                );
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> changeUserProperty(string property,string newVal, string userName)
        {


            var user = await _userManager.FindByNameAsync(userName);

            // Use the user ID to fetch the user from the database
            // Save the updated user information
            
            if (user==null)
                return null;
            property = property.ToLower();
            switch (property)
            {
                case "name": user.Name = newVal;
                    break;
                case "email":
                    {
                        user.NormalizedUserName = newVal;
                        user.UserName = newVal;
                        user.Email = newVal;
                    }
                    break;
                default:
                    return null;
            }
            var result = await _userManager.UpdateAsync(user);
            if(property=="email") return await NewToken(user);
            return result.Succeeded+"";

        }    

        public async Task<bool> changeUserPassword(string oldPassword,string newPassword,string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return false;
            var res = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return res.Succeeded;
       

        }
    }
}

