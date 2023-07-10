using System;
using System.Security.Claims;
using book_store_server_side.Models;
using Microsoft.AspNetCore.Identity;

namespace book_store_server_side.Repositories
{
     public interface IAccountRepository
    {
        Task<bool> isAdmin(string userName);
        Task<IdentityResult> SignUp(SignupModel signupModel);
        Task<string> Login(LoginModel loginModel);
        Task<string> changeUserProperty(string property,string newVal, string userName);
        Task<bool> changeUserPassword(string oldPassword, string newPassword, string userName);
    }
}

