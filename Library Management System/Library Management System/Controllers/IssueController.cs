using Library_Management_System.Entities;
using Library_Management_System.Models;
using Library_Management_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IssueController : Controller
    {
        private readonly IIssueRepository _IIssueRepository;
        private readonly IMemberRepository _IMemberRepository;
        private readonly IBookRepository _IBookRepository;

        public IssueController(IIssueRepository iIssueRepository, IMemberRepository iMemberRepository, IBookRepository iBookRepository) 
        { 
            _IIssueRepository = iIssueRepository;
            _IMemberRepository = iMemberRepository;
            _IBookRepository = iBookRepository;
        }

        /// <summary>
        /// API Endpoint to add issue into database and updating specified book as issued table by calling related repository method.
        /// </summary>
        /// <param name="issue"></param>
        [HttpPost]
        public async Task<IActionResult> AddIssueAndUpdateBookIsIssued(Issue issue)
        {
            MemberEntity memberEntity = await _IMemberRepository.GetMemberByUId(issue.MemberId);

            if (memberEntity == null)
            {
                return NotFound("Invalid MemberId");
            }

            BookEntity bookEntity = await _IBookRepository.GetBookByUId(issue.BookId);

            if (bookEntity == null)
            {
                return NotFound("Invalid BookId");
            }

            if (bookEntity.IsIssued)
            {
                return BadRequest("Book is already issued to some one");
            }

            string uniqueId = Guid.NewGuid().ToString();

            IssueEntity ToAdd = new IssueEntity()
            {
                UId = uniqueId,
                Id = uniqueId,
                DocumentType = "Issue",
                Version = 1,
                CreatedBy = "User",
                CreatedOn = DateTime.Now,
                Active = true,
                Archived = false,

                BookId = issue.BookId,
                MemberId = issue.MemberId,
                IssueDate = issue.IssueDate,
                ReturnDate = issue.ReturnDate,
                IsReturned = false
            };

            bool result = await _IIssueRepository.AddIssue(ToAdd);

            if (!result) 
            {
                return BadRequest("While adding Issued book.");
            }

            bookEntity.Active = false;
            bookEntity.Archived = true;
            bool isExecuted = await _IBookRepository.UpdateBook(bookEntity);

            if (!isExecuted)
            {
                return BadRequest("Failed to update the book.");
            }

            bookEntity.Id = Guid.NewGuid().ToString();
            bookEntity.Version = bookEntity.Version + 1;
            bookEntity.UpdatedBy = "Users";
            bookEntity.UpdatedOn = DateTime.Now;
            bookEntity.Active = true;
            bookEntity.Archived = false;
            bookEntity.IsIssued = true; // here added book is issued to some one

            bool result2 = await _IBookRepository.UpdateBook(bookEntity);

            if (!result2)
            {
                return BadRequest("Failed to update the book.");
            }

            return Ok("Book issued successfully.");
        }

        /// <summary>
        /// API Endpoint to get issue specified by issue UId from database table by calling related repository method.
        /// </summary>
        /// <param name="UId"></param>
        [HttpGet("{UId}")]
        public async Task<IActionResult> GetIssueByUId(string UId)
        {
            IssueEntity issueEntity = await _IIssueRepository.GetIssueByUId(UId);

            if (issueEntity == null)
            {
                return BadRequest("Issue Entity not found");
            }

            Issue issue = new Issue()
            {
                UId = issueEntity.UId,
                MemberId = issueEntity.MemberId,
                BookId = issueEntity.BookId,
                IssueDate = issueEntity.IssueDate,
                ReturnDate = issueEntity.ReturnDate,
                isReturned = issueEntity.IsReturned
            };

            return Ok(issue);
        }

        /// <summary>
        /// API Endpoint to get book specified by issue UId from database table by calling related repository method.
        /// </summary>
        /// <param name="UId"></param>
        [HttpGet("{UId}")]
        public async Task<IActionResult> GetIssuedBookByIssueUId(string UId)
        {
            IssueEntity issueEntity = await _IIssueRepository.GetIssueByUId(UId); 

            if (issueEntity == null)
            {
                return BadRequest("Issue Entity not found");
            }

            BookEntity bookEntity = await _IBookRepository.GetBookByUId(issueEntity.BookId);

            if (bookEntity == null)
            {
                return BadRequest("Issued Book not found");
            }

            Book book = new Book()
            {
                UId = bookEntity.UId,
                Title = bookEntity.Title,
                Author = bookEntity.Author,
                PublishedDate = bookEntity.PublishedDate,
                ISBN =  bookEntity.ISBN,
                IsIssued = bookEntity.IsIssued
            };

            return Ok(book);
        }

        /// <summary>
        /// API Endpoint to update issue into database table by calling related repository method.
        /// </summary>
        /// <param name="issue"></param>
        [HttpPut]
        public async Task<IActionResult> UpdateIssue(Issue issue)
        {
            IssueEntity issueEntity = await _IIssueRepository.GetIssueByUId(issue.UId);

            if (issueEntity == null)
            {
                return BadRequest("Issue Entity not found");
            }

            issueEntity.Active = false;
            issueEntity.Archived = true;

            await _IIssueRepository.UpdateIssue(issueEntity);

            issueEntity.Id = Guid.NewGuid().ToString();
            issueEntity.Active = true;
            issueEntity.Archived = false;
            issueEntity.Version = issueEntity.Version + 1;
            issueEntity.UpdatedBy = "User";
            issueEntity.UpdatedOn = DateTime.Now;

            issueEntity.BookId = issue.BookId;
            issueEntity.MemberId = issue.MemberId;
            issueEntity.IssueDate = issue.IssueDate;
            issueEntity.ReturnDate = issue.ReturnDate;
            issueEntity.IsReturned = issue.isReturned;

            bool isUpdated =  await _IIssueRepository.UpdateIssue(issueEntity);

            if (!isUpdated)
            {
                return BadRequest("Not updated.");
            }

            return Ok("Issue updated successfully.");
        }

        /// <summary>
        /// API Endpoint to remove issue specified by UId from database table by calling related repository method.
        /// </summary>
        /// <param name="UId"></param>
        [HttpDelete]
        public async Task<IActionResult> RemoveIssueByUId(string UId)
        {
            IssueEntity toDelete = await _IIssueRepository.GetIssueByUId(UId);

            if (toDelete == null)
            {
                return NotFound("Data not found");
            }

            bool isDeleted = await _IIssueRepository.RemoveIssue(toDelete);

            if (!isDeleted)
            {
                return BadRequest("Data not removed");
            }

            return Ok("Issue Removed Successfully");
        }
    }
}
