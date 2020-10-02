using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            // Al incluir [Required] se obliga a que el campo Titulo (tiene que estar debajo) tenga valor, si no, no se guarda
            // [Required(ErrorMessage  = "Por favor, introduzca el Título del curso.")]
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
        }

        // Lógica de la validación con FluentValidation
        public class EjecutaValidacion : AbstractValidator<Ejecuta>{
            public EjecutaValidacion(){
                RuleFor( x => x.Titulo).NotEmpty();
                RuleFor( x => x.Descripcion).NotEmpty();
                RuleFor( x => x.FechaPublicacion).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnLineContext _context;
            public Manejador(CursosOnLineContext context){
                _context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso = new Curso {
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion
                };

                _context.Curso.Add(curso);
                var valor = await _context.SaveChangesAsync();
                // valor = 0 es que no hubo transacción
                if(valor>0){
                    return Unit.Value;  // y se termina la transacción
                }

                // Si se produce error, o sea, valor = 0
                throw new Exception("No se pudo insertar el curso.");

            }
        }
    }
}