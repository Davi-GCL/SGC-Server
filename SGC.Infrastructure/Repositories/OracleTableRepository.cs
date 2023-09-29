using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using SGC.Domain.Entities;
using SGC.Domain.Interfaces;


namespace SGC.Infrastructure.Repositories
{
    public class OracleTableRepository : ITableRepository
    {
        private OracleConnection? conn;

        public IList<Table> GetAllMetaData(string connString)
        {
            conn = new OracleConnection(connString);
            //conn.ConnectionString = connString;
            try
            {
                //Verifica se a conexao está fechada ou já está aberta antes de conectar
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }



            //SELECT table_name  from all_tables where owner = 'SISCOM';
            //SELECT* from ALL_TAB_COLUMNS where owner = 'SISCOM';
            //cmd.CommandText = "SELECT table_name  from all_tables where owner = 'SISCOM'";
            dynamic result = "";
            var tables = new List<Table>();
            var columns = new List<Column>();
            string tableName = "";
            string catalog = "";

            using (OracleCommand command = new OracleCommand("select * from ALL_TAB_COLUMNS where owner = 'SISCOM'", conn))
            {
                
                OracleDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    
                    catalog = reader.GetValue(0).ToString();
                    //tableName = reader.GetValue(2).ToString();
                    try
                    {
                        var column = new Column()
                        {
                            TableName = reader.GetValue(1).ToString(),
                            Name = reader.GetValue(2).ToString(),
                            IsPrimaryKey = reader.GetValue(32).ToString() == "YES" ? true : false,
                            IsNullable = reader.GetValue(9).ToString() == "YES" ? true : false,
                            Type = reader.GetValue(3).ToString(),
                            CharMaxLength = reader.GetValue(26) != DBNull.Value ? Decimal.ToInt32( (decimal)reader.GetValue(26) ) : null,

                        };

                        columns.Add(column);
                    }
                    catch (Exception ex)
                    {
                        reader.Close();
                        conn.Close();
                        throw new Exception(ex.Message);
                    }
                }
                reader.Close();
            }

            var columnGroup = new List<Column>();
            var columnGroup2 = new List<Column>();

            for (int i = 0; i < columns.Count; i++)
            {

                if (i > 0)
                {
                    if (columns[i-1].TableName != columns[i].TableName)
                    {
                        //Se o loop está em um item de uma tabela diferente da que estava antes, as colunas da tabela já passada são armazenadas, atribuidas à essa tabela;
                        tables.Add(new Table() { Catalog=catalog, Name=columns[i-1].TableName, Columns= columnGroup });
                        columnGroup = new List<Column>();
                        continue;
                    }
                    else if (i == columns.Count-1)
                    {
                        //Se a iteração chegou no ultimo item
                        columnGroup.Add(columns[i]);
                        tables.Add(new Table() { Catalog=catalog, Name=columns[i].TableName, Columns= columnGroup });
                    }
                }
                columnGroup.Add(columns[i]);
            }

            conn.Close();
            //tables.Add(new Table() { Catalog=catalog, Name=tableName, Columns= columns });
            return tables;
        }

        public Table GetMetaDataByTableName(string connString, string tableName)
        {
            conn = new OracleConnection();
            conn.ConnectionString = connString;
            try
            {

                //Verifica se a conexao está fechada ou já está aberta antes de conectar
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            //cmd.CommandText = "SELECT * from INFORMATION_SCHEMA.TABLES";
            dynamic result = "";
            var tables = new List<Table>();
            var columns = new List<Column>();
            string catalog = "";

            using (OracleCommand command = new OracleCommand($"select * from ALL_TAB_COLUMNS where owner = 'SISCOM' and TABLE_NAME = '{tableName.ToUpper()}'", conn))
            {
                OracleDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    //Se não encontrar encontrar a tabela em questão, lança uma exceção
                    throw new Exception("tabela não encontrada!") ;
                } 
                while (reader.Read())
                {
                    catalog = reader.GetValue(0).ToString();
                    //tableName = reader.GetValue(2).ToString();
                    try
                    {
                        var column = new Column()
                        {
                            TableName = reader.GetValue(1).ToString(),
                            Name = reader.GetValue(2).ToString(),
                            IsPrimaryKey = reader.GetValue(32).ToString() == "YES" ? true : false,
                            IsNullable = reader.GetValue(9).ToString() == "YES" ? true : false,
                            Type = reader.GetValue(3).ToString(),
                            CharMaxLength = reader.GetValue(26) != DBNull.Value ? Decimal.ToInt32((decimal)reader.GetValue(26)) : null,

                        };

                        columns.Add(column);
                    }
                    catch (Exception ex)
                    {
                        reader.Close();
                        conn.Close();
                        throw new Exception(ex.Message);
                    }
                }
                reader.Close();

                return new Table() { Catalog = catalog, Name = tableName, Columns = columns };
            }
        }
    }
}

