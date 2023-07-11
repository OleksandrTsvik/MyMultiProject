using System.Security.Claims;
using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly DataContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly TokenService _tokenService;

    public AccountController(DataContext context, UserManager<AppUser> userManager,
        TokenService tokenService)
    {
        _context = context;
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        AppUser user = await _userManager.Users
            .Include(x => x.Photos)
            .FirstOrDefaultAsync(x => x.Email == loginDto.Email);

        if (user == null)
        {
            return Unauthorized();
        }

        bool result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!result)
        {
            return Unauthorized();
        }

        return await CreateUserDto(user);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Rregister(RegisterDto registerDto)
    {
        if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.UserName))
        {
            ModelState.AddModelError("userName", "Username is already taken");

            return ValidationProblem();
        }

        if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
        {
            ModelState.AddModelError("email", "Email is already taken");

            return ValidationProblem();
        }

        AppUser user = new AppUser
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            RegistrationDate = DateTime.Now
        };

        IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return await CreateUserDto(user);
    }

    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        AppUser user = await _userManager.Users
            .Include(x => x.Photos)
            .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

        return await CreateUserDto(user);
    }

    private async Task<UserDto> CreateUserDto(AppUser user)
    {
        return new UserDto
        {
            UserName = user.UserName,
            Email = user.Email,
            RegistrationDate = user.RegistrationDate,
            Image = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            Token = _tokenService.CreateToken(user),
            CountNotCompletedDuties = await _context.Duties.CountAsync(x => x.AppUser.Id == user.Id && !x.IsCompleted),
            CountCompletedDuties = await _context.Duties.CountAsync(x => x.AppUser.Id == user.Id && x.IsCompleted)
        };
    }
}