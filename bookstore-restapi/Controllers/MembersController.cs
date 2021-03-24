using bookstore_restapi.Data;
using bookstore_restapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace bookstore_restapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}/cart")]
        [Authorize]
        public async Task<ActionResult<CartDTO>> GetCart(string userId)
        {
            string userIDfromToken = User.Identity.Name;
            if (userIDfromToken != null && userId == userIDfromToken)
            {
                var booksInCart = _context.CartElements.Where(element => element.UserId == userId)
                    .Select(element => new BookInCartDTO { BookId = element.BookId, BookQnty = element.BookQnty });
                return new CartDTO { UserId = userIDfromToken, BooksInCart = await booksInCart.ToListAsync() };
            }
            return Forbid();
        }

        [HttpPut("{userId}/cart")]
        [Authorize]
        public async Task<ActionResult<CartElementDTO>> PostBookInCart(string userId, [FromBody] BookInCartDTO bookInCart)
        {
            string userIDfromToken = User.Identity.Name;
            if (userIDfromToken != null && userId == userIDfromToken)
            {
                var element = await _context.CartElements.Where(e => e.UserId == userId && e.BookId == bookInCart.BookId)
                    .FirstOrDefaultAsync();
                if (element == null)
                {
                    element = new CartElement { UserId = userIDfromToken, BookId = bookInCart.BookId, BookQnty = bookInCart.BookQnty };
                    _context.CartElements.Add(element);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    element.BookQnty += bookInCart.BookQnty;
                    _context.Entry(element).State = EntityState.Modified;
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!(_context.CartElements.Any(e => e.UserId == userId && e.BookId == bookInCart.BookId)))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                var elementDTO = new CartElementDTO { UserId = element.UserId, BookId = element.BookId, BookQnty = element.BookQnty };
                return CreatedAtAction(nameof(GetCart), new { userId = userId }, elementDTO);
            }
            return Forbid();
        }
    }
}
