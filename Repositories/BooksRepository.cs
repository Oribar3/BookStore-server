using System;
using book_store_server_side.data;
using book_store_server_side.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using static book_store_server_side.data.BooksContext;

namespace book_store_server_side.Repositories
{
    public class BooksRepository : IBooksRepository
    {
        private readonly BooksContext _context;
        public BooksRepository(BooksContext context)
        {
            _context = context;
        }


        public async Task<List<Book>> GetAllBooksAsync()
        {
            var books = await _context.Books.ToListAsync();
            return books;
        }

        public async Task<Book> GetBookByTitle(string title)
        {
            /*var book = await _context.Books.FindAsync(id);*/
            var book = await _context.Books.Where(b => b.Title == title).FirstOrDefaultAsync();
            return book;
        }
        public async Task<Book> GetBookById(int id)
        {
            /*var book = await _context.Books.FindAsync(id);*/
            var book = await _context.Books.Where(b => b.Id == id).FirstOrDefaultAsync();
            if (book == null)
            {
                var cartItemsToRemove = await _context.CartItems.Where(b => b.BookId == id).ToListAsync() ;
                 _context.CartItems.RemoveRange(cartItemsToRemove);
                await _context.SaveChangesAsync();
            }
            return book;
        }
        public async Task<int> AddBookAsync(NewBookModel newBookModel)
        {

            Book bookModel = new()
            {
                Title = newBookModel.Title,
                Description = newBookModel.Description,
                Price = newBookModel.Price,
                Image = newBookModel.Image,

            };
            _context.Add(bookModel);

            await _context.SaveChangesAsync();
            return bookModel.Id;
        }

    
        public async Task<Book> UpdateBookAsync(int bookId, NewBookModel updatedModel)
        {
            var book = await _context.Books.Where(b => b.Id == bookId).FirstOrDefaultAsync();
            if (book != null)
            {
                book.Description = updatedModel.Description;
                book.Title = updatedModel.Title;
                book.Price = updatedModel.Price;
                book.Image = updatedModel.Image;



                await _context.SaveChangesAsync();
            }

            return book;
        }

        public async Task<Book> UpdateByPatch(int bookId, JsonPatchDocument updatedBook)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book != null)
            {
                updatedBook.ApplyTo(book);
                await _context.SaveChangesAsync();
            }
            return book;
        }

        public async Task<int> DeleteById(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book != null)
            {
                _context.Books.Remove(book);
                var CartItems = _context.CartItems.Where(b => b.BookId == book.Id).Select(b => _context.CartItems.Remove(b));
                return await _context.SaveChangesAsync();
            }
            return -1;
        }

    }
}

