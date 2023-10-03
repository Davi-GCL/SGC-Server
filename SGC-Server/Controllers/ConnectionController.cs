﻿using Microsoft.AspNetCore.Http.Connections;
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
        private readonly ITableRepository _oracleTableRepo;
        private readonly IClassBuilderService _classBuilderService;
        private readonly IFileService _fileService;
        private FormConnection _connStorage = new FormConnection();

        public ConnectionController(ITableRepository SqlServerTableRepository, ITableRepository OracleTableRepo,IClassBuilderService ClassBuilderService, IFileService fileService)
        {
            _sqlServerTableRepo = SqlServerTableRepository;
            _oracleTableRepo = OracleTableRepo;
            _classBuilderService = ClassBuilderService;
            _fileService = fileService;
            //_connStorage = new FormConnection();
        }

        [HttpPost("/connect")]
        public IActionResult Connect([FromBody] FormConnection formConnection)
        {
            _connStorage.ConnString = formConnection.ConnString;
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

        [HttpPost("/class")]
        public IActionResult BuildClass([FromBody] FormTables formTables)
        {
            var classString = "";
            var table = new Table();
            var urlList = new List<byte[]>();
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
                    urlList.Add( _fileService.GenerateFile(selectedTableName, classString) ); //Utiliza uma classe escrita em uma string para criar um arquivo de classe na pasta ClassFiles

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(urlList);
        }



        [HttpGet("ObterArquivo")]
        public IActionResult ObterArquivo()
        {
            // Certifique-se de que o nome do arquivo seja seguro para evitar ataques de travessia de diretório.
            // Você também pode adicionar outras verificações de segurança, conforme necessário.

            string caminhoArquivo = @"C:\Users\davi.lemos\Desktop\Projetos_C#\SGC-Server\SGC-Server\ClassFiles\Teste.txt";

            if (System.IO.File.Exists(caminhoArquivo))
            {
                var bytesArquivo = System.IO.File.ReadAllBytes(caminhoArquivo);
                return File(bytesArquivo, "application/octet-stream", "Teste.txt");
            }
            else
            {
                return NotFound(); // Arquivo não encontrado
            }
        }

    }
}
//"Data Source=OPERACIONAL39\\SQLEXPRESS;Initial Catalog=sistema_banco;Persist Security Info=True;User ID=sa;Password=root"
