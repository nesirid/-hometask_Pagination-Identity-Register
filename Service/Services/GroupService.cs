using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.Interfaces;
using Service.DTOs.Admin.Groups;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupTeacherRepository _groupTeacherRepository;
        private readonly IMapper _mapper;

        public GroupService(IGroupRepository groupRepository,
                            IMapper mapper,
                            IGroupTeacherRepository groupTeacherRepository)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
            _groupTeacherRepository = groupTeacherRepository;
        }

        public async Task CreateAsync(GroupCreateDto model)
        {
            var data = _mapper.Map<Group>(model);

            await _groupRepository.CreateAsync(data);
            await _groupTeacherRepository.CreateAsync(new GroupTeachers { GroupId = data.Id, TeacherId = model.teacherId });
        }

        public async Task DeleteAsync(int? id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            var group = await _groupRepository.GetById(id.Value);
            if (group == null) throw new KeyNotFoundException("Group not found");

            await _groupRepository.DeleteAsync(group);
        }

        public async Task EditAsync(int? id, GroupEditDto model)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            var existingGroup = await _groupRepository.GetById(id.Value);
            if (existingGroup == null) throw new KeyNotFoundException("Group not found");

            _mapper.Map(model, existingGroup);
            await _groupRepository.EditAsync(existingGroup);

            var existingGroupTeachers = await _groupTeacherRepository.FindAll(gt => gt.GroupId == id.Value);
            foreach (var groupTeacher in existingGroupTeachers)
            {
                await _groupTeacherRepository.DeleteAsync(groupTeacher);
            }

            foreach (var teacherId in model.TeacherIds)
            {
                var groupTeacher = new GroupTeachers
                {
                    GroupId = id.Value,
                    TeacherId = teacherId
                };
                await _groupTeacherRepository.CreateAsync(groupTeacher);
            }
        }

        public async Task<IEnumerable<GroupDto>> GetAllAsync()
        {
            var datas = await _groupRepository.FindAllWithIncludes()
                                              .Include(m => m.Education)
                                              .Include(m => m.Room)
                                              .Include(m => m.GroupTeachers)
                                              .ThenInclude(m => m.Teacher)
                                              .Include(m => m.GroupStudents)
                                              .ThenInclude(m => m.Student)
                                              .ToListAsync();

            return _mapper.Map<IEnumerable<GroupDto>>(datas);
        }

        public async Task<GroupDto> GetByIdAsync(int id)
        {
            var data = await _groupRepository.FindBy(m => m.Id == id)
                                             .Include(m => m.Education)
                                             .Include(m => m.Room)
                                             .Include(m => m.GroupTeachers)
                                             .ThenInclude(m => m.Teacher)
                                             .Include(m => m.GroupStudents)
                                             .ThenInclude(m => m.Student)
                                             .FirstOrDefaultAsync();

            if (data == null) throw new KeyNotFoundException("Group not found");

            return _mapper.Map<GroupDto>(data);
        }

    }
}
