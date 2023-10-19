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
using SGC.Infrastructure.Repositories;
using System.Globalization;
using System.Text;

namespace SGC_Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConnectionController : ControllerBase
    {
        private readonly ITableRepository _sqlServerTableRepo = new SqlServerTableRepository();
        private readonly ITableRepository _oracleTableRepo = new OracleTableRepository();
        private readonly IClassBuilderService _classBuilderService;
        private readonly IFileService _fileService;
        private FormConnection _connStorage = new FormConnection();

        public ConnectionController(IClassBuilderService ClassBuilderService, IFileService fileService)
        {
            //_sqlServerTableRepo = SqlServerTableRepository;
            //_oracleTableRepo = OracleTableRepo;
            _classBuilderService = ClassBuilderService;
            _fileService = fileService;
            //_connStorage = new FormConnection();
        }

        [HttpPost("Connect")]
        public IActionResult Connect([FromBody] FormConnection formConnection)
        {
            //_connStorage.ConnString = formConnection.ConnString;
            Console.WriteLine(formConnection.ConnString);
            try
            {
                //Switch para utilizar o repository correspondente ao sgbd selecionado
                switch (formConnection.Sgbd)
                {
                    case 1:
                        return Ok(_sqlServerTableRepo.GetAllMetaData(formConnection.ConnString));
                        //break;
                    case 2:
                        return Ok(_oracleTableRepo.GetAllMetaData(formConnection.ConnString));
                        //break;
                    default: return BadRequest("SGBD invalido!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            
        }

        [HttpPost("Class")]
        public IActionResult BuildClass([FromBody] FormTables formTables)
        {
            var classString = "";
            var table = new Table();
            //var urlList = new List<byte[]>();
            var urlDict = new Dictionary<string, byte[]>();
            try
            {         
                foreach (var selectedTableName in formTables.SelectedTablesNames)
                {
                    //Switch para utilizar o repository correspondente ao sgbd selecionado
                    switch (formTables.Sgbd)
                    {
                        case 1:
                            table = _sqlServerTableRepo.GetMetaDataByTableName(formTables.ConnString, selectedTableName);
                            break;
                        case 2: 
                            table = _oracleTableRepo.GetMetaDataByTableName(formTables.ConnString, selectedTableName);
                            break;
                        default: return BadRequest("SGBD invalido!");
                    }
                    classString = _classBuilderService.GenerateClass(table, formTables.Namespace, formTables.Sgbd); //Retorna uma classe escrita em uma string
                    urlDict.Add(
                        selectedTableName.ToLower(),
                        Encoding.ASCII.GetBytes(classString)
                    ); //Adiciona ao dictionary, o nome da classe e a string com o corpo da classe, transformado em array de bytes (base 64)
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(urlDict);
        }
        
        [HttpGet("/get")]
        public IActionResult Get()
        {
            return Ok("Okay!");
        }

    }
}
//"Data Source=OPERACIONAL39\\SQLEXPRESS;Initial Catalog=sistema_banco;Persist Security Info=True;User ID=sa;Password=root"
