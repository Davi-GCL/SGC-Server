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
        private readonly IFileService _fileService;

        public ConnectionController(ITableRepository SqlServerTableRepository, IClassBuilderService ClassBuilderService, IFileService fileService)
        {
            _sqlServerTableRepo = SqlServerTableRepository;
            _classBuilderService = ClassBuilderService;
            _fileService = fileService;
        }

        [HttpPost("/connect")]
        public IActionResult Connect([FromBody] FormConnection formConnection)
        {
            _sqlServerTableRepo.SetString(formConnection.ConnString);
            return Ok(_sqlServerTableRepo.GetAllMetaData());
        }

        [HttpPost("/class")]
        public IActionResult BuildClass([FromBody] FormTables formTables)
        {
            var classString = "";
            var tablesList = _sqlServerTableRepo.GetAllMetaData();

            for(var i = 0; i < tablesList.Count; i++)
            {
                classString = _classBuilderService.GenerateClass(tablesList[i], "SGC.Aplication.Services", formConnection.Sgbd);
                try
                {
                    _fileService.GenerateFile(tablesList[i].Name, classString);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            

            return Ok(classString);
        }
    }
}
//"Data Source=OPERACIONAL39\\SQLEXPRESS;Initial Catalog=sistema_banco;Persist Security Info=True;User ID=sa;Password=root"