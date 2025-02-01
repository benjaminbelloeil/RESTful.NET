using ApiMovies.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiMovies.Data;

public class AppDBContext : IdentityDbContext<AppUsuario>
{
    public AppDBContext(DbContextOptions<AppDBContext> options): base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
    
    //Aqui pasar todas la entidades (modelos)
    public DbSet<Categoria> Categoria { get; set; }
    public DbSet<Pelicula> Pelicula { get; set; }
    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<AppUsuario> AppUsuario { get; set; }
    
}