using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookstore_restapi.Models
{
    public class OrderDTO
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public IEnumerable<BookInCartDTO> OrderBooks { get; set; }
    }
}
