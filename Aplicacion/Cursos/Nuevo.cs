using System;
using System.Collections.Generic;
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

            public List<Guid> ListaInstructor {get;set;}

            public decimal Precio {get;set;}
            public decimal Promocion {get;set;}
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
                Guid _cursoId = Guid.NewGuid();
                var curso = new Curso {
                    CursoId = _cursoId,
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion
                };

                _context.Curso.Add(curso);

                // Graba los instructores del curso
                if(request.ListaInstructor!=null){                    
                    foreach(var id in request.ListaInstructor){
                        var cursoInstructor = new CursoInstructor{
                            CursoId = _cursoId,
                            InstructorId = id
                        };
                        _context.CursoInstructor.Add(cursoInstructor);
                    }
                }

                // Insertar precio del curso nuevo
                var precioEntidad = new Precio{
                    CursoId = _cursoId,
                    PrecioActual = request.Precio,
                    Promocion = request.Promocion,
                    PrecioId = Guid.NewGuid()
                };
                _context.Precio.Add(precioEntidad);

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