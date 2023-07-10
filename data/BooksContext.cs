using System;
using book_store_server_side.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace book_store_server_side.data
{
    public class BooksContext : IdentityDbContext<AppUser>
    {
        public BooksContext(DbContextOptions<BooksContext> options) : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Discount> discounts{ get; set; }
        public DbSet<CartItems> CartItems { get; set; }


    }
}

