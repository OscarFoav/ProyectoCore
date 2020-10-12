using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            public Guid CursoId { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }

            // la expresión DateTime? "fuerza" que un campo fecha permita nulos 
            // ,cosa que no es cierta por defecto, los campos Fecha no admiten nulos
            public DateTime? FechaPublicacion { get; set; }

            public List<Guid> ListaInstructor {get;set;}
        }
        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.Descripcion).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
            }
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
                var curso = await _context.Curso.FindAsync(request.CursoId);
                if (curso == null)
                {
                    // throw new Exception("No se puede eliminar el curso.");
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No se encontró el curso"});
                }

                // el operador ?? evalúa el valor de la variable, si NO ha cambiado no se actualiza, se mantiene curso.Titulo
                // vendría siendo algo así, si request.Titulo es nulo mantén el valor que tenía en la tabla
                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;

                if(request.ListaInstructor!=null){
                    if(request.ListaInstructor.Count>0){
                        /* 1º eliminar instructores actuales del curso en la BD*/
                        var InstructoresBD = _context.CursoInstructor.Where(x => x.CursoId == request.CursoId).ToList();
                        foreach(var instructorEliminar in InstructoresBD){
                            _context.CursoInstructor.Remove(instructorEliminar);
                        }
                        /* fin proc. eliminar instructores*/

                        /* 2º adjuntar los instructores que provienen del cliente*/
                        foreach(var ids in request.ListaInstructor){
                            var nuevoInstructor = new CursoInstructor{
                                CursoId = request.CursoId,
                                InstructorId = ids
                            };
                            _context.CursoInstructor.Add(nuevoInstructor);
                        }
                        /* Fin del proc. 2º */
                    }
                }

                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se guardaron los cambio en el curso.");
            }
        }
    }
}