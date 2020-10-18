using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class ObtenerRolesPorUsuario
    {
        public class Ejecuta : IRequest<List<string>> {
            public string UserName {get;set;}            
        }

        public class Manejador : IRequestHandler<Ejecuta, List<string>>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            public Manejador(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager){
                _userManager = userManager;
                _roleManager = roleManager;
            }

            public async Task<List<string>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuarioIden = await _userManager.FindByNameAsync(request.UserName);
                if(usuarioIden==null){
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No existe el usuario"});                    
                }
                var resultado = await _userManager.GetRolesAsync(usuarioIden);
                // resultado es un IList pero lo que se espera que devolvamos es un List, 
                // por eso hacemos un cast para convertir IList en List
                return new List<string>(resultado);
            }
        }
    }
}