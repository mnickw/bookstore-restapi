using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookstore_restapi.Models
{
    public class BookForPostDTO
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public double Price { get; set; }
    }
}
