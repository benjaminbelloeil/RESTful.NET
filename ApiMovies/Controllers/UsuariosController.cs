using System.Net;
using ApiMovies.Models;
using ApiMovies.Models.Dtos;
using ApiMovies.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ApiMovies.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepositorio _usRepo;
        protected RespuestaAPI _respuestaAPI;
        private readonly IMapper _mapper;

        public UsuariosController(IUsuarioRepositorio usRepo, IMapper mapper)
        {
            _usRepo = usRepo;
            _mapper = mapper;
            this._respuestaAPI = new();
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUsuarios()
        {
            var listaUsuarios = _usRepo.GetUsuarios();
            var listaUsuariosDto = new List<UsuarioDto>();
            foreach (var lista in listaUsuarios)
            {
                listaUsuariosDto.Add(_mapper.Map<UsuarioDto>(lista));
            }
            return Ok(listaUsuariosDto);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet("{usuarioId:int}", Name = "GetUsuario")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUsuario(int usuarioId)
        {
            var itemUsuario  = _usRepo.GetUsuario(usuarioId);

            if (itemUsuario == null)
            {
                return NotFound();
            }
            
            var itemUsuarioDto = _mapper.Map<UsuarioDto>(itemUsuario);
            
            return Ok(itemUsuarioDto);
        }
        
        [AllowAnonymous]
        [HttpPost("registro")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Registro([FromBody] UsuarioRegistroDto usuarioRegistroDto) 
        {
            bool validarNombreUsuarioUnico = _usRepo.IsUniqueUser(usuarioRegistroDto.NombreUsuario);
            if(!validarNombreUsuarioUnico)
            {
                _respuestaAPI.statusCode = HttpStatusCode.BadRequest;
                _respuestaAPI.IsSuccess = false;
                _respuestaAPI.ErrorMessages.Add("El nombre de usuario ya existe");
                return BadRequest(_respuestaAPI);
            }
            
            var usuario = await _usRepo.Registro(usuarioRegistroDto);
            if (usuario == null)
            {
                _respuestaAPI.statusCode = HttpStatusCode.BadRequest;
                _respuestaAPI.IsSuccess = false;
                _respuestaAPI.ErrorMessages.Add("Error en el registro");
                return BadRequest(_respuestaAPI);
            }
            
            _respuestaAPI.statusCode = HttpStatusCode.OK;
            _respuestaAPI.IsSuccess = true;
            return Ok(_respuestaAPI);
        }
        
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDto usuarioLoginDto) 
        {
            var respuestaLogin = await _usRepo.Login(usuarioLoginDto);
            if(respuestaLogin.Usuario == null || string.IsNullOrEmpty(respuestaLogin.Token))
            {
                _respuestaAPI.statusCode = HttpStatusCode.BadRequest;
                _respuestaAPI.IsSuccess = false;
                _respuestaAPI.ErrorMessages.Add("El nombre de usuario o password son incorrectos");
                return BadRequest(_respuestaAPI);
            }
            
            _respuestaAPI.statusCode = HttpStatusCode.OK;
            _respuestaAPI.IsSuccess = true;
            _respuestaAPI.Result = respuestaLogin;
            return Ok(_respuestaAPI);
          
        }
    }
}

