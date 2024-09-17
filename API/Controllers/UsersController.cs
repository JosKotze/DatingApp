using System;
using System.Security.Claims;
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
public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
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

    //an update is an HTTP PUT
    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto){
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // we specified username as our token service

        if(username == null) return BadRequest("No username found in token");

        var user = await userRepository.GetUserByUsernameAsync(username);

        if(user == null) return BadRequest("Could not find user");

        mapper.Map(memberUpdateDto, user);

        if(await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update user");
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
