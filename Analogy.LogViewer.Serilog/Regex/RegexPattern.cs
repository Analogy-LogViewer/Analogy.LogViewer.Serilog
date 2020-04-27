using System;

namespace Analogy.LogViewer.Serilog.Regex
{
    [Serializable]
    public class RegexPattern
    {
        public string Pattern { get; set; }
        public string DateTimeFormat { get; set; }
        public string GuidFormat { get; set; }
        public bool IsSet => !string.IsNullOrEmpty(Pattern) && !string.IsNullOrEmpty(DateTimeFormat);
        public RegexPattern()
        {
            Pattern = string.Empty;
            DateTimeFormat = "yyyy-MM-dd HH:mm:ss,fff";
            GuidFormat = string.Empty;

        }
        public RegexPattern(string pattern, string dateTimeFormat, string guidFormat)
        {
            Pattern = pattern;
            DateTimeFormat = dateTimeFormat;
            GuidFormat = guidFormat;
        }

        public override string ToString() => $"Pattern: {Pattern}, DateTimeFormat: {DateTimeFormat}, GuidFormat: {GuidFormat}";
    }
}
