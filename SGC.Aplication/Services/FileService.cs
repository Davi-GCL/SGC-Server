using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGC.Domain.Interfaces;
using System.Globalization;

namespace SGC.Aplication.Services
{
    public class FileService : IFileService
    {
        public byte[] GenerateFile(string className , string classString)
        {
            var currentDir = Environment.CurrentDirectory;
            //Faz com o que o nome do arquivo seja pascal case(primeira letra maiuscula);
            className = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(className.ToLower());
            string filePath = $"ClassFiles\\{className}.cs";
            var fullPath = Path.Combine(currentDir, filePath);

            try
            {
                using (var file = File.CreateText(fullPath))
                {
                    file.Write(classString);
                    file.Close();
                }
                    
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return File.ReadAllBytes(fullPath);
        }
    }
}
