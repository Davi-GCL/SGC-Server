
namespace SGC.Domain.Entities;

public class Table
{
    public string? Name { get; set; }

    //public string? Type { get; set; }

    public string? Catalog { get; set; }

    public List<Column> Columns { get; set; }


}
