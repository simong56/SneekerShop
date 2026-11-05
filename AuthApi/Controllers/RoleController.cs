using AuthApi.Models;
using AuthApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Controllers;

[Route("roles")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly AuthUserContext _context;

    public RoleController(AuthUserContext context)
    {
        _context = context;
    }

    [HttpPost("createRole")]
    public async Task<IActionResult> NewRole([FromBody] NewRoleDto newRoleDto)
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = newRoleDto.Name
        };

        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();
        return StatusCode(201, newRoleDto);
    }

    [HttpGet("allRoles")]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _context.Roles.ToListAsync();

        return StatusCode(200, roles);
    }

    [HttpGet("role/{id}")]
    public async Task<IActionResult> GetRole([FromRoute] Guid id)
    {
        var role = await _context.Roles.SingleOrDefaultAsync(r => r.Id == id);

        if (role == null) return StatusCode(404, "Role not found");

        return StatusCode(200, role);
    }

    [HttpPost("updateRole")]
    public async Task<IActionResult> UpdateRole([FromQuery] Guid id, [FromBody] UpdateRoleDto updateRoleDto)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);

        if (role == null) return StatusCode(404, "Role not found");

        role.Name = updateRoleDto.Name;

        _context.Roles.Update(role);
        await _context.SaveChangesAsync();
        return StatusCode(200, role);
    }

    [HttpDelete("deleteRole/{id}")]
    public async Task<IActionResult> DeleteRole([FromRoute] Guid id)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);

        if (role == null) return StatusCode(404, "Role not found");

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();

        return StatusCode(200, "Role successfully deleted");
    }
}
