using ApiMovies.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMovies.Data;

public class AppDBContext: DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options): base(options)
    {
    }
    //Aqui pasar todas la entidades (modelos)
    public DbSet<Categoria> Categoria { get; set; }
}