using ApiMovies.Models;

namespace ApiMovies.Repository.IRepository;

public interface IPeliculaRepositorio
{
    //V1
    // ICollection<Pelicula> GetPeliculas();
    
    //V2
    ICollection<Pelicula> GetPeliculas(int pageNumber, int pageSize);
    int GetTotalPeliculas();
    ICollection<Pelicula> GetPeliculasEnCategoria(int catId);
    IEnumerable<Pelicula> BuscarPelicula(string nombre);
    Pelicula GetPelicula(int peliculaId);
    bool ExsitePelicula(int id);
    bool ExsitePelicula(string nombre);
    bool CrearPelicula(Pelicula pelicula);
    bool ActualizarPelicula(Pelicula pelicula);
    bool BorrarPelicula(Pelicula pelicula);
    bool Guardar();







}