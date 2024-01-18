using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.DTO;
using ApiPeliculas.Peliculas.Data;
using ApiPeliculas.Repositorio.IRepositorio;
using XSystem.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Data;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using AutoMapper;

namespace ApiPeliculas.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private string claveSecreta;
        private readonly IMapper _mapper;
        public UsuarioRepositorio(ApplicationDbContext applicationDbContext, IConfiguration config, IMapper mapper)
        {
            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
            this._applicationDbContext = applicationDbContext;
            this._mapper = mapper;
        }
        public Usuario GetUsuario(int usuarioId)
        {
            return _applicationDbContext.Usuarios.FirstOrDefault(u=> u.Id==usuarioId);
        }

        public ICollection<Usuario> GetUsuarios()
        {
            return _applicationDbContext.Usuarios.OrderBy(u=> u.NombreUsuario).ToList();
        }

        public bool IsUniqueUser(string nombre)
        {
            var usuariobd= _applicationDbContext.Usuarios.FirstOrDefault(u=> u.NombreUsuario==nombre);
            if(usuariobd==null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
            var passwordEncriptado= obtenermd5(usuarioRegistroDto.Password);
            Usuario usuario=new()
            {
                NombreUsuario = usuarioRegistroDto.NombreUsuario,
                Password = passwordEncriptado,
                Nombre = usuarioRegistroDto.Nombre,
                Role=usuarioRegistroDto.Role  
            };
            _applicationDbContext.Usuarios.Add(usuario);
            await _applicationDbContext.SaveChangesAsync();
            usuario.Password=passwordEncriptado;
            
            return usuario;
        }
        
        //Método para encriptar contraseña con MD5 se usa tanto en el Acceso como en el Registro
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
        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto){
            string passwordEncriptado= obtenermd5(usuarioLoginDto.Password);

            Usuario? usuario = _applicationDbContext.Usuarios.FirstOrDefault(u =>
                                            u.NombreUsuario.ToLower() == usuarioLoginDto.NombreUsuario.ToLower()
                                            && u.Password == passwordEncriptado);

            if (usuario == null)
            {
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
            }
            //Aqui existe el usuario

            JwtSecurityTokenHandler manejadorToken= new();
            byte[] key = Encoding.ASCII.GetBytes(claveSecreta);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Name, usuario.NombreUsuario.ToString()),
                    new (ClaimTypes.Role, usuario.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = manejadorToken.CreateJwtSecurityToken(tokenDescriptor);

            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto= new UsuarioLoginRespuestaDto(){
                Token=manejadorToken.WriteToken(token),
                Usuario= usuario
            };

            return usuarioLoginRespuestaDto;
        }
    }
}