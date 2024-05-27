using Library_Management_System.Entities;

namespace Library_Management_System.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        Task<bool> AddMember(MemberEntity memberEntity);
        Task<MemberEntity> GetMemberByUId(string UId);
        Task<List<MemberEntity>> GetAllMember();
        Task<bool> UpdateMember(MemberEntity memberEntity);
        Task<bool> RemoveMember(MemberEntity memberEntity);
    }
}
