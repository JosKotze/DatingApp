using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class LoginDto
{
    [Required]
    public required string username { get; set; }
    public required string password { get; set; }
}
