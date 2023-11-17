using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiPeliculas.Modelos;
using ApiPeliculas.Peliculas.Data;
using ApiPeliculas.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Repositorio
{
    public class PeliculaRepositorio : IPeliculaRepositorio
    {
        private readonly ApplicationDbContext _bd;
        public PeliculaRepositorio(ApplicationDbContext bd)
        {
            this._bd=bd;
        }        
        public bool ActualizarPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion=DateTime.Now;
            _bd.Peliculas.Update(pelicula);
            return Guardar();
        }
        public bool BorrarPelicula(Pelicula pelicula)
        {
            _bd.Peliculas.Remove(pelicula);
            return Guardar();
        }
        public bool CrearPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion=DateTime.Now;
            _bd.Peliculas.Add(pelicula);
            return Guardar();
        }
        public bool ExistePelicula(string nombre)
        {
            bool valorExiste = _bd.Peliculas
                                .Any(c => c.Nombre.ToLower().Trim()==
                                            nombre.ToLower().Trim());
            return valorExiste;
        }
        public bool ExistePelicula(int id)
        {
            bool valorExiste = _bd.Peliculas
                                .Any(c => c.Id==id);
            return valorExiste;
        }
        public Pelicula GetPelicula(int peliculaId)
        {
            Pelicula result = _bd.Peliculas
                                .FirstOrDefault(peli=> peli.Id == peliculaId);
            return result;
        }
        public ICollection<Pelicula> GetPeliculas()
        {
            return _bd.Peliculas.OrderBy(peli => peli.Nombre).ToList();
        }

        public ICollection<Pelicula> GetPeliculasEnCategoria(int categoriaId)
        {
            return _bd.Peliculas
                            .Include(pe => pe.Categoria)
                            .Where(pe => pe.CategoriaId == categoriaId).ToList();
        }
        public ICollection<Pelicula> BuscarPelicula(string nombre)
        {
            IQueryable<Pelicula>query=_bd.Peliculas;
            if(!string.IsNullOrEmpty(nombre)){
                query=query.Where
                            (p => p.Nombre.ToLower()
                                .Contains(nombre.ToLower()) ||
                            p.Descripcion.ToLower().Contains(nombre.ToLower()));
            }
            return query.ToList();
        }
        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }
    }
}