using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGC.Domain.Entities;

namespace SGC.Infrastructure;

public class TableMapper
{
    public static List<Table> ColumnsToTableList(List<Column> columns, string catalog)
    {
        if(columns.Count == 0) { throw new ArgumentException("Amount of columns list is zero"); }

        var tables = new List<Table>();
        var columnGroup = new List<Column>();

        for (int i = 0; i < columns.Count; i++)
        {
            if (i > 0)
            {
                if (columns[i - 1].TableName != columns[i].TableName)
                {
                    //Se o loop está em um item de uma tabela diferente da que estava antes, as colunas da tabela já passada são armazenadas, atribuidas à essa tabela;
                    tables.Add(new Table() { Catalog = catalog, Name = columns[i - 1].TableName, Columns = columnGroup });
                    columnGroup = new List<Column>();
                }
                else if (i == columns.Count - 1)
                {
                    //Se a iteração chegou no ultimo item
                    columnGroup.Add(columns[i]);
                    tables.Add(new Table() { Catalog = catalog, Name = columns[i].TableName, Columns = columnGroup });
                }
            }
            columnGroup.Add(columns[i]);
        }
        return tables;
    }
}
