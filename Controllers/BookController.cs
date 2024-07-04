using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EFTraining.Entities;
using EFTraining.Data;
using Microsoft.EntityFrameworkCore;
using EFTraining.Services;

namespace EFTraining.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService bookService;

        public BookController(IBookService service) { 
            bookService = service;
        } 

        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetAllBooks()
        {
            return await bookService.GetAllBooks();
        }

        [HttpPost]
        public async Task<ActionResult<List<Book>>> AddBook(Book book)
        {
            return await bookService.AddBook(book);
        }
    }
}
