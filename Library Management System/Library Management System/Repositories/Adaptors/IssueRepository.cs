using Library_Management_System.Entities;
using Library_Management_System.Repositories.Interfaces;
using Microsoft.Azure.Cosmos;

namespace Library_Management_System.Repositories.Adaptors
{
    public class IssueRepository : IIssueRepository
    {
        private readonly Container _container;

        public IssueRepository(DBConfig dBConfig)
        {
            _container = dBConfig.GetContainer();
        }

        public async Task<bool> AddIssue(IssueEntity issueEntity)
        {
            var rs = await _container.CreateItemAsync(issueEntity, new PartitionKey(issueEntity.DocumentType));
            return rs != null;
        }

        public async Task<bool> UpdateIssue(IssueEntity issueEntity)
        {
            var rs = await _container.ReplaceItemAsync(issueEntity, issueEntity.UId);
            return rs != null;
        }

        public async Task<IssueEntity> GetIssueByUId(string UId)
        {
            var toReturn = _container.GetItemLinqQueryable<IssueEntity>(true).Where(x => x.UId == UId && x.Active && x.Archived == false && x.DocumentType == "Issue").FirstOrDefault();
            return toReturn;
        }

        public async Task<bool> RemoveIssue(IssueEntity issue)
        {
            var rs = await _container.DeleteItemAsync<IssueEntity>(issue.UId, new PartitionKey(issue.DocumentType));
            return rs != null;
        }
    }
}
