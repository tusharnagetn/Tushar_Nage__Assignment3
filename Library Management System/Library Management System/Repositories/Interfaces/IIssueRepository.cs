using Library_Management_System.Entities;

namespace Library_Management_System.Repositories.Interfaces
{
    public interface IIssueRepository
    {
        Task<bool> AddIssue(IssueEntity issue);
        Task<bool> UpdateIssue(IssueEntity issue);
        Task<IssueEntity> GetIssueByUId(string UId);
        Task<bool> RemoveIssue(IssueEntity issue);
    }
}
