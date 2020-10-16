using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Persistencia.DapperConexion.Paginación
{
    public class PaginacionRepositorio : IPaginacion
    {
        private readonly IFactoryConection _factoryConection;
        public PaginacionRepositorio(IFactoryConection factoryConection){
            _factoryConection = factoryConection;
        }
        public async Task<PaginacionModel> devolverPaginacion(string storeProcedure, int numeroPagina, int cantidadElementos, IDictionary<string, object> parametrosFiltro, string ordenamientoColumna)
        {
            PaginacionModel paginacionModel = new PaginacionModel();
            List<IDictionary<string,object>> listaReporte = null;
            int totalRecords = 0;
            int totalPaginas = 0;
            try{
                var connection = _factoryConection.GetConnection();
                DynamicParameters parametros = new DynamicParameters();

                // Parámetros de entrada
                // Aquí se insertan todos los posibles filtros seleccionados, que son un array
                foreach(var param in parametrosFiltro){
                    parametros.Add("@" + param.Key, param.Value);
                }
                parametros.Add("@NumeroPagina", numeroPagina);
                parametros.Add("@CantidadElementos", cantidadElementos);
                parametros.Add("@Ordenamiento", ordenamientoColumna);

                // Parámetros de salida (devueltos después de terminar la consulta en la BD)
                parametros.Add("@TotalRecords", totalRecords, DbType.Int32, ParameterDirection.Output);
                parametros.Add("@TotalPaginas", totalPaginas, DbType.Int32, ParameterDirection.Output);

                var result = await connection.QueryAsync(storeProcedure, parametros, commandType : CommandType.StoredProcedure);
                listaReporte = result.Select(x => (IDictionary<string,object>)x ).ToList();
                paginacionModel.ListaRecords = listaReporte;
                paginacionModel.NumeroPaginas = parametros.Get<int>("@TotalPaginas");
                paginacionModel.TotalRecords = parametros.Get<int>("@TotalRecords");
            }catch(Exception e){
                throw new System.Exception("No se ha podido ejecutar el procedimiento almacenado", e);
            }finally{
                _factoryConection.CloseConnection();
            }
            return paginacionModel;
        }
    }
}