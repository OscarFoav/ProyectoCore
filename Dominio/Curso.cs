// si no se agregan al crear la clase, agregarlas manualmente
using System;
using System.Collections.Generic;
namespace Dominio
{
    public class Curso
    {
        public Guid CursoId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }        
        public DateTime? FechaPublicacion { get; set; } // DateTime? significa que permitimos nulos en la fecha (por defecto no se permite)
        public byte[] FotoPortada {get;set;}
        // relacion con Precio (1 a 1)
        public Precio PrecioPromocion { get; set; }
        // relacion con Comentario (1 a muchos)
        public ICollection<Comentario> ComentarioLista { get; set; }
        // relacion con CursoInstructor (1 a muchos)
        public ICollection<CursoInstructor> InstructoresLink {get;set;}
    }
}