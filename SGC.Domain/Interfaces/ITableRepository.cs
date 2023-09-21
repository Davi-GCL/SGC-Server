using SGC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGC.Domain.Interfaces
{
    public interface ITableRepository
    {
        public IList<Table> GetAllMetaData(string connString);
    }
}
