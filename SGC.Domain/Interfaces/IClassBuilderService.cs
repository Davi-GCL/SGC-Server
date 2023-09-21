using SGC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGC.Domain.Interfaces
{
    public interface IClassBuilderService
    {
        
        public string GenerateClass(Table table, string namespaceName,int dbType);
        public void TableMap(Table table, int dbType);
        public string TranslateType(string type);

    }
}
