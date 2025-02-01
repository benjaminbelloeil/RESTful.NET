using ApiMovies.Data;
using ApiMovies.Models;
using ApiMovies.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiMovies.Repository;

public class PeliculaRepositorio: IPeliculaRepositorio
{
    private readonly AppDBContext _bd;

        public PeliculaRepositorio(AppDBContext bd)
        {
            _bd = bd;
        }

        // V1
        // public ICollection<Pelicula> GetPeliculas()
        // {
        //     return _bd.Pelicula.OrderBy(c => c.Nombre).ToList();
        // }
        
        // V2
        public ICollection<Pelicula> GetPeliculas(int pageNumber, int pageSize)
        {
            return _bd.Pelicula.OrderBy(c => c.Nombre)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetTotalPeliculas()
        {
            return _bd.Pelicula.Count();
        }

        public ICollection<Pelicula> GetPeliculasEnCategoria(int catId)
        {
            return _bd.Pelicula.Include(c => c.Categoria).Where(c => c.categoriaId == catId).ToList();
        }

        public IEnumerable<Pelicula> BuscarPelicula(string nombre)
        {
            IQueryable<Pelicula> query = _bd.Pelicula;
            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(p => p.Nombre.Contains(nombre) || p.Descripcion.Contains(nombre));
            }
            return query.ToList();
        }

        public Pelicula GetPelicula(int peliculaId)
        {
            return _bd.Pelicula.FirstOrDefault(c => c.Id == peliculaId);
        }

        public bool ExsitePelicula(int id)
        {
            return _bd.Pelicula.Any(c => c.Id == id);
        }

        public bool ExsitePelicula(string nombre)
        {
            bool valor = _bd.Pelicula.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        public bool CrearPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            _bd.Pelicula.Add(pelicula);
            return Guardar();
        }

        public bool ActualizarPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            var peliculaExistente = _bd.Pelicula.Find(pelicula.Id);
            if (peliculaExistente != null)
            {
                _bd.Entry(peliculaExistente).CurrentValues.SetValues(pelicula);
            }
            else
            {
                _bd.Pelicula.Update(pelicula);
            }
            _bd.Pelicula.Update(pelicula);
            return Guardar();
        }

        public bool BorrarPelicula(Pelicula pelicula)
        {
            _bd.Pelicula.Remove(pelicula);
            return Guardar();
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }
}