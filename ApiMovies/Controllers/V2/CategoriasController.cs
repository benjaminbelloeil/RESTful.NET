using ApiMovies.Repository.IRepository;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiMovies.Controllers.V2
{
    // [Authorize(Roles = "Admin")]
    // [ResponseCache(Duration = 20)]
    // [EnableCors("PoliticaCors")]
    [Route("api/v{version:apiVersion}/categorias")] // opcion dinamica
    [ApiController]
    [ApiVersion("2.0")]
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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2", "value3" };
        }
    }
}