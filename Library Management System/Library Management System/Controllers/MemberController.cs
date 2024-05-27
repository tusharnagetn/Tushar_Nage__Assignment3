using Library_Management_System.Entities;
using Library_Management_System.Models;
using Library_Management_System.Repositories.Adaptors;
using Library_Management_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MemberController : Controller
    {
        private readonly IMemberRepository _IMemberRepository;

        public MemberController(IMemberRepository iMemberRepository) 
        { 
            _IMemberRepository = iMemberRepository;
        }

        /// <summary>
        /// API Endpoint to add Member into database table by calling related repository method.
        /// </summary>
        /// <param name="member"></param>
        [HttpPost]
        public async Task<IActionResult> AddMember(Member member)
        {
            string uniqueId = Guid.NewGuid().ToString();

            MemberEntity ToAdd = new MemberEntity()
            {
                UId = uniqueId,
                Id = uniqueId,
                DocumentType = "Member",
                Version = 1,
                CreatedBy = "Admin",
                CreatedOn = DateTime.Now,
                Active = true,
                Archived = false,

                Name = member.Name,
                DateOfBirth = member.DateOfBirth,
                Email = member.Email
            };

            bool result = await _IMemberRepository.AddMember(ToAdd);

            if (!result)
            {
                return StatusCode(500, "Failed to add the member.");
            }

            return Ok("Member added successfully.");
        }

        /// <summary>
        /// API Endpoint to get Member by specified Member UId from database table by calling related repository method.
        /// </summary>
        /// <param name="UId"></param>
        [HttpGet("{UId}")]
        public async Task<IActionResult> GetMemberByUId(string UId)
        {
            var memberEntity = await _IMemberRepository.GetMemberByUId(UId);

            if (memberEntity == null)
            {
                return NotFound("Data not found");
            }

            Member toReturn = new Member()
            {
                UId = memberEntity.UId,
                Name = memberEntity.Name,
                DateOfBirth = memberEntity.DateOfBirth,
                Email = memberEntity.Email,
            };

            return Ok(toReturn);
        }

        /// <summary>
        /// API Endpoint to get all members from database table by calling related repository method.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllMember()
        {
            var GetMembers = await _IMemberRepository.GetAllMember();

            List<Member> members = new List<Member>();

            foreach (var member in GetMembers)
            {
                Member toAdd = new Member()
                {
                    UId = member.UId,
                    Name = member.Name,
                    DateOfBirth = member.DateOfBirth,
                    Email = member.Email,
                };

                members.Add(toAdd);
            }
            return Ok(members);
        }

        /// <summary>
        /// API Endpoint to update member into database table by calling related repository method.
        /// </summary>
        /// <param name="member"></param>
        [HttpPut]
        public async Task<IActionResult> UpdateMember(Member member)
        {
            MemberEntity memberEntity = await _IMemberRepository.GetMemberByUId(member.UId);

            if (memberEntity == null)
            {
                return NotFound("Data not found");
            }

            memberEntity.Active = false;
            memberEntity.Archived = true;
            bool isExecuted = await _IMemberRepository.UpdateMember(memberEntity);

            if (!isExecuted)
            {
                return StatusCode(500, "Failed to update the book.");
            }

            memberEntity.Id = Guid.NewGuid().ToString();
            memberEntity.Version = memberEntity.Version + 1;
            memberEntity.UpdatedBy = "Users";
            memberEntity.UpdatedOn = DateTime.Now;
            memberEntity.Active = true;
            memberEntity.Archived = false;
            memberEntity.Name = member.Name;
            memberEntity.DateOfBirth = member.DateOfBirth;
            memberEntity.Email = member.Email;

            bool result = await _IMemberRepository.UpdateMember(memberEntity);

            if (!result)
            {
                return StatusCode(500, "Failed to update the book.");
            }

            return Ok("Member updated successfully.");
        }

        /// <summary>
        /// API Endpoint to remove member specified by UId from database table by calling related repository method.
        /// </summary>
        /// <param name="UId"></param>
        [HttpDelete]
        public async Task<IActionResult> RemoveMemberByUId(string UId)
        {
            MemberEntity toDelete = await _IMemberRepository.GetMemberByUId(UId);

            if (toDelete == null)
            {
                return NotFound("Data not found");
            }

            bool isDeleted = await _IMemberRepository.RemoveMember(toDelete);

            if (!isDeleted)
            {
                return BadRequest("Data not removed");
            }

            return Ok("Member Removed Successfully");
        }
    }
}
