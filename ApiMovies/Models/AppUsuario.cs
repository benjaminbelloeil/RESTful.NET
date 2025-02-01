using Microsoft.AspNetCore.Identity;

namespace ApiMovies.Models;

public class AppUsuario : IdentityUser
{
    public string Nombre { get; set; }
}