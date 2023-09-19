using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using SGC.Aplication.Services;
using SGC.Infrastructure.Repositories;
using SGC_Server.Model;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml.Linq;


namespace SGC_Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConnectionController : ControllerBase
    {
        private readonly ITableRepository _sqlServerTableRepo;
        private readonly ClassBuilderService _classBuilderService = new ClassBuilderService();

        public ConnectionController(ITableRepository SqlServerTableRepository)
        {
            _sqlServerTableRepo = SqlServerTableRepository;
        }

        [HttpPost("/connect")]
        public IActionResult Connect([FromBody] FormConnection formConnection)
        {
            return Ok(_sqlServerTableRepo.GetAllMetaData(formConnection.ConnString));
        }

        [HttpGet("/class")]
        public IActionResult BuildClass()
        {
            return Ok(_classBuilderService.GenerateClass());
        }
    }
}
//"Data Source=OPERACIONAL39\\SQLEXPRESS;Initial Catalog=sistema_banco;Persist Security Info=True;User ID=sa;Password=root"