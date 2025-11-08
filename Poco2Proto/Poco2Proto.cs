using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Poco2Proto
{
    /// <summary>
    /// A wizard for generating Protobuf message definitions from C# POCO classes.
    /// </summary>
    public static class Poco2Proto
    {
        /// <summary>
        /// Generates a .proto message definition string from a given C# type.
        /// </summary>
        /// <typeparam name="T">The C# class to convert.</typeparam>
        /// <returns>A string containing the generated .proto content.</returns>
        public static string GenerateProto<T>()
        {
            var type = typeof(T);
            var protoBuilder = new StringBuilder();

            protoBuilder.AppendLine($"syntax = \"proto3\";");
            protoBuilder.AppendLine($"package {type.Namespace};");
            protoBuilder.AppendLine();
            protoBuilder.AppendLine($"message {type.Name} {{");

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            int fieldNumber = 1;
            foreach (var prop in properties)
            {
                var protoType = MapToProtoType(prop.PropertyType);
                string fieldName = ToSnakeCase(prop.Name);
                protoBuilder.AppendLine($"  {protoType} {fieldName} = {fieldNumber};");
                fieldNumber++;
            }

            protoBuilder.AppendLine("}");
            return protoBuilder.ToString();
        }

        /// <summary>
        /// Converts a PascalCase string to snake_case.
        /// </summary>
        private static string ToSnakeCase(string pascalCaseString)
        {
            if (string.IsNullOrEmpty(pascalCaseString)) return pascalCaseString;

            // Example: "FullName" -> "full_name"
            return Regex.Replace(pascalCaseString, "(?<!^)([A-Z])", "_$1").ToLower();
        }

        /// <summary>
        /// Maps a C# type to a Protobuf type.
        /// </summary>
        /// <param name="type">The C# type to map.</param>
        /// <returns>The corresponding Protobuf type as a string.</returns>
        private static string MapToProtoType(Type type)
        {
            Type underlyingType = Nullable.GetUnderlyingType(type) ?? type;

            // 2. Handle collections (List<T>, string[], IEnumerable<T>)
            if (underlyingType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(underlyingType))
            {
                // Get the inner type (e.g., the 'string' from List<string>)
                Type itemType = underlyingType.IsArray
                    ? underlyingType.GetElementType()
                    : underlyingType.GetGenericArguments().FirstOrDefault();

                if (itemType != null)
                {
                    // Recursively map the inner type
                    string innerProtoType = MapToProtoType(itemType);
                    return $"repeated {innerProtoType}";
                }
            }
            if (underlyingType == typeof(string)) return "string";
            if (underlyingType == typeof(int) || underlyingType == typeof(Int32)) return "int32";
            if (underlyingType == typeof(long) || underlyingType == typeof(Int64)) return "int64";
            if (underlyingType == typeof(bool)) return "bool";
            if (underlyingType == typeof(float)) return "float";
            if (underlyingType == typeof(double)) return "double";
            if (underlyingType == typeof(byte[])) return "bytes";

            // map DateTime to string for simplicity
            if (underlyingType == typeof(DateTime)) return "string";

            // 4. Handle nested classes (basic support)
            if (underlyingType.IsClass)
            {
                return underlyingType.Name;
            }

            throw new NotSupportedException($"C# type {type.Name} is not supported.");
        }
    }
}
