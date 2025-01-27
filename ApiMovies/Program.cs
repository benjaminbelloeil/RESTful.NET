using ApiMovies.Data;
using ApiMovies.MovieMapper;
using ApiMovies.Repository;
using ApiMovies.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql")));

// Agregamos los Repositorios
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IPeliculaRepositorio, PeliculaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

// Agregamos el AutoMapper
builder.Services.AddAutoMapper(typeof(MovieMappers));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Soporte par CORS
// Se puede hablilitar: 1- Un Dominio 2- Multiple Dominios
// 3- Cualquier Dominio (ocupamos tener en cuenta la seguridad)
// Usamos de ejemplo el dominio: http://localhost:3223, se tiene que cambiar por el correcto
// Se usa (*) para todos los dominios
builder.Services.AddCors(p => p.AddPolicy("PoliticaCors", build =>
{
    build.WithOrigins("http://localhost:3223").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // optional dev-specific settings
}

app.UseCors("PoliticaCors");

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.UseHttpsRedirection();

app.Run();