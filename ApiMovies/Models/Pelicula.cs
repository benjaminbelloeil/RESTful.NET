using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMovies.Models;

public class Pelicula
{
    [Key]
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string Duracion { get; set; }
    public string? RutaImagen { get; set; }
    public string? RutaLocalImagen { get; set; }
    public enum TipoClasificacion { Siete, Trece, Dieciseis, Diechiocho }
    public TipoClasificacion Clasificacion { get; set; }
    public DateTime? FechaCreacion { get; set; }
    
    // Relacion con Categoria
    public int categoriaId { get; set; }
    [ForeignKey("categoriaId")] 
    public Categoria Categoria { get; set; }
    
}