using ApiMovies.Models;

namespace ApiMovies.Repository.IRepository;

public interface ICategoriaRepositorio
{
    ICollection<Categoria> GetCategorias();
    Categoria GetCategoria(int categoriaId);
    bool ExsiteCategoria(int id);
    bool ExsiteCategoria(string nombre);
    bool CrearCategoria(Categoria categoria);
    bool ActualizarCategoria(Categoria categoria);
    bool BorrarCategoria(Categoria categoria);
    bool Guardar();







}