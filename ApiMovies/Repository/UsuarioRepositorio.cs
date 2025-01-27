using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ApiMovies.Data;
using ApiMovies.Models;
using ApiMovies.Models.Dtos;
using ApiMovies.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;

namespace ApiMovies.Repository;

public class UsuarioRepositorio : IUsuarioRepositorio
{
    private readonly AppDBContext _bd;
    private string claveSecreta;

    public UsuarioRepositorio(AppDBContext bd, IConfiguration config)
    {
        _bd = bd;
        claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
    }

    public ICollection<Usuario> GetUsuarios()
    {
        return _bd.Usuario.OrderBy(c => c.NombreUsuario).ToList();
    }

    public Usuario GetUsuario(int usuarioId)
    {
        return _bd.Usuario.FirstOrDefault(u => u.Id == usuarioId);
    }

    public bool IsUniqueUser(string usuario)
    {
        var usuarioBd = _bd.Usuario.FirstOrDefault(u => u.NombreUsuario == usuario);
        if (usuarioBd == null)
        {
            return true;
        }
        return false;
    }

    public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
    {
        {
            var passwordEncriptado = obtenermd5(usuarioLoginDto.Password);
            var usuario = _bd.Usuario.FirstOrDefault(u => u.NombreUsuario.ToLower() == usuarioLoginDto.NombreUsuario.ToLower() && u.Password == passwordEncriptado);
        
            // Validamos si el usuario no existe con la combinacion de usuario y contraseña
            if (usuario == null)
            {
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
            
            }
            // aqui existe el usuario entonces podemos procesar el login
            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario.ToString()),
                    new Claim(ClaimTypes.Role, usuario.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = manejadorToken.CreateToken(tokenDescriptor);

            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto = new UsuarioLoginRespuestaDto()
            {
                Token = manejadorToken.WriteToken(token),
                Usuario = usuario
            };
            return usuarioLoginRespuestaDto;
        }
    }
    
    public async Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto)
    {
        var passwrodEncriptado = obtenermd5(UsuarioRegistroDto.Password);

        Usuario usuario = new Usuario()
        {
            NombreUsuario = usuarioRegistroDto.NombreUsuario,
            Password = passwrodEncriptado,
            Nombre = usuarioRegistroDto.Nombre,
            Role = usuarioRegistroDto.Role
        };
        
        _bd.Usuario.Add(usuario);
        await _bd.SaveChangesAsync();
        usuario.Password = passwrodEncriptado;
        return usuario;
    }
    // Metodo para encriptar contraseña con MD5
    public static string obtenermd5(string valor)
    {
        MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
        data = x.ComputeHash(data);
        string resp = "";
        for (int i = 0; i < data.Length; i++)
            resp += data[i].ToString("x2").ToLower();
        return resp;
    }
}