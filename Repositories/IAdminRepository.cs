using System;
using book_store_server_side.Models;
using Microsoft.AspNetCore.Identity;

namespace book_store_server_side.Repositories
{
	public interface IAdminRepository
	{
        Task<IdentityResult> SignUp(SignupModel signupModel);
        Task<int> SetDiscount(int value);
        Task<int> GetDiscount();
        Task<bool> DeleteUser(string userName);


    }
}

