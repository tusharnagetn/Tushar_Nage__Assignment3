using Library_Management_System.Entities;

namespace Library_Management_System.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<bool> AddBook(BookEntity bookEntity);
        Task<BookEntity> GetBookByUId(string UId);
        Task<BookEntity> GetBookByName(string Name);
        Task<List<BookEntity>> GetAllBook();
        Task<List<BookEntity>> GetAllNotIssuedBook();
        Task<List<BookEntity>> GetAllIssuedBook();
        Task<bool> UpdateBook(BookEntity bookEntity);
        Task<bool> RemoveBook(BookEntity bookEntity);
    }
}
