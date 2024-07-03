using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.DTOs.Admin.Rooms;
using Service.DTOs.Admin.Teachers;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepo;
        private readonly IMapper _mapper;

        public TeacherService(ITeacherRepository teacherRepository, IMapper mapper)
        {
            _teacherRepo = teacherRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(TeacherCreateDto model)
        {
            await _teacherRepo.CreateAsync(_mapper.Map<Teacher>(model));
        }

        public async Task<IEnumerable<TeacherDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<TeacherDto>>(await _teacherRepo.GetAllAsync());
        }

        public async Task<TeacherDto> GetByIdAsync(int id)
        {
            var teacher = await _teacherRepo.GetById(id);
            if (teacher == null) throw new KeyNotFoundException("Teacher not found");

            return _mapper.Map<TeacherDto>(teacher);
        }

        public async Task EditAsync(int id, TeacherEditDto model)
        {
            var existingTeacher = await _teacherRepo.GetById(id);
            if (existingTeacher == null) throw new KeyNotFoundException("Teacher not found");

            _mapper.Map(model, existingTeacher);
            await _teacherRepo.EditAsync(existingTeacher);
        }

        public async Task DeleteAsync(int id)
        {
            var teacher = await _teacherRepo.GetById(id);
            if (teacher == null) throw new KeyNotFoundException("Teacher not found");

            await _teacherRepo.DeleteAsync(teacher);
        }
    }
}