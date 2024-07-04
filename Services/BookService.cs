using EFTraining.Data;
using EFTraining.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFTraining.Services
{
    public class BookService : IBookService
    {
        private readonly DBContext dbContext;

        public BookService(DBContext context)
        {
            dbContext = context;
        }

        public async Task<ActionResult<List<Book>>> AddBook(Book book)
        {
            dbContext.Books.Add(book);
            await dbContext.SaveChangesAsync();
            return await dbContext.Books.ToListAsync();
        }

        public async Task<ActionResult<List<Book>>> GetAllBooks()
        {
            return await dbContext.Books.ToListAsync();
        }
    }
}
