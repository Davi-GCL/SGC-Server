using Microsoft.AspNetCore.Mvc;
using SGC.Aplication.Services;
using SGC.Domain.Interfaces;
using SGC.Domain.Entities;
using SGC.Domain.Entities.DTOs;
using System.Collections.Generic;
using SGC.Infrastructure.Repositories;
using SGC.Domain.Validators;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;

namespace SGC_Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConnectionController : ControllerBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly IClassBuilderService _classBuilderService;
        private FormConnection _connStorage = new FormConnection();

        public ConnectionController(IClassBuilderService ClassBuilderService, IDatabaseService databaseService)
        {
            //_sqlServerTableRepo = SqlServerTableRepository;
            //_oracleTableRepo = OracleTableRepo;
            _classBuilderService = ClassBuilderService;
            _databaseService=databaseService;
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
                //Altera a implementação da classe para o objeto repo 
                IRepository repo;
                switch (formConnection.Sgbd)
                {
                    case 1:
                        repo = new SqlServerTableRepository();
                        break;
                    case 2:
                        repo = new OracleTableRepository();
                        break;
                    default: return BadRequest("SGBD invalido");
                } 

                return Ok( await _databaseService.GetAllTablesInformationAsync(formConnection, repo));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost("Class")]
        public async Task<IActionResult> BuildClass([FromBody] FormTables formTables)
        {
            string classString = "";
            Table table = new Table();
            //var urlList = new List<byte[]>();
            var urlDict = new Dictionary<string, byte[]>();
            var classList = new List<GeneratedClass>();
            try
            {
                var validation = await new FormTablesValidator().ValidateAsync(formTables);
                if(!validation.IsValid)
                {
                    validation.Errors?.Select(x => new ValidationResult()
                    {
                        Codigo= x.ErrorCode,
                        NomePropriedade= x.PropertyName,
                        Mensagem = x.ErrorMessage
                    });
                    return BadRequest(validation);
                }
                //Altera a implementação da classe para o objeto repo 

                IRepository repo;
                switch (formTables.Sgbd)
                {
                    case 1:
                        repo = new SqlServerTableRepository();
                        break;
                    case 2:
                        repo = new OracleTableRepository();
                        break;
                    default: return BadRequest("SGBD invalido");
                }
                classList = await _databaseService.GetTablesInformationAsync(formTables,repo,_classBuilderService);
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
            RepositoryClass<SqlConnection> aux = new RepositoryClass<SqlConnection>(new SqlConnection());

            return Ok("Okay!");
        }

    }
}
//"Data Source=OPERACIONAL39\\SQLEXPRESS;Initial Catalog=sistema_banco;Persist Security Info=True;User ID=sa;Password=root"
