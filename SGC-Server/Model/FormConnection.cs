using System.Data.Common;

namespace SGC_Server.Model
{
    public class FormConnection
    {
        //public FormConnection() { }
        //public FormConnection(string connString, int sgbd) {
        //    this.ConnString = connString;
        //    this.Sgbd = sgbd;
        //}
        public int Sgbd { get; set; }

        public string ConnString { get; set; }
    }
}
