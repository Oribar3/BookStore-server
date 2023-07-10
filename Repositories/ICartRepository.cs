using System;
using book_store_server_side.Models;

namespace book_store_server_side.Repositories
{
	public interface ICartRepository
	{

        Task<CartItems[]> GetCartBooks(string userName);
        Task<bool> RemoveBookFromCart(string userName, int bookId);
        Task<bool> AddBookToCart(string userName, int bookId);

    }
}

