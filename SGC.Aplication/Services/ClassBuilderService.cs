using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGC.Domain.Entities;
using SGC.Domain.Interfaces;

namespace SGC.Aplication.Services
{
    public class ClassBuilderService : IClassBuilderService
    {
        private string ClassName { get; set; }
        //private Dictionary<string, string> FieldsDict = new Dictionary<string, string>();
        private Dictionary<string, string> SqlServerTypes = new Dictionary<string, string>()
        {
            {"int","int"}, 
            {"tinyint","byte"},
            {"char","string" },
            {"smallint","short"},
            {"bigint","long"},
            {"money","decimal"},
            {"datetime", "DateTime"},
            {"varchar", "string"},
            {"text", "string"},
            {"bit", "bool" },
            {"blob","byte[]"},
            {"clob","String"},
            {"nclob","String"}
        };
        private Dictionary<string, string> OracleTypes = new Dictionary<string, string>()
        {
            {"integer","int"},
            {"varchar", "string"},
            {"varchar2","string"},
            {"char","string"},
            {"nvarchar2","string"},
            {"number","double"},
            {"date","DateTime"},
            {"timestamp", "DateTime"},
            {"binary_float","float"},
            {"binary_double","double"},
            {"blob","byte[]"},
            {"clob","String"},
            {"nclob","String"}

        };
        
        public string GenerateClass(Table table, string namespaceName,int dbType)
        {
        //Metodo que retorna uma string de classe escrita com base em uma tabela e suas colunas, considerando seus tipos baseado nos tipos de dado c#
            string getSetTemplate = "{ get; set; }";

            Dictionary<string, string> FieldsDict = this.TableMap(table , dbType);
    
            StringBuilder propsBuild = new StringBuilder();

            foreach(KeyValuePair<string,string> field in FieldsDict)
            {
                propsBuild.AppendLine($"\t{field.Value} {field.Key} {getSetTemplate}");
            }

            StringBuilder classBuild = new StringBuilder();

            classBuild.AppendLine($"public class {this.ClassName}\n{{\n {propsBuild} \n}}");

            StringBuilder usingsBuild = new StringBuilder();

            usingsBuild.AppendLine("using System;");
            usingsBuild.AppendLine("using System.Collections.Generic;");
            usingsBuild.AppendLine($"\nnamespace {namespaceName};\n");
            usingsBuild.AppendLine(classBuild.ToString());

            return usingsBuild.ToString();
        }
        
        public Dictionary<string, string> TableMap(Table table, int dbType)
        {
        //Funcao que mapeia o dicionario FieldsDict(nome_coluna, tipo_dado) com o nome da coluna e seu respectivo tipo no C#, e indicando se é anulavel
            this.ClassName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(table.Name.ToLower());
            string type = "";
            
            Dictionary<string, string> FieldsDict = new Dictionary<string, string>();

            switch(dbType)
            {
                case 1:
                    foreach (var c in table.Columns)
                    {
                        type = this.SqlServerTypes[c.Type.ToLower()];
                        if (c.IsNullable == true) { type += "?"; } //Se a coluna for anulavel, adiciona a indicação de tipo anulavel do C#
                        FieldsDict.Add(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(c.Name), type);
                    }
                break;
                case 2:
                    foreach (var c in table.Columns)
                    {
                        type = this.OracleTypes[c.Type.ToLower()];
                        if (c.IsNullable == true) { type += "?"; } //Se a coluna for anulavel, adiciona a indicação de tipo anulavel do C#
                        FieldsDict.Add(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(c.Name), type);
                    }
                break;
                default:
                    throw new Exception("sgbd invalido!");
            }

            return FieldsDict;
        }
        
        public string TranslateType(string sqlType)
        {
            var typesDict = new Dictionary<string, string>()
            {
                {"int","int"}, 
                {"tinyint","int"}, 
                {"money","decimal"},
                {"datetime", "DateTime"}
            };
            return "";
        }
    }
}
