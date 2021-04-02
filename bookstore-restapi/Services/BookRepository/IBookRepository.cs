using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bookstore_restapi.Models;

namespace bookstore_restapi.Services.BookRepository
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookDTO>> GetBooks();
        Task<BookDTO> GetBookById(long bookId);
        Task<ChangeItemResult> ChangeBook(BookDTO book);
        Task<BookDTO> AddBook(BookForPostDTO bookForPostDTO);
        Task<ChangeItemResult> DeleteBookById(long bookId);
        bool BookExists(long bookId);
    }
}
