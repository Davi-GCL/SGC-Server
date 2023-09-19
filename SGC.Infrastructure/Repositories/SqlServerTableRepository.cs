using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGC.Domain.Entities;


namespace SGC.Infrastructure.Repositories
{
    public class SqlServerTableRepository : ITableRepository
    {
        private SqlConnection conn = new SqlConnection();
        public IList<Table> GetAllMetaData(string connString)
        {
            try
            {
                conn.ConnectionString = connString;

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
            string tableName = "";
            string catalog = "";

            using (SqlCommand command = new SqlCommand("SELECT * from INFORMATION_SCHEMA.COLUMNS", conn))
            {
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    catalog = reader.GetValue(0).ToString();
                    //tableName = reader.GetValue(2).ToString();
                    try
                    {
                        var column = new Column()
                        {
                            TableName = reader.GetValue(2).ToString(),
                            Name = reader.GetValue(3).ToString(),
                            IsPrimaryKey = false,
                            IsNullable = reader.GetValue(6).ToString() == "YES" ? true : false,
                            Type = reader.GetValue(7).ToString(),
                            CharMaxLength = reader.GetValue(8) != DBNull.Value ? (int)reader.GetValue(8) : null,

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


            //tables.Add(new Table() { Catalog=catalog, Name=tableName, Columns= columns });
            return tables;
        }
    }
}
