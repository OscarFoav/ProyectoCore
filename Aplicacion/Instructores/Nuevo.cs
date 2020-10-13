using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class Nuevo
    {
        public class Ejecuta : IRequest{
            // No es necesario Guid porque lo genera la lógica del programa
            public string Nombre {get;set;}
            public string Apellidos {get;set;}
            public string Titulo {get;set;}
        }

        public class EjecutaValida : AbstractValidator<Ejecuta>{
            public EjecutaValida(){
                RuleFor(x=>x.Nombre).NotEmpty();
                RuleFor(x=>x.Apellidos).NotEmpty();
                RuleFor(x=>x.Titulo).NotEmpty();
            }
        }

        public  class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly IInstructor _instructorRepository;
            public Manejador(IInstructor instructorRepository){
                _instructorRepository = instructorRepository;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var resultado = await _instructorRepository.Nuevo(request.Nombre, request.Apellidos, request.Titulo);
                if(resultado > 0){
                    return Unit.Value;
                }

                throw new Exception("No se ha podido insertar el Instructor");
            }
        }
    }
}