// si no se agrega al crear la clase, agregarla manualmente
using System;
// si no se agrega al crear la línea public ICollection, agregarla manualmente
using System.Collections.Generic;

namespace Dominio
{
    public class Curso
    {
        public Guid CursoId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        // DateTime? significa que permitimos nulos en la fecha (por defecto no se permite)
        public DateTime? FechaPublicacion { get; set; }
        public byte[] FotoPortada;

        // relacion con Precio (1 a 1)
        public Precio PrecioPromocion { get; set; }

        // relacion con Comentario (1 a muchos)
        public ICollection<Comentario> ComentarioLista { get; set; }

        // relacion con CursoInstructor (1 a muchos)
        public ICollection<CursoInstructor> InstructoresLink {get;set;}

    }
}