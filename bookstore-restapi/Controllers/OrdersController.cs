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
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderDTO>> GetOrder(long id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            string userIDfromToken = User.Identity.Name;
            if (userIDfromToken != null && order.UserId == userIDfromToken)
            {
                var orderDTO = new OrderDTO { Id = order.Id, UserId = order.UserId };
                orderDTO.OrderBooks = _context.OrderBooks.Where(ob => ob.OrderId == orderDTO.Id)
                    .Select(ob => new BookInCartDTO { BookId = ob.BookId, BookQnty = ob.BookQnty });
                return orderDTO;
            }

            return Forbid();
        }

        // POST: api/Orders 
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderDTO>> PostOrder()
        {
            string userIDfromToken = User.Identity.Name;
            if (userIDfromToken == null)
                return Unauthorized();
            //проверить повторяются ли книги
            //лимит книг
            //проверить, что книги не пустые
            //очистить корзину после добавления
            var order = new Order { UserId = userIDfromToken };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            var booksInCart = _context.CartElements.Where(element => element.UserId == userIDfromToken);
            //два селекта вс форич
            var orderDTO = new OrderDTO { Id = order.Id, UserId = userIDfromToken };
            List<BookInCartDTO> booksForOrderDTO = new();
            foreach (var book in booksInCart)
            {
                _context.OrderBooks.Add(new OrderBook { OrderId = order.Id, BookId = book.BookId, BookQnty = book.BookQnty });
                booksForOrderDTO.Add(new BookInCartDTO { BookId = book.BookId, BookQnty = book.BookQnty });
            }
            await _context.SaveChangesAsync();
            orderDTO.OrderBooks = booksForOrderDTO;
            return CreatedAtAction("GetOrder", new { id = order.Id }, orderDTO);
        }
    }
}
