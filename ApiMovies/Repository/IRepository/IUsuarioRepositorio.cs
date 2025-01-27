using ApiMovies.Models;
using ApiMovies.Models.Dtos;

namespace ApiMovies.Repository.IRepository;

public interface IUsuarioRepositorio
{
    ICollection<Usuario> GetUsuarios();
    Usuario GetUsuario(int usuarioId);
    bool IsUniqueUser(string usuario);
    Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);
    Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto);


}