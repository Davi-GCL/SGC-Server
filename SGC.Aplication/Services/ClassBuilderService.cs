using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGC.Aplication.Services
{
    public class ClassBuilderService
    {
        public string GenerateClass() 
        {
            string className = "GeneratedClass";
            StringBuilder usingsBuild = new StringBuilder();

            usingsBuild.AppendLine("using System;");
            usingsBuild.AppendLine("using System.Collections.Generic;");
            usingsBuild.Append("namespace SGC.Aplication.Services\n{}");

            StringBuilder classBuild = new StringBuilder();

            classBuild.AppendFormat("public class {0}\n {}");

            return usingsBuild.ToString() + classBuild.ToString();
        }
    }
}
