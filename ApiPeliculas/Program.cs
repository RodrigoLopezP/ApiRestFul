using System.Text;
using ApiPeliculas.Peliculas.Data;
using ApiPeliculas.PeliculasMapper;
using ApiPeliculas.Repositorio;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
        builder.Services.AddScoped<IUsuarioRepositorio,UsuarioRepositorio>();


        //Agregamos automapper
        builder.Services.AddAutoMapper(typeof(PeliculasMapper));

        //Authentication configuration
        string? key=builder.Configuration.GetValue<string>("AppSettings:Secreta");

        builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Install package Microsoft.AspNetCore.Authentication.JwtBearer
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
        ).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        ///Cross-Origin Resource Sharing
        ///Meccanismo per aggiungere degli headers extra, quindi permettiamo l'accesso usando diversi domini, per poter accedere alla nostra API,
        ///oltre a quelle che abbiamo di default
        ///con asterisco *, lo usiamo come wildcard e qualunque si l'header della api andrà bene 
        builder.Services.AddCors(
            p => p.AddPolicy("PolicyCorss",
            builder =>
            {
                builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            })
        );

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

        //add UseAuthentication before the  UseAuthorization (that it's already write here by deafult) 
        app.UseAuthentication(); // obbligatory for starting use 
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}