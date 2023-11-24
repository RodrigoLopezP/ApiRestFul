using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.DTO;
using ApiPeliculas.Peliculas.Data;
using ApiPeliculas.Repositorio.IRepositorio;
using XSystem.Security.Cryptography;

namespace ApiPeliculas.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UsuarioRepositorio(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
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
        public Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            throw new NotImplementedException();
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
    }
}