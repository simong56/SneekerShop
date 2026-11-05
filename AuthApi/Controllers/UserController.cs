using AuthApi.Models;
using AuthApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Controllers;

[Route("users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly AuthUserContext _context;
    
    public UserController(AuthUserContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> NewUser([FromBody]NewUserDto newUserDto)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = newUserDto.Name,
            Email = newUserDto.Email,
            Password = newUserDto.Password
        };
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return StatusCode(201, newUserDto);
    }

    [HttpGet("allUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _context.Users.ToListAsync();
        
        return StatusCode(200, users);
    }

    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetUser([FromRoute] Guid id)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);

        if (user == null) return StatusCode(404,  "User not found");
        
        return StatusCode(200, user);
    }

    [HttpPost("upateUser")]
    public async Task<IActionResult> UpdateUser([FromQuery] Guid id, [FromBody] UpdateUserDto updateUserDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        
        if (user == null) return StatusCode(404,  "User not found");
        
        else if (user != null)
        {
            user.Name = updateUserDto.Name;
            user.Email = updateUserDto.Email;
            user.Password = updateUserDto.Password;

            _context.Users.Update(user);
            return StatusCode(200, user);
        }
        
        return StatusCode(400,  "User not found");
    }

    [HttpDelete("deleteUser/{id}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        
        if (user == null) return StatusCode(404,  "User not found");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        
        return StatusCode(200, "User successfully deleted");
    }
}