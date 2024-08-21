using System;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options) // need to register it in program class.
{
    public DbSet<AppUser> Users { get; set; } // Going to be name of table
}
