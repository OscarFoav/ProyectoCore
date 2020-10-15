using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.Cursos;
using Aplicacion.Seguridad.TokenSeguridad;
using AutoMapper;
using Dominio;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistencia;
using Persistencia.DapperConexion;
using Persistencia.DapperConexion.Instructor;
using Seguridad;
using WebAPI.Middleware;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CursosOnLineContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            // Instanciar que se lance IFactoryConnection y IInstructor al arrancar el proyecto
            services.AddOptions();

            // Conexión a dapper
            services.Configure<ConexionConfiguracion>(Configuration.GetSection("ConnectionStrings"));

            services.AddMediatR(typeof(Consulta.Manejador).Assembly);

            // La modificación de debajo es para incluir la librería FluentValidation
            // services.AddControllers();
            services.AddControllers( opt => {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());
            // incluimos Core Identity <54> en WebAPI
            var builder = services.AddIdentityCore<Usuario>();
            var idenitifyBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            idenitifyBuilder.AddEntityFrameworkStores<CursosOnLineContext>();
            idenitifyBuilder.AddSignInManager<SignInManager<Usuario>>();
            services.TryAddSingleton<ISystemClock, SystemClock>();
            // Instanciar que se lance IFactoryConnection y IInstructor al arrancar el proyecto
            services.AddTransient<IFactoryConection, FactoryConnection>();
            services.AddScoped<IInstructor, InstructorRepositorio>();

            // Soportar Swagger
            services.AddSwaggerGen( c=> {
                c.SwaggerDoc("v1", new OpenApiInfo{
                    Title = "Servicion de mantenimiento de cursos.",
                    Version = "v1"
                });
                c.CustomSchemaIds(c=>c.FullName);
            });
            
            // Incluir seguridad del Token (posterior a JWT)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => {
                opt.TokenValidationParameters = new TokenValidationParameters{
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });

            // JWT JSON Web Tokens
            services.AddScoped<IJwtGenerador, JwtGenerador>();
            services.AddScoped<IUsuarioSesion, UsuarioSesion>();
            services.AddAutoMapper(typeof(Consulta.Manejador));
            // Instanciar que se lance IFactoryConnection y al arrancar el proyecto
            services.AddTransient<IFactoryConection, FactoryConnection>();
            services.AddScoped<IInstructor, InstructorRepositorio>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ManejadorErrorMiddleware>();


            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
            }

            // Se comenta porque todavía no es producción
            // app.UseHttpsRedirection();

            // Aquí hay que informar que usamos autenticación
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Soportar Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c=> {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cursos OnLine versión 1");
            });
        }
    }
}
