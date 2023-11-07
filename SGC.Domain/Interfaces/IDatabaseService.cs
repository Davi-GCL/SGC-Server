using SGC.Domain.Entities;
using SGC.Domain.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGC.Domain.Interfaces
{
    public interface IDatabaseService
    {
        Task<IList<Table>> GetAllTablesInformationAsync(FormConnection formConnection, IRepository repository);

        Task<List<GeneratedClass>> GetTablesInformationAsync(FormTables formTables, IRepository repository, IClassBuilderService classBuilderService);

    }
}
