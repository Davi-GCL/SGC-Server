﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            var columns = new List<Column>();
            string tableName = "";
            string catalog = "";

            OracleCommand command = new OracleCommand("select OWNER, TABLE_NAME, COLUMN_NAME, DATA_TYPE, NULLABLE, CHAR_LENGTH, IDENTITY_COLUMN from ALL_TAB_COLUMNS where OWNER = 'SISCOM' order by TABLE_NAME", conn);
            OracleDataReader reader = command.ExecuteReader();

            if (!reader.HasRows) { throw new DataException("Query has not results rows"); } //Se não encontrar encontrar tabelas, lança uma exceção
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
                        Type = reader.GetValue(3).ToString(),
                        IsNullable = reader.GetValue(4).ToString() == "YES" ? true : false,
                        CharMaxLength = reader.GetValue(5) != DBNull.Value ? Decimal.ToInt32((decimal)reader.GetValue(5)) : null,
                        IsPrimaryKey = reader.GetValue(6).ToString() == "YES" ? true : false,
                    };
                    columns.Add(column);
                }
                //Mapeia a lista de colunas para uma lista de tabelas, agrupando as colunas com suas respectivas tabelas pelo nome
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
            
            var columns = new List<Column>();
            string catalog = "";

            using (OracleCommand command = new OracleCommand($"select OWNER, TABLE_NAME, COLUMN_NAME, DATA_TYPE, NULLABLE, CHAR_LENGTH, IDENTITY_COLUMN from ALL_TAB_COLUMNS where owner = 'SISCOM' and TABLE_NAME = '{tableName.ToUpper()}'", conn))
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
                            Type = reader.GetValue(3).ToString(),
                            IsNullable = reader.GetValue(4).ToString() == "YES" ? true : false,
                            CharMaxLength = reader.GetValue(5) != DBNull.Value ? Decimal.ToInt32((decimal)reader.GetValue(5)) : null,
                            IsPrimaryKey = reader.GetValue(6).ToString() == "YES" ? true : false,
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

