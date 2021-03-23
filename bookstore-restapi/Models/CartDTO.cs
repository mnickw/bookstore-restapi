using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookstore_restapi.Models
{
    public class CartDTO
    {
        public string UserId { get; set; }
        public IEnumerable<BookInCartDTO> BooksInCart { get; set; }
    }
}
