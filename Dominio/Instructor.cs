using System;
using System.Collections.Generic;

namespace Dominio
{
    public class Instructor
    {
        public Guid InstructorId  {get;set;}
        public string Nombre {get;set;}
        public  string Apellidos {get;set;}
        public string Grado {get;set;}
        public byte[] FotoPerfil {get;set;}

        // relacion con CursoInstructor (1 a muchos)
        public ICollection<CursoInstructor> CursoLink {get;set;}
    }
}