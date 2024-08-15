using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Extension
{
    public static class DataTableExtension
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> items)
        {
            DataTable table = new DataTable();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
                table.Columns.Add(property.Name);
            foreach (var item in items)
            {
                var data = new object?[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                    data[i] = properties[i].GetValue(item);

                table.Rows.Add(data);
            }
            return table;
        }

        public static async Task< DataTable> ToDataTableAsync<T>(this IEnumerable<T> items)
       => await Task.Factory.StartNew(() => ToDataTable(items));

        public static IEnumerable<T> Read<T>(this DataTable table, Func<DataRow, T> reader)
        {
            List<T> list = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                var item = reader(row);
                list.Add(item);
            }
            return list;
        }

        public static Task<IEnumerable<T>> ReadAsync<T>(this DataTable table, Func<DataRow, T> reader) =>
            Task.Factory.StartNew(() => table.Read(reader));

        public static IEnumerable<T> AutoMap<T>(this DataTable table,params string[] skipProperties) where T : new()
        {
        var properties = typeof(T).GetProperties();
            var hasSkipProp = skipProperties.HasData();
            return table.Read(row =>
            {
                T item = new();
                foreach(var property in properties)
                {
                    if(hasSkipProp)
                    {
                        var isYes = skipProperties.Where(x => x == property.Name).HasData();
                        if (isYes)
                            continue;
                    }
                    property.SetValue(item, row[property.Name]);
                }
                return item;
            });
        }

    }
}
