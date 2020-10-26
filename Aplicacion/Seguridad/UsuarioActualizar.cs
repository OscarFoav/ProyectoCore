using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class UsuarioActualizar
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string NombreCompleto { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Username { get; set; }
        }

        public class EjecutaValidador : AbstractValidator<Ejecuta>
        {
            public EjecutaValidador()
            {
                RuleFor(x => x.NombreCompleto).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Username).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly CursosOnLineContext _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IJwtGenerador _jwtGenerador;
            private readonly IPasswordHasher<Usuario> _passwordHasser;
            public Manejador(CursosOnLineContext context, UserManager<Usuario> userManager,
                             IJwtGenerador jwtGenerador,IPasswordHasher<Usuario> passwordHasser){
                _context = context;
                _userManager = userManager;
                _jwtGenerador = jwtGenerador;
                _passwordHasser = passwordHasser;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuarioIden = await _userManager.FindByNameAsync(request.Username);
                if(usuarioIden==null){
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No existe usuario con este Username"});
                }
                var resultado = await _context.Users.Where(x=>x.Email == request.Email && x.UserName != request.Username).AnyAsync();
                if(resultado){
                    throw new ManejadorExcepcion(HttpStatusCode.InternalServerError, new {mensaje = "Ya existe un usuario con este email"});
                }
                usuarioIden.NombreCompleto = request.NombreCompleto;
                usuarioIden.PasswordHash = _passwordHasser.HashPassword(usuarioIden, request.Password);
                usuarioIden.Email = request.Email;
                var resultadoUpdate = await _userManager.UpdateAsync(usuarioIden);
                var resultadoRoles = await _userManager.GetRolesAsync(usuarioIden);
                var listRoles = new List<string>(resultadoRoles);
                if(resultadoUpdate.Succeeded){
                    return new UsuarioData{
                        NombreCompleto = usuarioIden.NombreCompleto,
                        Username = usuarioIden.UserName,
                        Email = usuarioIden.Email,
                        Token = _jwtGenerador.CrearToken(usuarioIden, listRoles)
                    };
                }
                throw new Exception("No se ha podido actualizar el usuario");
            }
        }
    }
}