using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ApiMovies.Data;
using ApiMovies.Models;
using ApiMovies.Models.Dtos;
using ApiMovies.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ApiMovies.Repository;

public class UsuarioRepositorio : IUsuarioRepositorio
{
    private readonly AppDBContext _bd;
    private string claveSecreta;
    private readonly UserManager<AppUsuario> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;

    public UsuarioRepositorio(AppDBContext bd, IConfiguration config, UserManager<AppUsuario> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
    {
        _bd = bd;
        claveSecreta = config.GetValue<string>("AppSettings:Secreta");
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
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
        var usuarioBd = _bd.AppUsuario.FirstOrDefault(u => u.UserName == usuario);
        if (usuarioBd == null)
        {
            return true;
        }
        return false;
    }

    public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
    {
        {
            if (usuarioLoginDto == null || string.IsNullOrEmpty(usuarioLoginDto.NombreUsuario) || string.IsNullOrEmpty(usuarioLoginDto.Password))
            {
                throw new ArgumentNullException("Invalid login request. Username and password must be provided.");
            }
            // var passwordEncriptado = obtenermd5(usuarioLoginDto.Password);
            var usuario = _bd.AppUsuario.FirstOrDefault(
                    u => u.UserName.ToLower() == usuarioLoginDto.NombreUsuario.ToLower());
            
            bool isValid = await _userManager.CheckPasswordAsync(usuario, usuarioLoginDto.Password);
        
            // Validamos si el usuario no existe con la combinacion de usuario y contraseña
            if (usuario == null || isValid == false)
            {
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
            
            }
            // aqui existe el usuario entonces podemos procesar el login
            var roles = await _userManager.GetRolesAsync(usuario);
            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = manejadorToken.CreateToken(tokenDescriptor);

            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto = new UsuarioLoginRespuestaDto()
            {
                Token = manejadorToken.WriteToken(token),
                Usuario = _mapper.Map<UsuarioDatosDto>(usuario),
            };
            return usuarioLoginRespuestaDto;
        }
    }
    
    public async Task<UsuarioDatosDto> Registro(UsuarioRegistroDto usuarioRegistroDto)
    {
        // var passwordEncriptado = obtenermd5(usuarioRegistroDto.Password);

        AppUsuario usuario = new AppUsuario()
        {
            UserName = usuarioRegistroDto.NombreUsuario,
            Email = usuarioRegistroDto.NombreUsuario,
            NormalizedEmail = usuarioRegistroDto.NombreUsuario.ToUpper(),
            Nombre = usuarioRegistroDto.Nombre,
        };
        
        var result = await _userManager.CreateAsync(usuario, usuarioRegistroDto.Password);

        if (result.Succeeded)
        {
            if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Registrado"));
            }
            
            await _userManager.AddToRoleAsync(usuario, "Admin");
            var usuarioRetornado = _bd.AppUsuario.FirstOrDefault(u => u.UserName == usuarioRegistroDto.NombreUsuario);
            
            return _mapper.Map<UsuarioDatosDto>(usuarioRetornado);
        }

        // _bd.Usuario.Add(usuario);
        // await _bd.SaveChangesAsync();
        // usuario.Password = passwordEncriptado;
        return new UsuarioDatosDto();
    }
    // Metodo para encriptar contraseña con MD5
    // public static string obtenermd5(string valor)
    // {
    //     MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
    //     byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
    //     data = x.ComputeHash(data);
    //     string resp = "";
    //     for (int i = 0; i < data.Length; i++)
    //         resp += data[i].ToString("x2").ToLower();
    //     return resp;
    
}