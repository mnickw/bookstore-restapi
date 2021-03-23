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
        private readonly IAuthorizationService _authorizationService;
        private readonly ApplicationDbContext _context;

        public MembersController(IAuthorizationService authorizationService,
                                 ApplicationDbContext context)
        {
            _authorizationService = authorizationService;
            _context = context;
        }

        [HttpGet("{userId}/cart")]
        public async Task<ActionResult<CartDTO>> Cart(string userId)
        {
            var booksInCart = _context.CartElements.Where(element => element.UserId == userId)
                .Select(element => new BookInCartDTO { BookId = element.BookId, BookQnty = element.BookQnty });
            return new CartDTO { UserId = userId, BooksInCart = await booksInCart.ToListAsync() };
        }
    }
}
