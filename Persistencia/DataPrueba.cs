using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Identity;

namespace Persistencia
{
    public class DataPrueba
    {
        public static async Task InsertarData(CursosOnLineContext context, UserManager<Usuario> UsuarioManager){
            if(!UsuarioManager.Users.Any()){
                var usuario = new Usuario{NombreCompleto = "Óscar", UserName="OscarFoav", Email="oscar.foav@gmail.com"};
                await UsuarioManager.CreateAsync(usuario, "Password$123");
            }
        }
    }
}