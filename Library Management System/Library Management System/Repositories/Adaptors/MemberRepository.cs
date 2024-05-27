using Library_Management_System.Entities;
using Library_Management_System.Models;
using Library_Management_System.Repositories.Interfaces;
using Microsoft.Azure.Cosmos;

namespace Library_Management_System.Repositories.Adaptors
{
    public class MemberRepository : IMemberRepository
    {
        private readonly Container _container;

        public MemberRepository(DBConfig dBConfig)
        {
            _container = dBConfig.GetContainer();
        }

        public async Task<bool> AddMember(MemberEntity memberEntity)
        {
            var rs = await _container.CreateItemAsync(memberEntity, new PartitionKey(memberEntity.DocumentType));
            return rs != null;
        }

        public async Task<List<MemberEntity>> GetAllMember()
        {
            var toReturn = _container.GetItemLinqQueryable<MemberEntity>(true).Where(x => x.Active == true && x.Archived == false && x.DocumentType == "Member").ToList();
            return toReturn;
        }

        public async Task<MemberEntity> GetMemberByUId(string UId)
        {
            var toReturn = _container.GetItemLinqQueryable<MemberEntity>(true).Where(x => x.UId == UId && x.Active && x.Archived == false && x.DocumentType == "Member").FirstOrDefault();
            return toReturn;
        }

        public async Task<bool> RemoveMember(MemberEntity memberEntity)
        {
            var rs = await _container.DeleteItemAsync<MemberEntity>(memberEntity.UId, new PartitionKey(memberEntity.DocumentType));
            return rs != null;
        }

        public async Task<bool> UpdateMember(MemberEntity memberEntity)
        {
            var rs = await _container.ReplaceItemAsync(memberEntity, memberEntity.UId);
            return rs != null;
        }
    }
}
