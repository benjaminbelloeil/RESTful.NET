using System.Text;
using ApiMovies.Data;
using ApiMovies.Models;
using ApiMovies.MovieMapper;
using ApiMovies.Repository;
using ApiMovies.Repository.IRepository;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql")));

// Soporte para cache
builder.Services.AddResponseCaching();

// Agregamos los Repositorios
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IPeliculaRepositorio, PeliculaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

var key = builder.Configuration.GetValue<string>("AppSettings:Secreta");

// Sporte para authenticacion con .Net Identity

builder.Services.AddIdentity<AppUsuario, IdentityRole>()
    .AddEntityFrameworkStores<AppDBContext>()
    .AddDefaultTokenProviders();

// soporte para versionamiento
var apiVersioningBuilder = builder.Services.AddApiVersioning(opcion =>
{
    opcion.AssumeDefaultVersionWhenUnspecified = true;
    opcion.DefaultApiVersion = new ApiVersion(1, 0);
    opcion.ReportApiVersions = true;
    // opcion.ApiVersionReader = ApiVersionReader.Combine
    // (
    //     new QueryStringApiVersionReader("api-version") //?api-version=1.0
    //     // new HeaderApiVersionReader("X-Version");
    //     // new MediaTypeApiVersionReader("ver");
    //         
    // );
});

apiVersioningBuilder.AddApiExplorer(
    opciones =>
    {
        opciones.GroupNameFormat = "'v'VVV";
        opciones.SubstituteApiVersionInUrl = true;
    }
);

// Agregamos el AutoMapper
builder.Services.AddAutoMapper(typeof(MovieMappers));

// Aqui se configura la authenticacion
builder.Services.AddAuthentication
    (
        x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }
    ).AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers(opcion =>
{
    // Cache profile. Un cache global para no tener que ponerse en todas partes.
    opcion.CacheProfiles.Add("PorDefecto30Segundos", new CacheProfile() { Duration = 30 });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Authentication JWT Bearer Token \r\n\r\n" 
                          + "Insert the word bearer followed by a space and the token given to you when you register \r\n\r\n" 
                          + "Authorization Example: Bearer ksnxanixanjcnes",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Scheme = "Bearer",
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });
        options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1.0",
                Title = "Peliculas API v1.0",
                Description = "Peliculas de ASP.NET Core Web API",
                TermsOfService = new Uri("https://benjaminbelloeil.vercel.app/explore"),
                Contact = new OpenApiContact
                {
                    Name = "Benjamin Belloeil",
                    Url = new Uri("https://benjaminbelloeil.vercel.app/explore")
                },
                License = new OpenApiLicense
                {
                    Name = "Licensia Personal",
                    Url = new Uri("https://benjaminbelloeil.vercel.app/explore")
                }
            }
        );
        
        options.SwaggerDoc("v2", new OpenApiInfo
            {
                Version = "v2.0",
                Title = "Peliculas API v2.0",
                Description = "Peliculas de ASP.NET Core Web API",
                TermsOfService = new Uri("https://benjaminbelloeil.vercel.app/explore"),
                Contact = new OpenApiContact
                {
                    Name = "Benjamin Belloeil",
                    Url = new Uri("https://benjaminbelloeil.vercel.app/explore")
                },
                License = new OpenApiLicense
                {
                    Name = "Licensia Personal",
                    Url = new Uri("https://benjaminbelloeil.vercel.app/explore")
                }
            }
        );
    }
);

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
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(opciones =>
{
    opciones.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiPelciulasV1");
    opciones.SwaggerEndpoint("/swagger/v2/swagger.json", "ApiPelciulasV2");
});
app.MapControllers();

app.UseHttpsRedirection();

app.Run();