using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGC.Domain.Interfaces
{
    public interface IFileService
    {
        //Utiliza uma classe escrita em uma string para criar um arquivo de classe na pasta ClassFiles
        public byte[] GenerateFile(string className ,string classString);
    }
}
