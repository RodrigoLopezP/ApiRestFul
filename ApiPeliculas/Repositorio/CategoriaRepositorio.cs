using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiPeliculas.Modelos;
using ApiPeliculas.Peliculas.Data;
using ApiPeliculas.Repositorio.IRepositorio;

namespace ApiPeliculas.Repositorio
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly ApplicationDbContext _bd;
        public CategoriaRepositorio(ApplicationDbContext bd)
        {
            this._bd=bd;
        }        
        public bool ActualizarCategoria(Categoria categoria)
        {
            categoria.FechaCreacion=DateTime.Now;
            _bd.Categorias.Update(categoria);
            return Guardar();
        }

        public bool BorrarCategoria(Categoria categoria)
        {
            _bd.Categorias.Remove(categoria);
            return Guardar();
        }

        public bool CrearCategoria(Categoria categoria)
        {
            categoria.FechaCreacion=DateTime.Now;
            _bd.Categorias.Add(categoria);
            return Guardar();
        }

        public bool ExisteCategoria(string nombre)
        {
            bool valorExiste = _bd.Categorias
                                .Any(c => c.Nombre.ToLower().Trim()==
                                            nombre.ToLower().Trim());
            return valorExiste;
        }

        public bool ExisteCategoria(int id)
        {
            bool valorExiste = _bd.Categorias
                                .Any(c => c.Id==id);
            return valorExiste;
        }

        public Categoria GetCategoria(int categoriaId)
        {
            Categoria result = _bd.Categorias
                                .FirstOrDefault(c=> c.Id == categoriaId);
            return result;
        }

        public ICollection<Categoria> GetCategorias()
        {
            return _bd.Categorias.OrderBy(c => c.Nombre).ToList();
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }
    }
}