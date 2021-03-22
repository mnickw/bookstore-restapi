using System;

namespace bookstore_restapi.Models
{
    public class BookDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public double Price { get; set; }
    }
}
