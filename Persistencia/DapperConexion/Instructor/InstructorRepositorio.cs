using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace Persistencia.DapperConexion.Instructor
{
    public class InstructorRepositorio : IInstructor
    {
        private readonly IFactoryConection _factoryConnection;
        public InstructorRepositorio(IFactoryConection factoryConection)
        {
            _factoryConnection = factoryConection;
        }
        public async Task<int> Actualiza(Guid instructorId, string nombre, string apellidos, string titulo)
        {
            var storeProcedure = "usp_instructor_editar";
            try
            {
                var connection = _factoryConnection.GetConnection();
                var resultado = await connection.ExecuteAsync(
                    storeProcedure,
                    new
                    {
                        InstructorId = instructorId,
                        Nombre = nombre,
                        Apellidos = apellidos,
                        Titulo = titulo
                    },
                    commandType: CommandType.StoredProcedure
                );

                _factoryConnection.CloseConnection();
                return resultado;
            }
            catch (Exception e)
            {
                throw new Exception("No se ha podido guardar los datos del Instructor", e);
            }
        }

        public async Task<int> Elimina(Guid id)
        {
            var storeProcedure = "usp_Instructor_Elimina";
            try{
                var connection = _factoryConnection.GetConnection();
                var resultado = await connection.ExecuteAsync(
                    storeProcedure,
                    new {
                        InstructorId = id
                    },
                    commandType : CommandType.StoredProcedure
                );
                _factoryConnection.CloseConnection();
                return resultado;
            }catch(Exception e){
                throw new Exception("No se ha podido eliminar el Instructor", e);
            }
        }

        public async Task<int> Nuevo(string nombre, string apellidos, string titulo)
        {
            var storeProdecure = "usp_instructor_nuevo";
            try
            {
                var connection = _factoryConnection.GetConnection();
                var resultado = await connection.ExecuteAsync(storeProdecure, new
                {
                    InstructorId = Guid.NewGuid(),
                    Nombre = nombre,
                    Apellidos = apellidos,
                    Titulo = titulo
                },
                commandType: CommandType.StoredProcedure
                );
                _factoryConnection.CloseConnection();
                return resultado;
            }
            catch (Exception e)
            {
                throw new Exception("No se ha podido guardar el nuevo Instructor", e);
            }
        }

        public async Task<IEnumerable<InstructorModel>> ObtenerLista()
        {
            IEnumerable<InstructorModel> instructorList = null;
            var storeProcedure = "usp_obtener_instructores";
            try
            {
                var connection = _factoryConnection.GetConnection();
                instructorList = await connection.QueryAsync<InstructorModel>(storeProcedure, null, commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw new Exception("Error al consultar los datos", e);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }
            return instructorList;
        }

        public async Task<InstructorModel> ObtenerPorId(Guid id)
        {
            var storeProcedure = "usp_obtener_instructor_por_id";
            InstructorModel instructor = null;
            try{
                var connection = _factoryConnection.GetConnection();
                instructor = await connection.QueryFirstAsync<InstructorModel>(
                    storeProcedure,
                    new {
                        id = id
                    },
                    commandType : CommandType.StoredProcedure
                );
                return instructor;
            }catch(Exception e){
                throw new Exception("No se ha podido encontrar al Instructor", e);
            }
        }
    }
}