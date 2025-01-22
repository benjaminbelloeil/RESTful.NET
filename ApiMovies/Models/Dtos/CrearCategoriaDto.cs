using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Models.Dtos;

public class CrearCategoriaDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(100, ErrorMessage = "La longitud del campo es de 100 caracteres")]
    public string Nombre { get; set; }
}