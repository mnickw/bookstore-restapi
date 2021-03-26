using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bookstore_restapi.Data;
using bookstore_restapi.Models;
using Microsoft.AspNetCore.Authorization;
using bookstore_restapi.Services;

namespace bookstore_restapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            var books = await _bookRepository.GetBooks();
            return Ok(books);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBook(long id)
        {
            var book = await _bookRepository.GetBookById(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        [Authorize("change:books")]
        public async Task<IActionResult> PutBook(long id, BookForPostDTO bookForPostDTO)
        {
            var bookDTO = new BookDTO
            {
                Id = id,
                Author = bookForPostDTO.Author,
                Name = bookForPostDTO.Name,
                Price = bookForPostDTO.Price
            };
            var putResult = await _bookRepository.ChangeBook(bookDTO);
            if (putResult == ChangeItemResult.NoItemInDB)
                return NotFound();
            if (putResult == ChangeItemResult.ConcurrencyException_TryAgain)
                return StatusCode(503, "Concurrency error, try again");
            return NoContent();
        }

        // POST: api/Books
        [HttpPost]
        [Authorize("change:books")]
        public async Task<ActionResult<BookDTO>> PostBook(BookForPostDTO bookForPostDTO)
        {
            var bookDTO = await _bookRepository.AddBook(bookForPostDTO);
            return CreatedAtAction(nameof(GetBook), new { id = bookDTO.Id }, bookDTO);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [Authorize("change:books")]
        public async Task<IActionResult> DeleteBook(long id)
        {
            var result = await _bookRepository.DeleteBookById(id);

            if (result == ChangeItemResult.NoItemInDB)
                return NotFound();
            if (result == ChangeItemResult.Ok)
                return NoContent();
            throw new Exception();
        }
    }
}
