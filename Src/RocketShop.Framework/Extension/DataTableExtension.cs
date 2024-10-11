using RocketShop.Framework.Attribute;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Extension
{
    public static class DataTableExtension
    {
        /// <summary>
        /// Converts an enumerable collection of items into a <see cref="DataTable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="items">The collection of items to convert. If null, an empty <see cref="DataTable"/> is returned.</param>
        /// <returns>A <see cref="DataTable"/> containing the data from the items in the collection.</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T>? items, string? tableName = null)
        {
            DataTable table = tableName.IsNullOrEmpty() ? new DataTable() : new DataTable(tableName);
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
                table.Columns.Add(property.Name);
            items.HasDataAndForEach(item =>
            {
                var data = new object?[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                    data[i] = properties[i].GetValue(item);

                table.Rows.Add(data);
            });
            return table;
        }
        public static DataTable ToDataTableWithNHAutoTableFormat<T>(this IEnumerable<T>? items, string? tableName = null)
        {
            DataTable table = tableName.IsNullOrEmpty() ? new DataTable() : new DataTable(tableName);
            var properties = typeof(T).GetProperties();
            int skiped = 0;
            foreach (var property in properties)
            {
                if (property.GetCustomAttribute(typeof(NHAutoTableColumnDisplay)).IsNotNull())
                {
                    var display = property.GetCustomAttribute(typeof(NHAutoTableColumnDisplay)) as NHAutoTableColumnDisplay;
                    table.Columns.Add(display!.ColumnDisplay);
                    continue;
                }
                if (property.GetCustomAttribute(typeof(NHAutoTableSkipColumn)).IsNotNull())
                {
                    skiped++;
                    continue;
                }
                table.Columns.Add(property.Name);
            }

            items.HasDataAndForEach(item =>
            {

                var data = new object?[properties.Length - skiped];
                int isSkip = 0;
                for (int i = 0; i < properties.Length; i++)
                {
                    var j = i - isSkip;
                    var property = properties[i];
                    if (property.GetCustomAttribute(typeof(NHAutoTableSkipColumn)).IsNotNull())
                    {
                        isSkip++;
                        continue;
                    }
                    var value = property.GetValue(item);
                    if (value.IsNotNull() && value!.GetType() == typeof(bool) && ((bool?)value).IsTrue() && property.GetCustomAttribute(typeof(NHAutoTableTrueDisplay)).IsNotNull())
                    {
                        var attr = property.GetCustomAttribute(typeof(NHAutoTableTrueDisplay)) as NHAutoTableTrueDisplay;
                        data[j] = attr?.Value.IfNull(value?.ToString() ?? string.Empty);
                    }
                    else if (value.IsNotNull() && value!.GetType() == typeof(bool) && ((bool?)value).IsFalse() && property.GetCustomAttribute(typeof(NHAutoTableFalseDisplay)).IsNotNull())
                    {
                        var attr = property.GetCustomAttribute(typeof(NHAutoTableFalseDisplay)) as NHAutoTableFalseDisplay;
                        data[j] = attr?.Value.IfNull(value?.ToString() ?? string.Empty);
                    }
                    else
                    {
                        if (value is null && property.GetCustomAttribute(typeof(NHAutoTableNullDisplay)) is not null)
                        {
                            var attr = property.GetCustomAttribute(typeof(NHAutoTableNullDisplay)) as NHAutoTableNullDisplay;
                            data[j] = attr?.Value.IfNull(value?.ToString() ?? string.Empty);
                        }
                        else if (value is not null && value.IsNumericType() && property.GetCustomAttribute(typeof(NHAutoTableNumberFormatDisplay)) is not null)
                        {
                            var attr = property.GetCustomAttribute(typeof(NHAutoTableNumberFormatDisplay)) as NHAutoTableNumberFormatDisplay;
                            var val = (decimal)value;
                            data[j] = val.ToString(attr!.FormatType);
                        }
                        else if(value is not null && value.GetType() == typeof(DateTime) && property.GetCustomAttribute(typeof(NHAutoTableDateTimeFormatDisplay)) is not null)
                        {
                            var attr = property.GetCustomAttribute(typeof(NHAutoTableDateTimeFormatDisplay)) as NHAutoTableDateTimeFormatDisplay;
                            var val = (DateTime)value;
                            data[j] = @val.ToString(attr!.FormatType);
                        }
                        else
                        {
                            data[j] = value;
                        }
                    }
                }


                table.Rows.Add(data);
            });
            return table;
        }

        /// <summary>
        /// Asynchronously converts an enumerable collection of items into a <see cref="DataTable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="items">The collection of items to convert.</param>
        /// <returns>A <see cref="Task{DataTable}"/> that represents the asynchronous operation, 
        /// containing the data from the items in the collection.</returns>
        public static async Task<DataTable> ToDataTableAsync<T>(this IEnumerable<T> items)
       => await Task.Factory.StartNew(() => ToDataTable(items));

        /// <summary>
        /// Converts each row of a <see cref="DataTable"/> into an instance of a specified type using a provided mapping function.
        /// </summary>
        /// <typeparam name="T">The type to convert each <see cref="DataRow"/> into.</typeparam>
        /// <param name="table">The <see cref="DataTable"/> to read data from.</param>
        /// <param name="reader">A function that maps each <see cref="DataRow"/> to an instance of type <typeparamref name="T"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing the converted instances.</returns>
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

        /// <summary>
        /// Asynchronously converts each row of a <see cref="DataTable"/> into an instance of a specified type 
        /// using a provided mapping function.
        /// </summary>
        /// <typeparam name="T">The type to convert each <see cref="DataRow"/> into.</typeparam>
        /// <param name="table">The <see cref="DataTable"/> to read data from.</param>
        /// <param name="reader">A function that maps each <see cref="DataRow"/> to an instance of type <typeparamref name="T"/>.</param>
        /// <returns>A <see cref="Task{IEnumerable{T}}"/> that represents the asynchronous operation, 
        /// containing the converted instances.</returns>
        public static Task<IEnumerable<T>> ReadAsync<T>(this DataTable table, Func<DataRow, T> reader) =>
            Task.Factory.StartNew(() => table.Read(reader));

        /// <summary>
        /// Automatically maps each row of a <see cref="DataTable"/> to an instance of a specified type, 
        /// using column names to match properties on the type. Optionally, specific properties can be skipped during the mapping.
        /// </summary>
        /// <typeparam name="T">The type to convert each <see cref="DataRow"/> into. The type must have a parameterless constructor.</typeparam>
        /// <param name="table">The <see cref="DataTable"/> to read data from.</param>
        /// <param name="skipProperties">An optional array of property names to skip during the mapping process.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing the mapped instances.</returns>
        public static IEnumerable<T> AutoMap<T>(this DataTable table, params string[] skipProperties) where T : new()
        {
            var properties = typeof(T).GetProperties();
            var hasSkipProp = skipProperties.HasData();
            return table.Read(row =>
            {
                T item = new();
                foreach (var property in properties)
                {
                    if (hasSkipProp)
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
        /// <summary>
        /// Converts each row of a <see cref="DataTable"/> into an instance of a specified type using default property mapping.
        /// </summary>
        /// <typeparam name="T">The type to convert each <see cref="DataRow"/> into. The type must have a parameterless constructor and public properties that match the column names.</typeparam>
        /// <param name="table">The <see cref="DataTable"/> to read data from.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing the instances created from the rows of the <see cref="DataTable"/>.</returns>
        public static IEnumerable<T> ToEnumerable<T>(this DataTable table)
        {
            var type = typeof(T);
            var proprties = type.GetProperties();
            List<T> values = new List<T>();
            var columns = GetColumnNames(table);
            foreach (DataRow item in table.Rows)
            {
                T it = Activator.CreateInstance<T>();
                foreach (var prop in proprties)
                {
                    var propName = prop.Name;
                    var isContained = columns.Where(x => x.ToLower() == propName.ToLower()).HasData();
                    if (!isContained)
                        continue;
                    object? value = item[propName];
                    if (value == null)
                        continue;
                    var ittype = type.GetType();
                    if (ittype == typeof(bool) && !(value is bool))
                    {
                        value = value.Equals(1);
                    }
                    if (value.GetType() == typeof(string))
                    {
                        string? val = value?.ToString();
                        if (string.IsNullOrEmpty(val))
                            continue;
                        if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
                            prop.SetValue(it, int.Parse(val));
                        else if (prop.PropertyType == typeof(long) || prop.PropertyType == typeof(long?))
                            prop.SetValue(it, long.Parse(val));
                        else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                            prop.SetValue(it, decimal.Parse(val));
                        else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
                            prop.SetValue(it, double.Parse(val));
                        else if (prop.PropertyType == typeof(float) || prop.PropertyType == typeof(float?))
                            prop.SetValue(it, float.Parse(val));
                        else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                            prop.SetValue(it, DateTime.Parse(val));

                        else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
                        {
                            var isBool = bool.TryParse(val, out var result);
                            prop.SetValue(it, isBool ? result : val.Equals(1));
                        }
                        else
                        {
                            prop.SetValue(it, val);
                        }
                    }
                    else if (value.GetType() == typeof(DBNull))
                    {
                        prop.SetValue(it, string.Empty);
                    }
                    else
                    {
                        prop.SetValue(it, value);
                    }
                }
                values.Add(it);
            }
            return values;
        }
        static string[] GetColumnNames(DataTable dataTable)
        {
            string[] columnNames = new string[dataTable.Columns.Count];

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                columnNames[i] = dataTable.Columns[i].ColumnName;
            }

            return columnNames;
        }
    }
}
