using AuthMicroservice.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthMicroservice.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
}