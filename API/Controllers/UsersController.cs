using System;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository) : BaseApiController
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers(){
        var users = await userRepository.GetMembersAsync();

        return Ok(users);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username){
        var user = await userRepository.GetMemberAsync(username);

        if(user == null) return NotFound();

        return user;
    }

    // [HttpGet("{username}")]
    // public async Task<ActionResult<MemberDto>> GetUser(string username){
    //     var user = await userRepository.GetUserByUsernameAsync(username);

    //     if(user == null) return NotFound();

    //     return new MemberDto{
    //         Id = user.Id,
    //         KnownAs = user.KnownAs,
    //     }
    // }

    // [HttpPost]
    // public ActionResult<IEnumerable<AppUser>> AddUser(AppUser user){
    //     context.Users.Add(user);

    //     return Ok;
    // }

}
