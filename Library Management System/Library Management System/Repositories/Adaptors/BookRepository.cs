using Library_Management_System.Entities;
using Library_Management_System.Models;
using Library_Management_System.Repositories.Interfaces;
using Microsoft.Azure.Cosmos;

namespace Library_Management_System.Repositories.Adaptors
{
    public class BookRepository : IBookRepository
    {
        private readonly Container _container;

        public BookRepository(DBConfig dBConfig) 
        {
            _container = dBConfig.GetContainer();
        }

        public async Task<bool> AddBook(BookEntity bookEntity)
        {
            var rs = await _container.CreateItemAsync(bookEntity);
            return rs != null ;
        }

        public async Task<List<BookEntity>> GetAllBook()
        {
            var toReturn = _container.GetItemLinqQueryable<BookEntity>(true).Where(x => x.Active == true && x.Archived == false && x.DocumentType == "Book").ToList();
            return toReturn;
        }

        public async Task<List<BookEntity>> GetAllIssuedBook()
        {
            var toReturn = _container.GetItemLinqQueryable<BookEntity>(true).Where(x => x.IsIssued && x.Active && x.Archived == false && x.DocumentType == "Book").ToList();
            return toReturn;
        }

        public async Task<List<BookEntity>> GetAllNotIssuedBook()
        {
            var toReturn = _container.GetItemLinqQueryable<BookEntity>(true).Where(x => x.IsIssued == false && x.Active && x.Archived == false && x.DocumentType == "Book").ToList();
            return toReturn;
        }

        public async Task<BookEntity> GetBookByName(string Name)
        {
            var toReturn = _container.GetItemLinqQueryable<BookEntity>(true).Where(x => x.Title == Name && x.Active && x.Archived == false && x.DocumentType == "Book").FirstOrDefault();
            return toReturn;
        }

        public async Task<BookEntity> GetBookByUId(string UId)
        {
            var toReturn = _container.GetItemLinqQueryable<BookEntity>(true).Where(x => x.UId == UId && x.Active && x.Archived == false && x.DocumentType == "Book").FirstOrDefault();
            return toReturn;
        }

        public async Task<bool> RemoveBook(BookEntity bookEntity)
        {
            var rs = await _container.DeleteItemAsync<BookEntity>(bookEntity.UId, new PartitionKey(bookEntity.DocumentType));
            return rs != null;
        }

        public async Task<bool> UpdateBook(BookEntity bookEntity)
        {
            var rs = await _container.ReplaceItemAsync(bookEntity, bookEntity.UId);
            return rs != null;
        }
    }
}
