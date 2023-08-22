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

        await SetRefreshToken(user);

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
            RegistrationDate = DateTime.UtcNow
        };

        IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await SetRefreshToken(user);

        return await CreateUserDto(user);
    }

    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        AppUser user = await _userManager.Users
            .Include(x => x.Photos)
            .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

        await SetRefreshToken(user);

        return await CreateUserDto(user);
    }

    [HttpPost("refreshToken")]
    public async Task<ActionResult<UserDto>> RefreshToken()
    {
        var refreshToken = Request.Cookies["refresh_token"];

        AppUser user = await _userManager.Users
            .Include(x => x.RefreshTokens)
            .Include(x => x.Photos)
            .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

        if (user == null)
        {
            return Unauthorized();
        }

        RefreshToken oldRefreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken);

        if (oldRefreshToken != null && !oldRefreshToken.IsActive)
        {
            return Unauthorized();
        }

        return await CreateUserDto(user);
    }

    [NonAction]
    private async Task SetRefreshToken(AppUser user)
    {
        RefreshToken refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshTokens.Add(refreshToken);
        await _userManager.UpdateAsync(user);

        CookieOptions cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = refreshToken.Expires
        };

        Response.Cookies.Append("refresh_token", refreshToken.Token, cookieOptions);
    }

    [NonAction]
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
