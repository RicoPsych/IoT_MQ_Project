using backend.Entities;
using Backend.Filters;
using Backend.Services;
using System.Runtime.CompilerServices;
using System.Text;

namespace Backend.Utilities
{
    public static class CsvSerializer
    {
        public static string Serialize<T>(IEnumerable<T> enumerable)
        {
            return string.Join('\n', enumerable
                .Select(_object => Serialize(_object))
                .Prepend(SerializeHeader(typeof(T))));
        }

        private static string SerializeHeader(Type type)
        {
            return string.Join(',', type.GetProperties()
                .Select(property => $"\"{property.Name}\""));
        }
        public static string Serialize<T>(T obj)
        {
            return string.Join(',', typeof(T).GetProperties()
                .Select(property => $"\"{property.GetValue(obj)}\""));   
        }

        public static byte[] SerializeToUtf8Bytes<T>(IEnumerable<T> enumerable)
        {
            return Encoding.UTF8.GetBytes(Serialize(enumerable));
        }
    }
}
