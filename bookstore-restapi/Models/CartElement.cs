using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bookstore_restapi.Models
{
    public class CartElement
    {
        public string UserId { get; set; }
        public long BookId { get; set; }
        public int BookQnty { get; set; }
        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }
    }
}
