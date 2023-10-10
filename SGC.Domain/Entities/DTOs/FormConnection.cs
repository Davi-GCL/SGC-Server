using System.Data.Common;

namespace SGC.Domain.Entities.DTOs
{
    public class FormConnection
    {
        public int Sgbd { get; set; }

        public string ConnString { get; set; }
    }
}
