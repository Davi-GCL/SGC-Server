namespace SGC.Infrastructure.Model
{
    public class Column
    {
        public string TableName { get; set; }

        public string Name { get; set; }

        public bool IsPrimaryKey { get; set; }

        public bool IsNullable { get; set; }

        public String Type { get; set; }

        public int? CharMaxLength { get; set; }
    }
}
