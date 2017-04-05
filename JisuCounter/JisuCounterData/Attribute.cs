using System;


namespace JisuCounterData
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnNameAttribute : Attribute
    {
        public  string _ColumName;
        public ColumnNameAttribute(string columnName)
        {
            _ColumName = columnName;
        }
    }
}
