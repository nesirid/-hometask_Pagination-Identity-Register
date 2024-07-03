using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Admin.Students;
using Service.Services.Interfaces;

namespace App.Controllers.Admin
{
    public class StudentController : BaseController
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentCreateDto request)
        {
            await _studentService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), new { Response = "Succesfull" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var mappedStudents = await _studentService.GetAllWithInclude();
            return Ok(mappedStudents);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] StudentEditDto request)
        {
            await _studentService.EditAsync(id, request);
            return NoContent();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _studentService.GetByIdWithInclude(id);
            return Ok(student);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _studentService.DeleteAsync(id);
            return NoContent();
        }
    }
}
