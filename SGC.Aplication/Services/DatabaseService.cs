using SGC.Domain.Entities;
using SGC.Domain.Entities.DTOs;
using SGC.Domain.Interfaces;
using SGC.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGC.Aplication.Services
{
    public class DatabaseService : IDatabaseService
    {

        public async Task<IList<Table>> GetAllTablesInformationAsync(FormConnection formConnection, IRepository repository)
        {
            return await repository.GetAllMetaData(formConnection.ConnString);
        }

        public async Task<List<GeneratedClass>> GetTablesInformationAsync(FormTables formTables, IRepository repository, IClassBuilderService classBuilderService)
        {
            string classString = "";
            Table table = new Table();
            //var urlList = new List<byte[]>();
            var urlDict = new Dictionary<string, byte[]>();
            var classList = new List<GeneratedClass>();

            foreach (var selectedTableName in formTables.SelectedTablesNames)
            {
                table = await repository.GetMetaDataByTableName(formTables.ConnString,selectedTableName);

                classString = classBuilderService.GenerateClass(table, formTables.Namespace, formTables.Sgbd); //Retorna uma classe escrita em uma string
                                                                                                                //Adiciona a lista, o nome da classe, a classe transformado em array de bytes (base 64) para download e a string com o corpo da classe
                classList.Add(new GeneratedClass() { Name=selectedTableName.ToLower(), Download=Encoding.ASCII.GetBytes(classString), Description=classString });
            }
            return classList;
        }
    }
}
