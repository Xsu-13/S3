using EFTraining.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EFTraining.Services
{
    public interface IBookService
    {
        Task<ActionResult<List<Book>>> GetAllBooks();
        Task<ActionResult<List<Book>>> AddBook(Book book);
    }
}
