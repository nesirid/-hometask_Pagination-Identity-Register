using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Admin.Teachers;
using Service.Services.Interfaces;

namespace App.Controllers.Admin
{
 
    public class TeacherController : BaseController
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeacherCreateDto request)
        {
            await _teacherService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), new { response = "Data successfully created" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _teacherService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await _teacherService.GetByIdAsync(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] TeacherEditDto request)
        {
            await _teacherService.EditAsync(id, request);
            return Ok(new { response = "Data successfully updated" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _teacherService.DeleteAsync(id);
            return Ok(new { response = "Data successfully deleted" });
        }
    }
}
