using System;
using book_store_server_side.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace book_store_server_side.Repositories
{
    public interface IBooksRepository
    {
        Task<List<Book>> GetAllBooksAsync();
        Task<Book> GetBookByTitle(string title);
        Task<Book> GetBookById(int id);

        Task<int> AddBookAsync(NewBookModel newBookModel);
        Task<Book> UpdateBookAsync(int bookId, NewBookModel updatedModel);
        Task<Book> UpdateByPatch(int bookId, JsonPatchDocument updatedBook);
        Task<int> DeleteById(int bookId);
    }
}

