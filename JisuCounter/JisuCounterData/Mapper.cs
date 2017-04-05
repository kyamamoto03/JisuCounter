using System;
using System.Data.Common;

namespace JisuCounterData
{
    public class Mapper<T> where T: new()
    {
        /// <summary>
        /// データベースのレコードとクラスをマッピングする
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public T Mapping(DbDataReader dr)
        {
            Type putClass = typeof(T);
            //データを格納するためクラスをインスタンス化する
            object data = putClass.InvokeMember(null, System.Reflection.BindingFlags.CreateInstance, null, null, null);

            for (int index = 0; index < dr.FieldCount; index++)
            {
                if (dr.IsDBNull(index) == false)
                {
                    string ColumName = dr.GetName(index);
                    var propertyInfo = typeof(T).GetProperty(ColumName);
                    if (propertyInfo == null)
                    {
                        //プロパティが見つからない場合はColumnNameAttributeで検索する
                        var CustomProperties = typeof(T).GetProperties();
                        foreach (var CustomPropery in CustomProperties)
                        {
                            ColumnNameAttribute[] properties = (ColumnNameAttribute[])CustomPropery.GetCustomAttributes(typeof(ColumnNameAttribute), true);
                            foreach (ColumnNameAttribute columnNameAttribute in properties)
                            {
                                if (columnNameAttribute._ColumName == ColumName)
                                {
                                    propertyInfo = CustomPropery;
                                    ColumName = CustomPropery.Name;
                                    break;
                                }
                            }
                        }
                    }
                    if (propertyInfo != null)
                    {
                        if (propertyInfo.PropertyType == typeof(string))
                        {
                            putClass.InvokeMember(ColumName, System.Reflection.BindingFlags.SetProperty, null, data, new object[] { dr.GetString(index) });
                        }
                        else if (propertyInfo.PropertyType == typeof(long))
                        {
                            putClass.InvokeMember(ColumName, System.Reflection.BindingFlags.SetProperty, null, data, new object[] { dr.GetInt64(index) });
                        }
                        else if (propertyInfo.PropertyType == typeof(int))
                        {
                            putClass.InvokeMember(ColumName, System.Reflection.BindingFlags.SetProperty, null, data, new object[] { dr.GetInt32(index) });
                        }
                        else if (propertyInfo.PropertyType == typeof(DateTime))
                        {
                            putClass.InvokeMember(ColumName, System.Reflection.BindingFlags.SetProperty, null, data, new object[] { dr.GetDateTime(index) });
                        }
                        else if (propertyInfo.PropertyType == typeof(double))
                        {
                            putClass.InvokeMember(ColumName, System.Reflection.BindingFlags.SetProperty, null, data, new object[] { dr.GetDouble(index) });
                        }
                    }
                }
            }

            return (T)data;
        }
    }
}
