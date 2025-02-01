using ApiMovies.Models;
using ApiMovies.Models.Dtos;
using ApiMovies.Repository.IRepository;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiMovies.Controllers.V1
{
    // [Authorize(Roles = "Admin")]
    // [ResponseCache(Duration = 20)]
    // [EnableCors("PoliticaCors")]
    [Route("api/v{version:apiVersion}/categorias")] // opcion dinamica
    [ApiController]
    [ApiVersion("1.0")]
    // [Obsolete("Deprecated in version 1.0, will be removed in version 3.0")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepositorio _ctRepo;
        private readonly IMapper _mapper;

        public CategoriasController(ICategoriaRepositorio ctRepo, IMapper mapper)
        {
            _ctRepo = ctRepo;
            _mapper = mapper;
        }
        
        [HttpGet("GetString")]
        // [MapToApiVersion("2.0")]
        [Obsolete("Deprecated in version 1.0, will be removed in version 3.0")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2", "value3" };
        }

        [AllowAnonymous]
        [HttpGet]
        // [ResponseCache(Duration = 20)] 
        // [EnableCors("PoliticaCors")]
        // [ResponseCache(CacheProfileName = "PorDefecto30Segundos")] 
        // [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        
        public IActionResult GetCategorias()
        {
            var ListaCategorias = _ctRepo.GetCategorias();
            var ListaCategoriasDto = new List<CategoriaDto>();
            foreach (var lista in ListaCategorias)
            {
                ListaCategoriasDto.Add(_mapper.Map<CategoriaDto>(lista));
            }
            return Ok(ListaCategoriasDto);
        }
        
        [AllowAnonymous]
        [HttpGet("{categoriaId:int}", Name = "GetCategoria")]
        // [ResponseCache(Duration = 20)]
        // [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoria(int categoriaId)
        {
            var itemCategoria  = _ctRepo.GetCategoria(categoriaId);

            if (itemCategoria == null)
            {
                return NotFound();
            }
            
            var itemCategoriaDto = _mapper.Map<CategoriaDto>(itemCategoria);
            
            return Ok(itemCategoriaDto);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult CrearCategoria([FromBody] CrearCategoriaDto crearCategoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (crearCategoriaDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_ctRepo.ExsiteCategoria(crearCategoriaDto.Nombre))
            {
                ModelState.AddModelError("", $"La categoria ya existe");
                return StatusCode(404, ModelState);
            }
            
            var categoria = _mapper.Map<Categoria>(crearCategoriaDto);

            if (!_ctRepo.CrearCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal, guardando el registro {categoria.Nombre}");
                return StatusCode(404, ModelState);
            }
            
            return CreatedAtRoute("GetCategoria", new { categoriaId = categoria.Id}, categoria);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPatch("{categoriaId:int}", Name = "ActualizarPatchCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult ActualizarPatchCategoria(int categoriaId, [FromBody] CategoriaDto categoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (categoriaDto == null || categoriaId != categoriaDto.Id)
            {
                return BadRequest(ModelState);
            }
            
            var categoriaExistente = _ctRepo.GetCategoria(categoriaId);
            if (categoriaExistente == null)
            {
                return NotFound($"No se encontra la categoria con Id {categoriaId}");
            }
            
            var categoria = _mapper.Map<Categoria>(categoriaDto);

            if (!_ctRepo.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal, actualizando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            
            return NoContent();
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPut("{categoriaId:int}", Name = "ActualizarPutCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarPutCategoria(int categoriaId, [FromBody] CategoriaDto categoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (categoriaDto == null || categoriaId != categoriaDto.Id)
            {
                return BadRequest(ModelState);
            }

            var categoriaExistente = _ctRepo.GetCategoria(categoriaId);
            if (categoriaExistente == null)
            {
                return NotFound($"No se encontra la categoria con Id {categoriaId}");
            }
            
            var categoria = _mapper.Map<Categoria>(categoriaDto);

            if (!_ctRepo.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            
            return NoContent();
        }
        
        [Authorize(Roles = "Admin")]
        [HttpDelete("{categoriaId:int}", Name = "BorrarCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BorrarCategoria(int categoriaId)
        {
            if (!_ctRepo.ExsiteCategoria(categoriaId))
            {
                return NotFound();
            }
            
            var categoria = _ctRepo.GetCategoria(categoriaId);

            if (!_ctRepo.BorrarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal borrando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            
            return NoContent();
        }
    }
}