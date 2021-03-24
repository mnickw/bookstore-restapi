using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bookstore_restapi.Models
{
    public class OrderBook
    {
        public long OrderId { get; set; }
        public long BookId { get; set; }
        public int BookQnty { get; set; }
        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }
    }
}
