using bookstore_frontendASPNETCoreMVC.Services;
using bookstore_restapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace bookstore_frontendASPNETCoreMVC.Controllers
{
    public class BooksController : Controller
    {
        IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // GET: BooksController
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var books = await _bookRepository.GetBooks();
            return View(books);
        }

        // GET: BooksController/Details/5
        public async Task<ActionResult> DetailsAsync(int id)
        {
            var book = await _bookRepository.GetBookById(id);
            return View(book);
        }

        // GET: BooksController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: BooksController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(IndexAsync));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: BooksController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        // POST: BooksController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(IndexAsync));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: BooksController/Delete/5
        [Authorize(Roles = "Moderator")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var book = await _bookRepository.GetBookById(id);
            return View(book);
        }

        // POST: BooksController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator")]
        public async Task<ActionResult> DeleteConfirmedAsync(int id)
        {
            try
            {
                await _bookRepository.DeleteBookById(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
