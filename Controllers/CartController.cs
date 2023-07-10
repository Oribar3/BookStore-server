using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using book_store_server_side.Models;
using book_store_server_side.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace book_store_server_side.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;


        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpGet("")]
        public async Task<CartItems[]> GetAllBooksInCart()
        {
            var userName = User.Identity.Name;
            if (userName == null)
                return null;
            var result = await _cartRepository.GetCartBooks(userName);
            return result;
        }

        [HttpPost("{bookId}")]
        public async Task<bool> AddBookToCart([FromRoute]int bookId)
        {
            var userName = User.Identity.Name;
            if (userName == null)
                return false;
            var result = await _cartRepository.AddBookToCart(userName,bookId);
            return result;
        }

        [HttpDelete("{bookId}")]
        public async Task<bool> RemoveBookFromCart([FromRoute] int bookId)
        {
            var userName = User.Identity.Name;
            if (userName == null)
                return false;
            var result = await _cartRepository.RemoveBookFromCart(userName, bookId);
            return result;
        }
    }
}

