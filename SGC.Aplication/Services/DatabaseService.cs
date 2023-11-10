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
            List<Table> tables = new List<Table>();
            //var urlList = new List<byte[]>();
            var urlDict = new Dictionary<string, byte[]>();
            var classList = new List<GeneratedClass>();

            tables = await repository.GetMetaDataByTableNameList(formTables.ConnString, formTables.SelectedTablesNames); 
            
            foreach (var table in tables)
            {
                //Retorna uma classe escrita em uma string
                classString = classBuilderService.GenerateClass(table, formTables.Namespace, formTables.Sgbd);
                
                //Adiciona a lista, o nome da classe, a classe transformado em array de bytes (base 64) para download e a string com o corpo da classe                                                                                                
                classList.Add(new GeneratedClass() { Name=table.Name.ToLower(), Download=Encoding.ASCII.GetBytes(classString), Description=classString });
            }
            return classList;
        }
    }
}
