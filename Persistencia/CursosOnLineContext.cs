// si DbContext se queda subrayado (con error) agregar la librería de debajo manualmente
using Dominio;
using Microsoft.EntityFrameworkCore;

namespace Persistencia
{
    public class CursosOnLineContext : DbContext
    {
        public CursosOnLineContext(DbContextOptions options) : base(options) {

        }

        // relacion muchos a muchos
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<CursoInstructor>().HasKey(ci => new {ci.InstructorId, ci.CursoId});
        }

        public DbSet<Comentario> Comentario {get;set;}
        public DbSet<Curso> Curso {get;set;}
        public DbSet<CursoInstructor> CursoInstructor {get;set;}
        public DbSet<Instructor> Instructor {get;set;}
        public DbSet<Precio> Precio {get;set;}
        
    }
}