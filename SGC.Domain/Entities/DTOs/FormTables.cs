namespace SGC.Domain.Entities.DTOs
{
    public class FormTables
    {
        public int Sgbd { get; set; }
        
        public string ConnString { get; set; }

        //public string DataBaseName { get; set; }

        public string Namespace { get; set; }

        public List<string> SelectedTablesNames { get; set; }
    }
}
