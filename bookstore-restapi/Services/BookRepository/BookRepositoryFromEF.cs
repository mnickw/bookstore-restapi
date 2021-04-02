using bookstore_restapi.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using bookstore_restapi.Data;
using Microsoft.EntityFrameworkCore;

namespace bookstore_restapi.Services.BookRepository
{
    public class BookRepositoryFromEF : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepositoryFromEF(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookDTO>> GetBooks()
        {
            return await _context.Books.Select(b =>
                new BookDTO
                {
                    Id = b.Id,
                    Author = b.Author,
                    Name = b.Name,
                    Price = b.Price
                }).ToListAsync();
        }

        public async Task<BookDTO> GetBookById(long bookId)
        {
            var book = await _context.Books.FindAsync(bookId);

            if (book == null)
                return null;

            return new BookDTO
            {
                Id = book.Id,
                Author = book.Author,
                Name = book.Name,
                Price = book.Price
            };
        }

        public async Task<ChangeItemResult> ChangeBook(BookDTO bookDTO)
        {
            var book = new Book
            {
                Id = bookDTO.Id,
                Author = bookDTO.Author,
                Name = bookDTO.Name,
                Price = bookDTO.Price
            };
            
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(bookDTO.Id))
                    return ChangeItemResult.NoItemInDB;
                return ChangeItemResult.ConcurrencyException_TryAgain;
            }

            return ChangeItemResult.Ok;
        }

        public async Task<BookDTO> AddBook(BookForPostDTO bookForPostDTO)
        {
            var book = new Book
            {
                Author = bookForPostDTO.Author,
                Name = bookForPostDTO.Name,
                Price = bookForPostDTO.Price
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            var bookDTO = new BookDTO
            {
                Id = book.Id,
                Author = bookForPostDTO.Author,
                Name = bookForPostDTO.Name,
                Price = bookForPostDTO.Price
            };
            return bookDTO;
        }

        public async Task<ChangeItemResult> DeleteBookById(long bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
                return ChangeItemResult.NoItemInDB;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return ChangeItemResult.Ok;
        }

        public bool BookExists(long bookId) => _context.Books.Any(e => e.Id == bookId);
    }
}
