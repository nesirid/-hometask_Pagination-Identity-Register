using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        public GroupRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<int> GetStudentCountInGroup(int groupId)
        {
            return await _context.GroupStudents.CountAsync(gs => gs.GroupId == groupId);
        }
    }
}
