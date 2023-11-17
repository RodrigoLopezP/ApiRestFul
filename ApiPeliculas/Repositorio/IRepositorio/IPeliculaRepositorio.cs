using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiPeliculas.Modelos;
using Microsoft.Extensions.Localization;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface IPeliculaRepositorio
    {
        ICollection<Pelicula>GetPeliculas();
        Pelicula GetPelicula(int peliculaId);
        bool ExistePelicula(string nombre);
        bool ExistePelicula(int id);
        bool CrearPelicula(Pelicula pelicula);
        bool ActualizarPelicula(Pelicula Pelicula);
        bool BorrarPelicula(Pelicula Pelicula);
        bool Guardar();
        //Metodos para buscar peliculas en categoria y buscar pelicula por nombre
        ICollection<Pelicula>GetPeliculasEnCategoria(int categoriaId);
        ICollection<Pelicula>BuscarPelicula(string nombre);

    }
}