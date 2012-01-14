using System;
using System.Data;

namespace AD.Shared.Extensions
{
    public static class DataReaderExtensions
    {
        public static T GetValue<T>(this IDataReader reader, string name)
        {
            return (T)Convert.ChangeType(reader[name], typeof(T));
        }

        public static T GetValue<T>(this IDataReader reader, int position)
        {
            return (T)Convert.ChangeType(reader[position], typeof(T));
        }
    }
}
