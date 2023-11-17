using ApiPeliculas.Peliculas.Data;
using ApiPeliculas.PeliculasMapper;
using ApiPeliculas.Repositorio;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        string strConn=builder.Configuration.GetConnectionString("Default");
        // Add services to the container.
        builder.Services.AddDbContext<ApplicationDbContext>(opciones=>
        {
            opciones.UseSqlite(strConn, opts => {});
        });
        //agregamos servicios
        builder.Services.AddScoped<ICategoriaRepositorio,CategoriaRepositorio>();
        builder.Services.AddScoped<IPeliculaRepositorio,PeliculaRepositorio>();

        //Agregamos automapper
        builder.Services.AddAutoMapper(typeof(PeliculasMapper)); 

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}