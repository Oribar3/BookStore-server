using System;
using book_store_server_side.data;
using book_store_server_side.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace book_store_server_side.Repositories
{
	public class CartRepository:ICartRepository
	{
        private readonly UserManager<AppUser> _userManager;
        private readonly BooksContext _context;

        public CartRepository(UserManager<AppUser> userManager, BooksContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<CartItems[]> GetCartBooks(string userName)
        {
       
           var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                var cartBooks = await _context.CartItems.Where(b => b.AppUserId == user.Id).ToListAsync();
                return cartBooks.ToArray();
            }
            else return null;

        }

        public async Task<bool> AddBookToCart(string userName, int bookId)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var book = await _context.Books.FindAsync(bookId);

            if (user == null && book == null)
                return false;

            var cartBooks = await _context.CartItems
                .Where(b => b.AppUserId == user.Id && b.BookId == book.Id)
                .ToListAsync();

            if (cartBooks.Count == 0)
            {
                var CartItems = new CartItems
                {
                    AppUserId = user.Id,
                    BookId = book.Id,
                    Amount = 1
                };
                _context.Add(CartItems);

            }
            else
            {
                foreach(var cartItem in cartBooks)
                {
                    cartItem.Amount++;
                }
            }
                
            await _context.SaveChangesAsync();

            return true;
            }

        public async Task<bool> RemoveBookFromCart(string userName, int bookId)
        {

            var user = await _userManager.FindByNameAsync(userName);
            var book = await _context.Books.FindAsync(bookId);

            if (user == null && book == null)
                return false;

            var cartBooks = await _context.CartItems
                .Where(b => b.AppUserId == user.Id && b.BookId == book.Id)
                .ToListAsync();
            foreach (var cartItem in cartBooks)
            {
                if (cartItem.Amount == 1)
                    _context.CartItems.Remove(cartItem);
                else
                  cartItem.Amount--;
            }
            _context.SaveChangesAsync();

            return true;
          }
          
        }
    
 }

