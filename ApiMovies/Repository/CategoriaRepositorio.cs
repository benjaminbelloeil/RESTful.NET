using ApiMovies.Data;
using ApiMovies.Models;
using ApiMovies.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiMovies.Repository;

public class CategoriaRepositorio: ICategoriaRepositorio
{
    private readonly AppDBContext _bd;

        public CategoriaRepositorio(AppDBContext bd)
        {
            _bd = bd;
        }

        public ICollection<Categoria> GetCategorias()
        {
            return _bd.Categoria.OrderBy(c => c.Nombre).ToList();
        }

        public Categoria GetCategoria(int CategoriaId)
        {
            return _bd.Categoria.FirstOrDefault(c => c.Id == CategoriaId);
        }

        public bool ExsiteCategoria(int id)
        {
            return _bd.Categoria.Any(c => c.Id == id);
        }

        public bool ExsiteCategoria(string nombre)
        {
            bool valor = _bd.Categoria.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        public bool CrearCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _bd.Categoria.Add(categoria);
            return Guardar();
        }

        public bool ActualizarCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _bd.Categoria.Update(categoria);
            return Guardar();
        }

        public bool BorrarCategoria(Categoria categoria)
        {
            _bd.Categoria.Remove(categoria);
            return Guardar();
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }
}