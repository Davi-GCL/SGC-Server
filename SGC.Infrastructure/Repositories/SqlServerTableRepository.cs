using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGC.Domain.Entities;
using SGC.Domain.Interfaces;


namespace SGC.Infrastructure.Repositories
{
    public class SqlServerTableRepository : ITableRepository
    {
        private SqlConnection conn = new SqlConnection();

        public IList<Table> GetAllMetaData(string connString)
        {
            conn = new SqlConnection();
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
            var columns = new List<Column>();
            string tableName = "";
            string catalog = "";

            SqlCommand command = new SqlCommand("select TABLE_CATALOG, TABLE_NAME, COLUMN_NAME, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS order by TABLE_NAME", conn);
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    catalog = reader.GetValue(0).ToString();
                    //tableName = reader.GetValue(2).ToString();
                    
                    var column = new Column()
                    {
                        TableName = reader.GetValue(1).ToString(),
                        Name = reader.GetValue(2).ToString(),
                        IsPrimaryKey = false,
                        IsNullable = reader.GetValue(3).ToString() == "YES" ? true : false,
                        Type = reader.GetValue(4).ToString(),
                        CharMaxLength = reader.GetValue(5) != DBNull.Value ? (int)reader.GetValue(5) : null,
                    };
                    columns.Add(column);     
                }
                List<Table> tables = TableMapper.ColumnsToTableList(columns, catalog);
                return tables;
            }
            catch (ArgumentException)
            { 
                throw new Exception("Columns not found");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Close();
                reader.Close();  
            }
            
            //conn.Close ();
            //tables.Add(new Table() { Catalog=catalog, Name=tableName, Columns= columns });
            //List<Table> tables = TableMapper.ColumnsToTableList(columns, catalog);
            //return tables;
        }

        public Table GetMetaDataByTableName(string connString ,string tableName)
        {
            conn = new SqlConnection();
            conn.ConnectionString = connString;
            try
            {
                //Verifica se a conexao está fechada ou já está aberta antes de conectar
                if (conn.State == System.Data.ConnectionState.Closed){ conn.Open(); }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            //cmd.CommandText = "SELECT * from INFORMATION_SCHEMA.TABLES";

            var columns = new List<Column>();
            string catalog = "";

            using (SqlCommand command = new SqlCommand($"select TABLE_CATALOG, TABLE_NAME, COLUMN_NAME, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='{tableName}'", conn))
            {
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        catalog = reader.GetValue(0).ToString();
                        //tableName = reader.GetValue(2).ToString();

                        var column = new Column()
                        {
                            TableName = reader.GetValue(1).ToString(),
                            Name = reader.GetValue(2).ToString(),
                            IsPrimaryKey = false,
                            IsNullable = reader.GetValue(3).ToString() == "YES" ? true : false,
                            Type = reader.GetValue(4).ToString(),
                            CharMaxLength = reader.GetValue(5) != DBNull.Value ? (int)reader.GetValue(5) : null,
                        };
                        columns.Add(column);
                    }
                }
                catch (Exception ex)
                { 
                    throw new Exception(ex.Message);
                }
                finally
                {
                    conn.Close();
                    reader.Close();
                }

                return new Table() { Catalog = catalog, Name = tableName, Columns = columns };
            }
        }
    }
}
