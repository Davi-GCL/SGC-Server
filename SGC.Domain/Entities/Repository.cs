using SGC.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGC.Domain.Entities
{
    public abstract class Repository<T> : IRepository
    {
        protected T Conn;
        public abstract Task<IList<Table>> GetAllMetaData(string connString);
        public abstract Task<Table> GetMetaDataByTableName(string connString, string tableName);
    }
}
