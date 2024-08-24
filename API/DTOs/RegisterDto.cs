using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    public required string name { get; set; }
    [Required]
    public required string surname { get; set; }
    public required string password { get; set; }
}
