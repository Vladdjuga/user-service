using Application.DTOs.User;
using Application.Services;
using Domain.Entities;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Test
{
    [Authorize]
    [Route("test/[controller]")]
    public class UserController(UserService userService) : Controller
    {
        [HttpGet("users")]
        public async Task<IEnumerable<ReadUserDto>> Index()
        {
            return await userService.GetAllAsync();
        }
        [HttpGet("user/{id}")]
        public async Task<ReadUserDto?> GetUser(Guid id)
        {
            if (id == Guid.Empty)
            {
                return null;
            }
            return await userService.GetByIdAsync(id);
        }
        [HttpPost("user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            try{
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            var readDto=await userService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetUser), new { id = readDto.Id }, readDto);
        }
    }
}
