using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class ConsultaId
    {

        public class CursoInico : IRequest<Curso>
        {
            public int Id { get; set; }
        }

        public class Manejador : IRequestHandler<CursoInico, Curso>
        {
            private readonly CursosOnLineContext _context;
            public Manejador(CursosOnLineContext context)
            {
                _context = context;
            }
            public async Task<Curso> Handle(CursoInico request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso.FindAsync(request.Id);

                if (curso == null)
                {
                    // throw new Exception("No se puede eliminar el curso.");
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se encontró el curso" });
                }
                return curso;
            }
        }
    }
}