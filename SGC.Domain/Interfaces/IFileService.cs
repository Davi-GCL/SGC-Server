using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGC.Domain.Interfaces
{
    public interface IFileService
    {
        public string GenerateFile(string className ,string classString);
    }
}
