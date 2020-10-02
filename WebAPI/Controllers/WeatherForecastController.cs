using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Persistencia;

namespace WebAPI.Controllers
{
    // http://localhost:5000/WeatherForecast

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly CursosOnLineContext context;
        public WeatherForecastController(CursosOnLineContext _context){
            this.context = _context;
        }

        [HttpGet]
        public IEnumerable<Curso> Get(){
            return context.Curso.ToList();
        }
    }
}
