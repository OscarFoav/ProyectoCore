namespace Dominio
{
    public class Comentario
    {
        public int ComentarioId { get; set; }
        public string Alumno { get; set; }
        public int Puntaje { get; set; }
        public string ComentarioTexto { get; set; }
        public int CursoId { get; set; }

        // relacion con Curso (1 a 1)
        public Curso Curso { get; set; }
    }
}