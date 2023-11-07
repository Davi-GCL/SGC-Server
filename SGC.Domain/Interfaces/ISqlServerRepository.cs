using SGC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGC.Domain.Interfaces
{
    public interface ISqlServerRepository : IRepository
    {
        Task<IList<Table>> GetAllMetaData(string connString);
        Task<Table> GetMetaDataByTableName(string connString, string tableName);
    }
}
