using Microsoft.AspNetCore.Mvc;
using SGC.Aplication.Services;
using SGC.Domain.Interfaces;
using SGC.Domain.Entities;
using SGC.Domain.Entities.DTOs;
using System.Collections.Generic;
using SGC.Infrastructure.Repositories;
using System.Text;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using SGC.Domain.Validators;
using System.Linq;

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
        public async Task<IActionResult> Connect([FromBody] FormConnection formConnection)
        {
            //_connStorage.ConnString = formConnection.ConnString;
            Console.WriteLine(formConnection.ConnString);
            try
            {
                var validation = await new FormConnectionValidator().ValidateAsync(formConnection);
                if (!validation.IsValid)
                {
                    var errors = validation.Errors?.Select(x=> new ValidationResult()
                    {
                        Codigo=x.ErrorCode,
                        NomePropriedade=x.PropertyName,
                        Mensagem=x.ErrorMessage
                    });
                    return BadRequest(errors);
                }

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
            string classString = "";
            Table table = new Table();
            //var urlList = new List<byte[]>();
            var urlDict = new Dictionary<string, byte[]>();
            var classList = new List<GeneratedClass>();
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
                    //Adiciona a lista, o nome da classe, a classe transformado em array de bytes (base 64) para download e a string com o corpo da classe
                    classList.Add(new GeneratedClass() { Name=selectedTableName.ToLower(),Download=Encoding.ASCII.GetBytes(classString), Description=classString });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(classList);
        }
        
        [HttpGet("/get")]
        public IActionResult Get()
        {
            return Ok("Okay!");
        }

    }
}
//"Data Source=OPERACIONAL39\\SQLEXPRESS;Initial Catalog=sistema_banco;Persist Security Info=True;User ID=sa;Password=root"
