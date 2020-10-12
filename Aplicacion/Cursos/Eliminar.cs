using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnLineContext _context;
            public Manejador(CursosOnLineContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                /* para eliminar un curso primero hay que eliminar sus instructores de CursoInstructor */
                var instructoresBD = _context.CursoInstructor.Where(x=>x.CursoId == request.Id);
                foreach(var instructor in instructoresBD){
                    _context.CursoInstructor.Remove(instructor);
                }
                
                var curso = await _context.Curso.FindAsync(request.Id);
                if (curso == null)
                {
                    // throw new Exception("No se puede eliminar el curso.");
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No se encontró el curso"});
                }

                _context.Remove(curso);

                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudieron guardar los cambios.");
            }
        }
    }
}