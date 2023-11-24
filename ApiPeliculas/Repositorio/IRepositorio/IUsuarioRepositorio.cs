using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.DTO;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        ICollection<Usuario> GetUsuarios();
        Usuario GetUsuario(int usuarioId);
        bool IsUniqueUser(string nombre);
        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);
        Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto);
    }
}