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

namespace bookstore_restapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            return await _context.Books.Select(b =>
                new BookDTO { Id = b.Id, Author = b.Author,
                    Name = b.Name, Price = b.Price }).ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBook(long id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return new BookDTO { Id = book.Id, Author = book.Author,
                Name = book.Name, Price = book.Price };
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        [Authorize("change:books")]
        public async Task<IActionResult> PutBook(long id, BookDTO bookDTO)
        {
            if (id != bookDTO.Id)
            {
                return BadRequest();
            }

            var book = new Book { Id = bookDTO.Id, Author = bookDTO.Author,
                Name = bookDTO.Name, Price = bookDTO.Price };
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        [HttpPost]
        [Authorize("change:books")]
        public async Task<ActionResult<BookDTO>> PostBook(BookForPostDTO bookForPostDTO)
        {
            var book = new Book
            {
                Author = bookForPostDTO.Author,
                Name = bookForPostDTO.Name,
                Price = bookForPostDTO.Price
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            var bookDTO = new BookDTO
            {
                Id = book.Id,
                Author = bookForPostDTO.Author,
                Name = bookForPostDTO.Name,
                Price = bookForPostDTO.Price
            };
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, bookDTO);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [Authorize("change:books")]
        public async Task<IActionResult> DeleteBook(long id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(long id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
