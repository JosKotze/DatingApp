using System;

namespace API.DTOs;

public class UserDto
{
    public required string Name { get; set; }
    public required string Token { get; set; }
}
