using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApiAutoresCLase.Data;
using WebApiAutoresCLase.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configuracion de Swagger para usar JWT
// ESto se puede eliminar para algo de la vidda real es solo para hacer prueba
// ESto es para los metodo de autenticacion (Metodo que solo tienen acceso limitado)
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { 
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{ }
        }
    });;

});


// Conexion a la base de datos
builder.Services.AddDbContext<AplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Para usar la autenticacion de Microsoft (Con usuarios y roles) de Identity
// Aqui se agrega la nueva Tabla modificada de la tabla original ***************************************************************
//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AplicationDbContext>()
    .AddDefaultTokenProviders();

// Configuracion del AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

//Configurar el JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opciones => opciones.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["LlaveJWT"])),
        ClockSkew = TimeSpan.Zero
    });

// Generar Roles
// Configurar claims de autorizacion (ESto es para poner los tipos de usuarios, se puede copiar estas lineas tantos tipos queramos)
builder.Services.AddAuthorization(opciones => {
    opciones.AddPolicy("EsAdmin", politica => politica.RequireClaim("esAdmin"));
    opciones.AddPolicy("EsCliente", politica => politica.RequireClaim("esCliente"));
});// Se crea dto ya que no tenemos modelos para hacer el tipo de rol

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
