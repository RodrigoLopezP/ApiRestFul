using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.DTO;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepositorio _ctRepo;
        private readonly IMapper _mapper;
        protected RespuestaAPI _respuestaApi;
        public UsuariosController(IUsuarioRepositorio usuarioRepositorio, IMapper mapper)
        {
            _ctRepo = usuarioRepositorio;
            _mapper = mapper;
            _respuestaApi= new();
        }
        
        [Authorize(Roles ="admin")]
        [HttpGet("getusuarios")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)] 
        [Authorize(Roles ="admin")]
        public IActionResult GetUsuario(){
            var listaUsuarios = _ctRepo.GetUsuarios();
            var listaUsuariosDTO= new List<UsuarioLoginDto>();
            foreach (var lista in listaUsuariosDTO)
            {
                listaUsuariosDTO.Add(_mapper.Map<UsuarioLoginDto>(lista));
            }
            return Ok(listaUsuariosDTO);
        }
        
        [Authorize(Roles ="admin")]
        [HttpGet("{userId:int}", Name = "GetUsuario")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUsuario(int userId)
        {
            Usuario itemUsuario = _ctRepo.GetUsuario(userId);
            if (itemUsuario == null)
                return NotFound();
            UsuarioDto itemUsuarioDto = _mapper.Map<UsuarioDto>(itemUsuario);
            return Ok(itemUsuarioDto);
        }
        
        [AllowAnonymous]
        [HttpPost("registro")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Registro([FromBody] UsuarioRegistroDto usuarioRegistroDto)
        {
            bool validarNombreUsuarioUnico=_ctRepo.IsUniqueUser(usuarioRegistroDto.NombreUsuario);
            if(!validarNombreUsuarioUnico){
                _respuestaApi.StatusCode=HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess=false;
                _respuestaApi.ErrorMessages.Add("El nombre de usuario ya existe");
                return BadRequest(_respuestaApi);
            }

            Usuario? usuario = await _ctRepo.Registro(usuarioRegistroDto);
            
            if (usuario is null)
            {
                _respuestaApi.StatusCode=HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess=false;
                _respuestaApi.ErrorMessages.Add("Error en el registro");
                return BadRequest(_respuestaApi);
            }

            _respuestaApi.StatusCode = HttpStatusCode.OK;
            _respuestaApi.IsSuccess = true;
            return BadRequest(_respuestaApi);
        }

        
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDto usuarioLoginDto)
        {
            UsuarioLoginRespuestaDto respuestaLogin= await _ctRepo.Login(usuarioLoginDto);

            bool validarNombreUsuarioUnico = _ctRepo.IsUniqueUser(usuarioLoginDto.NombreUsuario);
            if (respuestaLogin.Usuario==null || string.IsNullOrEmpty(respuestaLogin.Token))
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("El nombre de usuario a password son incorrectos");
                return BadRequest(_respuestaApi);
            }

            _respuestaApi.StatusCode = HttpStatusCode.OK;
            _respuestaApi.IsSuccess = true;
            _respuestaApi.Result=respuestaLogin;
            return Ok(_respuestaApi);
        }


    }
}