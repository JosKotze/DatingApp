using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto){

        if(await UserExists(registerDto.name))
        {
            return BadRequest("A user with that name is already in the system");
        }

        using var hmac = new HMACSHA512(); // using 'using' to dispose it after use

        var user = new AppUser{
            Name = registerDto.name.ToLower(),
            Surname = registerDto.surname.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.password)),
            PasswordSalt = hmac.Key
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new UserDto{
            Name = user.Name,
            Token = tokenService.CreateToken(user)
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){

        var user = await context.Users.FirstOrDefaultAsync(x => x.Name == loginDto.name.ToLower());

        if(user == null) return Unauthorized("Invalid user");

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
        }

        return new UserDto{
            Name = user.Name,
            Token = tokenService.CreateToken(user)
        };
    }

    private async Task<bool> UserExists(string name){
        return await context.Users.AnyAsync(x => x.Name.ToLower() == name.ToLower()); 
    }
}
