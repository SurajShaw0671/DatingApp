using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")] // account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto register)
    {
        if(await UserExists(register.Username)) 
            return BadRequest("Username already exists!");

        using var hmac = new HMACSHA512();
        var user = new AppUser{
            UserName = register.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.Password)),
            PasswordSalt = hmac.Key
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }
   [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user =await context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginDto.Username.ToLower());
        
        if(user==null) 
            return Unauthorized("Invalid Username!");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computeHash  = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for(int i =0 ; i< computeHash.Length;i++)
        {
            if(computeHash[i]!=user.PasswordHash[i])
            {
                return Unauthorized("Invalid Password!");
            }
        }
        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }
 
    private async Task<bool> UserExists(string Username)
    {
        return await context.Users.AnyAsync(x=>x.UserName.ToLower()==Username.ToLower());
    }
} 
