using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Admin.Groups
{
    public record GroupEditDto(string name, int capacity, int educationId, int roomId, int teacherId)
    {
        public List<int> TeacherIds { get; set; }
    }
}
