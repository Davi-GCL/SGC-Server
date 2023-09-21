namespace SGC.Domain.Entities;

public class Column
{

    public string TableName { get; set; }

    public string Name { get; set; }

    public bool IsPrimaryKey { get; set; }

    public bool IsNullable { get; set; }

    public string Type { get; set; }

    public int? CharMaxLength { get; set; }
}
