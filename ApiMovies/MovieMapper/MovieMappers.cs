using ApiMovies.Models;
using ApiMovies.Models.Dtos;
using AutoMapper;

namespace ApiMovies.MovieMapper;

public class MovieMappers : Profile
{
    public MovieMappers()
    {
        CreateMap<Categoria, CategoriaDto>().ReverseMap();
        CreateMap<Categoria, CrearCategoriaDto>().ReverseMap();
        CreateMap<Pelicula, PeliculaDto>().ReverseMap();
        CreateMap<Pelicula, CrearPeliculaDto>().ReverseMap();
    }
}