using Library_Management_System.Entities;
using Library_Management_System.Models;
using Library_Management_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly IBookRepository _IBookRepository;

        public BookController(IBookRepository iBookRepository) 
        { 
            _IBookRepository = iBookRepository;
        }

        /// <summary>
        /// API Endpoint to add Book into database table by calling related repository method.
        /// </summary>
        /// <param name="book"></param>
        [HttpPost]
        public async Task<IActionResult> AddBook(Book book)
        {
            string uniqueId = Guid.NewGuid().ToString();

            BookEntity ToAdd = new BookEntity()
            {
                UId = uniqueId,
                Id = uniqueId,
                DocumentType = "Book",
                Version = 1,
                CreatedBy = "Admin",
                CreatedOn = DateTime.Now,
                Active = true,
                Archived = false,

                Title = book.Title,
                Author = book.Author,
                PublishedDate = book.PublishedDate,
                ISBN = book.ISBN,
                IsIssued = book.IsIssued
            };

            bool result = await _IBookRepository.AddBook(ToAdd);

            if (!result)
            {
                return StatusCode(500, "Failed to add the book.");
            }

            return Ok("Book added successfully.");
        }

        /// <summary>
        /// API Endpoint to get book by specified Book UId from database table by calling related repository method.
        /// </summary>
        /// <param name="UId"></param>
        [HttpGet("{UId}")]
        public async Task<IActionResult> GetBookByUId(string UId)
        {
            var bookEntity = await _IBookRepository.GetBookByUId(UId);
            
            if (bookEntity == null)
            {
                return NotFound("Data not found");
            }

            Book toReturn = new Book() 
            { 
                UId = bookEntity.UId,
                Title = bookEntity.Title, 
                Author = bookEntity.Author,
                PublishedDate = bookEntity.PublishedDate,
                ISBN = bookEntity.ISBN,
                IsIssued = bookEntity.IsIssued
            };

            return Ok(toReturn);
        }

        /// <summary>
        /// API Endpoint to get book by specified Book Name or Title from database table by calling related repository method.
        /// </summary>
        /// <param name="Name"></param>
        [HttpGet("{Name}")]
        public async Task<IActionResult> GetBookByName(string Name)
        {
            var bookEntity = await _IBookRepository.GetBookByName(Name);

            if (bookEntity == null)
            {
                return NotFound("Data not found");
            }

            Book toReturn = new Book()
            {
                UId = bookEntity.UId,
                Title = bookEntity.Title,
                Author = bookEntity.Author,
                PublishedDate = bookEntity.PublishedDate,
                ISBN = bookEntity.ISBN,
                IsIssued = bookEntity.IsIssued
            };

            return Ok(toReturn);
        }

        /// <summary>
        /// API Endpoint to get all books from database table by calling related repository method.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllBook()
        {
            var Getbooks = await _IBookRepository.GetAllBook();

            List<Book> books = new List<Book>();

            foreach (var book in Getbooks)
            {
                Book toAdd = new Book()
                {
                    UId = book.UId,
                    Title = book.Title,
                    Author = book.Author,
                    PublishedDate = book.PublishedDate,
                    ISBN = book.ISBN,
                    IsIssued = book.IsIssued
                };

                books.Add(toAdd);
            }

            return Ok(books);
        }

        /// <summary>
        /// API Endpoint to get all not issued books from database table by calling related repository method.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllNotIssuedBook()
        {
            var Getbooks = await _IBookRepository.GetAllNotIssuedBook();

            List<Book> books = new List<Book>();

            foreach (var book in Getbooks)
            {
                Book toAdd = new Book()
                {
                    UId = book.UId,
                    Title = book.Title,
                    Author = book.Author,
                    PublishedDate = book.PublishedDate,
                    ISBN = book.ISBN,
                    IsIssued = book.IsIssued
                };

                books.Add(toAdd);
            }

            return Ok(books);
        }

        /// <summary>
        /// API Endpoint to get all issued books from database table by calling related repository method.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllIssuedBook()
        {
            var Getbooks = await _IBookRepository.GetAllIssuedBook();

            List<Book> books = new List<Book>();

            foreach (var book in Getbooks)
            {
                Book toAdd = new Book()
                {
                    UId = book.UId,
                    Title = book.Title,
                    Author = book.Author,
                    PublishedDate = book.PublishedDate,
                    ISBN = book.ISBN,
                    IsIssued = book.IsIssued
                };

                books.Add(toAdd);
            }

            return Ok(books);
        }

        /// <summary>
        /// API Endpoint to update Book into database table by calling related repository method.
        /// </summary>
        /// <param name="book"></param>
        [HttpPut]
        public async Task<IActionResult> UpdateBook(Book book)
        {
            BookEntity bookEntity = await _IBookRepository.GetBookByUId(book.UId);

            if (bookEntity == null)
            {
                return NotFound("Data not found");
            }

            bookEntity.Active = false;
            bookEntity.Archived = true;
            bool isExecuted = await _IBookRepository.UpdateBook(bookEntity);

            if (!isExecuted)
            {
                return StatusCode(500, "Failed to update the book.");
            }

            bookEntity.Id = Guid.NewGuid().ToString();
            bookEntity.Version = bookEntity.Version + 1;
            bookEntity.UpdatedBy = "Users";
            bookEntity.UpdatedOn = DateTime.Now;
            bookEntity.Active = true;
            bookEntity.Archived = false;
            bookEntity.Title = book.Title;
            bookEntity.Author = book.Author;
            bookEntity.PublishedDate = book.PublishedDate;
            bookEntity.ISBN = book.ISBN;
            bookEntity.IsIssued = book.IsIssued;

            bool result = await _IBookRepository.UpdateBook(bookEntity);

            if (!result)
            {
                return StatusCode(500, "Failed to update the book.");
            }

            return Ok("Book updated successfully.");
        }

        /// <summary>
        /// API Endpoint to remove Book specified by UId from database table by calling related repository method.
        /// </summary>
        /// <param name="UId"></param>
        [HttpDelete]
        public async Task<IActionResult> RemoveBookByUId(string UId)
        {
            BookEntity toDelete = await _IBookRepository.GetBookByUId(UId);

            if (toDelete == null)
            {
                return NotFound("Data not found");
            }

            bool isDeleted = await _IBookRepository.RemoveBook(toDelete);

            if (!isDeleted)
            {
                return BadRequest("Data not removed");
            }

            return Ok("Book Removed Successfully");
        }
    }
}
