using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Admin.Rooms;
using Service.Services.Interfaces;

namespace App.Controllers.Admin
{

    public class RoomController : BaseController
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]RoomCreateDto request)
        {
            await _roomService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), new { Response = "succesfull" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _roomService.GetAllAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var room = await _roomService.GetByIdAsync(id);
            return Ok(room);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] RoomEditDto request)
        {
            await _roomService.UpdateAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _roomService.DeleteAsync(id);
            return NoContent();
        }
    }
}
