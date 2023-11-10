using Oracle.ManagedDataAccess.Client;
using SGC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGC.Domain.Interfaces
{
    public interface IOracleRepository : IRepository
    {
        Task<IList<Table>> GetAllMetaData(string connString);
        Task<Table> GetMetaDataByTableName(string connString, string tableName);
    }
}
