using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class RolLista
    {
        public class Ejecuta : IRequest<List<IdentityRole>> {

        }

        public class Manejador : IRequestHandler<Ejecuta, List<IdentityRole>> {
            private readonly CursosOnLineContext _context;
            public Manejador(CursosOnLineContext context){
                _context = context;
            }
            public async Task<List<IdentityRole>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // _context.Roles hace referencia a la tabla AspNetRoles de la BD CursosOnLine
                var roles = await _context.Roles.ToListAsync();
                return roles;
            }
        }
    }
}