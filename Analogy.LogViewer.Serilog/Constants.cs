using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Analogy.LogViewer.Serilog
{
    public static class Constants
    {
        public static string Source { get; } = global::Serilog.Core.Constants.SourceContextPropertyName;
        public static string ThreadId { get; } = "ThreadId";

        public static string ProcessId { get; } = "ProcessId";
        public static string ProcessName { get; } = "ProcessName";
        public static string MachineName { get; } = "MachineName";
        public static string User { get; } = "User";
        public static string EnvironmentUserName { get; } = "EnvironmentUserName";

        public static Dictionary<string, string> GetFieldValues()
        {
            return typeof(Constants)
                      .GetFields(BindingFlags.Public | BindingFlags.Static)
                      .Where(f => f.FieldType == typeof(string))
                      .ToDictionary(f => f.Name, f => (string)f.GetValue(null));
        }
    }
}