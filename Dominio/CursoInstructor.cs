using System;
namespace Dominio
{
    public class CursoInstructor
    {
        public Guid CursoId { get; set; }
        // relacion con Curso (1 a 1)

        public Curso Curso { get; set; }

        public Guid InstructorId { get; set; }

        // relacion con Instructor (1 a 1)
        public Instructor Instructor { get; set; }
    }
}