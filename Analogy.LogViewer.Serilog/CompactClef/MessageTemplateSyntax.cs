using System;

namespace Analogy.LogViewer.Serilog.CompactClef
{
    static class MessageTemplateSyntax
    {
        public static string Escape(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            return text.Replace("{", "{{").Replace("}", "}}");
        }
    }
}
