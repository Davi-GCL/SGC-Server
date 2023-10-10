
namespace SGC_Server.Model
{
    public class FormTables
    {
        public string ConnString { get; set; }

        public int Sgbd { get; set; }

        public string DataBaseName { get; set; }

        public string Namespace { get; set; }

        public List<string> SelectedTables { get; set; }
    }
}
