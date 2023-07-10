using System;
namespace book_store_server_side.Models
{
	public class UpdateUserPasswordModel
	{
		public string oldPassword { get; set; }
		public string newPassword { get; set; }
	}
}

