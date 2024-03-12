﻿using bsStoreApp.Entity.Models;
using bsStoreApp.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bsStoreApp.Presentation.Controllers
{
    [ApiController]
    [Route("api/Books")]
    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _services;

        public BooksController(IServiceManager services)
        {
            _services = services;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var Getbooks = _services.BookService.GetAllBooks(false);
            return Ok(Getbooks);
        }
        [HttpGet("GetOneBook")]
        public IActionResult GetOneBook(int id)
        {
            var Getonebooks = _services.BookService.GetOneBook(id, false);
            if (Getonebooks == null)
            {
                return NotFound("Girilen ID Geçersiz!!");
            }
            else
            {
                return Ok(Getonebooks);
            }
        }
        [HttpPost]
        public IActionResult AddBook(Book book)
        {
            _services.BookService.CreateOneBook(book);
            return Ok("Kitap Eklendi.");
        }
        [HttpDelete("DeleteBook")]
        public IActionResult DeleteBook(int id)
        {
            _services.BookService.DeleteOneBook(id, false);
            return Ok();
        }
        [HttpPut]
        public IActionResult UpdateBook(int id, Book book)
        {
            if (book == null)
            {
                return NotFound();
            }
            else
            {
                _services.BookService.UpdateOneBook(id, book, true);
                return Ok(book);
            }
        }
    }
}
