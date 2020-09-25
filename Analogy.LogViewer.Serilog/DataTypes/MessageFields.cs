using System;
using System.Linq;

namespace Analogy.LogViewer.Serilog.DataTypes
{
    public interface IMessageFields
    {
        string Timestamp { get; }
        string MessageTemplate { get; }
        string Level { get; }
        string Exception { get; }
        string Renderings { get; }
        string EventId { get; }
        string Message { get; }
        string[] All { get; }
        string[] Required { get; }
        string Unescape(string name);
        bool IsUnrecognized(string name);
    }

    public class CompactJsonFormatMessageFields : IMessageFields
    {
        public string Timestamp { get; }
        public string MessageTemplate { get; }
        public string Level { get; }
        public string Exception { get; }
        public string Renderings { get; }
        public string EventId { get; }
        public string Message { get; }
        public string[] Required { get; }
        public string[] All { get; }
        string Prefix = "@";
        string EscapedInitialAt = "@@";

        public CompactJsonFormatMessageFields()
        {
            Timestamp = "@t";
            MessageTemplate = "@mt";
            Level = "@l";
            Exception = "@x";
            Renderings = "@r";
            EventId = "@i";
            Message = "@m";
            Prefix = "@";
            EscapedInitialAt = "@@";
            All = new[] { Timestamp, MessageTemplate, Level, Exception, Renderings, EventId, Message };
            Required = new[] { Timestamp, MessageTemplate, Level, Exception, Renderings };

        }
        public string Unescape(string name)
        {
            if (name.StartsWith(EscapedInitialAt))
                return name.Substring(1);

            return name;
        }

        public bool IsUnrecognized(string name)
        {
            return !name.StartsWith(EscapedInitialAt) &&
                   name.StartsWith(Prefix) &&
                   !All.Contains(name);
        }
    }


    public class JsonFormatMessageFields : IMessageFields
    {
        public string Timestamp { get; }
        public string MessageTemplate { get; }
        public string Level { get; }
        public string Exception { get; }
        public string Renderings { get; }
        public string EventId { get; }
        public string Message { get; }

        public string[] All { get; }
        public string[] Required { get; }
        string Prefix = "@";
        string EscapedInitialAt = "@@";

        public JsonFormatMessageFields()
        {
            Timestamp = "Timestamp";
            MessageTemplate = "MessageTemplate";
            Level = "Level";
            Exception = "Exception";
            Renderings = "RenderedMessage";
            EventId = "@i";
            Message = "@m";
            All = new[] { Timestamp, MessageTemplate, Level, Exception, Renderings, EventId, Message };
            Required = new[] { Timestamp, MessageTemplate, Level, Exception, Renderings };

        }
        public string Unescape(string name)
        {
            if (name.StartsWith(EscapedInitialAt))
                return name.Substring(1);

            return name;
        }

        public bool IsUnrecognized(string name)
        {
            return !name.StartsWith(EscapedInitialAt) &&
                   name.StartsWith(Prefix) &&
                   !All.Contains(name);
        }
    }


}

