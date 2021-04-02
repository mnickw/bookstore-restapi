using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bookstore_restapi.Models;

namespace bookstore_frontendASPNETCoreMVC.Services
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookDTO>> GetBooks();
        Task<BookDTO> GetBookById(int bookId);
        Task DeleteBookById(int bookId);
    }
}
