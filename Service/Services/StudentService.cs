using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.Interfaces;
using Service.DTOs.Admin.Students;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IGroupStudentRepository _groupStudentRepo;
        private readonly IMapper _mapper;
        private readonly IGroupRepository _groupRepo;

        public StudentService(IStudentRepository studentRepo,
                              IGroupStudentRepository groupStudentRepo,
                              IMapper mapper,
                              IGroupRepository groupRepo)
        {
            _studentRepo = studentRepo;
            _groupStudentRepo = groupStudentRepo;
            _mapper = mapper;
            _groupRepo = groupRepo;
        }
        public async Task<IEnumerable<StudentDto>> GetAllWithInclude()
        {
            var students = await _studentRepo.FindAllWithIncludes()
                 .Include(m => m.GroupStudents)
                 .ThenInclude(m => m.Group)
                 .ToListAsync();
            var mappedStudents = _mapper.Map<List<StudentDto>>(students);
            return mappedStudents;
        }

        public async Task CreateAsync(StudentCreateDto model)
        {
            var data = _mapper.Map<Student>(model);
            await _studentRepo.CreateAsync(data);

            foreach (var id in model.GroupIds)
            {
                var group = await _groupRepo.GetById(id);
                if (group == null) throw new KeyNotFoundException($"Group with ID {id} not found");

                var studentCount = await _groupRepo.GetStudentCountInGroup(id);
                if (studentCount >= group.Capacity)
                {
                    throw new InvalidOperationException($"Group with ID {id} is full.");
                }

                await _groupStudentRepo.CreateAsync(new GroupStudents
                {
                    StudentId = data.Id,
                    GroupId = id
                });
            }
        }

        public async Task EditAsync(int id, StudentEditDto model)
        {
            var existingStudent = await _studentRepo.GetById(id);
            if (existingStudent == null) throw new KeyNotFoundException("Student not found");

            _mapper.Map(model, existingStudent);
            await _studentRepo.EditAsync(existingStudent);

            var existingGroupStudents = await _groupStudentRepo.FindAll(gs => gs.StudentId == id);
            foreach (var groupStudent in existingGroupStudents)
            {
                await _groupStudentRepo.DeleteAsync(groupStudent);
            }

            foreach (var groupId in model.GroupIds)
            {
                var group = await _groupRepo.GetById(groupId);
                if (group == null) throw new KeyNotFoundException($"Group with ID {groupId} not found");

                var studentCount = await _groupRepo.GetStudentCountInGroup(groupId);
                if (studentCount >= group.Capacity)
                {
                    throw new InvalidOperationException($"Group with ID {groupId} is full.");
                }

                var groupStudent = new GroupStudents
                {
                    StudentId = id,
                    GroupId = groupId
                };
                await _groupStudentRepo.CreateAsync(groupStudent);
            }
        }

        public async Task<StudentDto> GetByIdWithInclude(int id)
        {
            var student = await _studentRepo.FindBy(s => s.Id == id)
                                            .Include(s => s.GroupStudents)
                                            .ThenInclude(gs => gs.Group)
                                            .FirstOrDefaultAsync();
            if (student == null) throw new KeyNotFoundException("Student not found");
            return _mapper.Map<StudentDto>(student);
        }
        public async Task DeleteAsync(int id)
        {
            var student = await _studentRepo.GetById(id);
            if (student == null) throw new KeyNotFoundException("Student not found");

            var groupStudents = await _groupStudentRepo.FindAll(gs => gs.StudentId == id);
            foreach (var groupStudent in groupStudents)
            {
                await _groupStudentRepo.DeleteAsync(groupStudent);
            }

            await _studentRepo.DeleteAsync(student);
        }
    }
}
