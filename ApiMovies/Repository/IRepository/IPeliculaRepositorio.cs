using ApiMovies.Models;

namespace ApiMovies.Repository.IRepository;

public interface IPeliculaRepositorio
{
    ICollection<Pelicula> GetPeliculas();
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