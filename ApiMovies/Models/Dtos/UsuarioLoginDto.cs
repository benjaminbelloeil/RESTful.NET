using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Models.Dtos;

public class UsuarioLoginDto
{
    [Required(ErrorMessage = "El usuario es requerido")]
    public string NombreUsuario { get; set; }
    [Required(ErrorMessage = "El password es requerido")]
    public string Password { get; set; }
}