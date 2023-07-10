using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using book_store_server_side.Models;
using book_store_server_side.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace book_store_server_side.Controllers
{
 
    [Route("api/[controller]")]
    [ApiController]

    public class BooksController : ControllerBase
    {
        private readonly IBooksRepository _booksRepository;

        public BooksController(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllBooks()
        {
            var res = await _booksRepository.GetAllBooksAsync();
            if (res?.Count > 0)
            {
                return Ok(res);
            }
            return NotFound();
        }

        [HttpGet("title/{title}")]
        public async Task<IActionResult> GetBookByTitle([FromRoute] string title)
        {
            var res = await _booksRepository.GetBookByTitle(title);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

       
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetBookById([FromRoute] int id)
        {
            var res = await _booksRepository.GetBookById(id);
            if (res == null)
            {
               
                return NotFound();
            }
            return Ok(res);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("")]
        public async Task<IActionResult> AddNewBook([FromBody] NewBookModel newBookModel)
        {
           
            var id = await _booksRepository.AddBookAsync(newBookModel);
            if (id == -1)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetBookById), new { id = id, controller = "books" }, id);
        }

        [HttpPut("{id}")]
       [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateBook([FromRoute] int id, [FromBody] NewBookModel newBookModel)
        {
            var book = await _booksRepository.UpdateBookAsync(id, newBookModel);
            if (book == null)
            {
                return BadRequest();
            }
            return Ok(book);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateByPatch([FromRoute] int id, [FromBody] JsonPatchDocument updatedBook)
        {
            var book = await _booksRepository.UpdateByPatch(id,updatedBook);
            if (book == null)
            {
                return BadRequest();
            }
            return Ok(book);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteBook([FromRoute] int id)
        {
            var changes = await _booksRepository.DeleteById(id);
            if (changes == -1)
            {
                return BadRequest();
            }
            return Ok();
        }

        
    }

}

