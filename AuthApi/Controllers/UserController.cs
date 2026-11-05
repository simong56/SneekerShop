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
}