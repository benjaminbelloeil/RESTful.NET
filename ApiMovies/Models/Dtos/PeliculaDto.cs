namespace ApiMovies.Models.Dtos;

public class PeliculaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string Duracion { get; set; }
    public string? RutaImagen { get; set; }
    public string? RutaLocalImagen { get; set; }
    public enum TipoClasificacion { Siete, Trece, Dieciseis, Diechiocho }
    public TipoClasificacion Clasificacion { get; set; }
    public DateTime FechaCreacion { get; set; }
    
    public int categoriaId { get; set; }
}

