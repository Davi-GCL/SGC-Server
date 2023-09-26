using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using SGC.Aplication.Services;
using SGC.Domain.Interfaces;
using SGC.Domain.Entities;
using SGC.Domain.Entities.DTOs;
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
        private FormConnection _connStorage = new FormConnection();

        public ConnectionController(ITableRepository SqlServerTableRepository, IClassBuilderService ClassBuilderService, IFileService fileService)
        {
            _sqlServerTableRepo = SqlServerTableRepository;
            _classBuilderService = ClassBuilderService;
            _fileService = fileService;
            //_connStorage = new FormConnection();
        }

        [HttpPost("/connect")]
        public IActionResult Connect([FromBody] FormConnection formConnection)
        {
            _connStorage.ConnString = formConnection.ConnString;
            //_sqlServerTableRepo.SetString(formConnection.ConnString);
            return Ok(_sqlServerTableRepo.GetAllMetaData(formConnection.ConnString));
        }

        [HttpPost("/class")]
        public IActionResult BuildClass([FromBody] FormTables formTables)
        {
            var classString = "";
            var table = new Table();
            try
            {         
                foreach (var selectedTableName in formTables.SelectedTablesNames)
                {
                    table = _sqlServerTableRepo.GetMetaDataByTableName(formTables.ConnString, selectedTableName);
                    classString = _classBuilderService.GenerateClass(table, formTables.Namespace, formTables.Sgbd); //Retorna uma classe escrita em uma string
                    _fileService.GenerateFile(selectedTableName, classString); //Utiliza uma classe escrita em uma string para criar um arquivo de classe na pasta ClassFiles

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(classString);
        }
    }
}
//"Data Source=OPERACIONAL39\\SQLEXPRESS;Initial Catalog=sistema_banco;Persist Security Info=True;User ID=sa;Password=root"