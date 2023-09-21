using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using SGC.Aplication.Services;
using SGC.Domain.Interfaces;
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
        private readonly IClassBuilderService _classBuilderService;

        public ConnectionController(ITableRepository SqlServerTableRepository, IClassBuilderService ClassBuilderService)
        {
            _sqlServerTableRepo = SqlServerTableRepository;
            _classBuilderService = ClassBuilderService;
        }

        [HttpPost("/connect")]
        public IActionResult Connect([FromBody] FormConnection formConnection)
        {
            return Ok(_sqlServerTableRepo.GetAllMetaData(formConnection.ConnString));
        }

        [HttpPost("/class")]
        public IActionResult BuildClass([FromBody] FormConnection formConnection)
        {
            var tablesList = _sqlServerTableRepo.GetAllMetaData(formConnection.ConnString);
            return Ok(_classBuilderService.GenerateClass(tablesList[0], "SGC.Aplication.Services", formConnection.Sgbd));
        }
    }
}
//"Data Source=OPERACIONAL39\\SQLEXPRESS;Initial Catalog=sistema_banco;Persist Security Info=True;User ID=sa;Password=root"