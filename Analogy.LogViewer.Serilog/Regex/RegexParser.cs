using Analogy.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Serilog.Regex
{
    public class RegexParser
    {
        private AnalogyLogMessage _current;
        private RegexPattern _lastUsedPattern;
        private readonly List<AnalogyLogMessage> _messages = new List<AnalogyLogMessage>();
        private readonly List<RegexPattern> _logPatterns;
        private readonly bool updateUIAfterEachParsedLine;
        private IAnalogyLogger Logger { get; }

        private IEnumerable<RegexPattern> LogPatterns
        {
            get
            {
                if (_lastUsedPattern != null)
                    yield return _lastUsedPattern;
                var oldLastUsedPattern = _lastUsedPattern;
                foreach (var logPattern in _logPatterns)
                {
                    //skip last used pattern (returned first)
                    if (oldLastUsedPattern == logPattern) continue;
                    _lastUsedPattern = logPattern;
                    yield return _lastUsedPattern;
                }
            }
        }

        public static IEnumerable<string> RegexMembers { get; }
        private static Dictionary<string, AnalogyLogMessagePropertyName> regexMapper;

        static RegexParser()
        {
            var names = Enum.GetNames(typeof(AnalogyLogMessagePropertyName));
            RegexMembers = names;
            regexMapper = new Dictionary<string, AnalogyLogMessagePropertyName>(names.Length);
            foreach (var name in names)
            {
                var enumValue = (AnalogyLogMessagePropertyName)Enum.Parse(typeof(AnalogyLogMessagePropertyName), name);
                regexMapper.Add(name, enumValue);
            }
        }

        public RegexParser(List<RegexPattern> logPatterns, bool updateUIAfterEachLine, IAnalogyLogger logger)
        {
            _logPatterns = logPatterns;
            Logger = logger;
            updateUIAfterEachParsedLine = updateUIAfterEachLine;

        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryParse(string line, RegexPattern regex, out AnalogyLogMessage message)
        {
            try
            {
                Match match = System.Text.RegularExpressions.Regex.Match(line, regex.Pattern,
                    RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
                if (match.Success)
                {
                    var m = new AnalogyLogMessage();
                    foreach (var regexMember in regexMapper)
                    {
                        string value = match.Groups[regexMember.Key].Success
                            ? match.Groups[regexMember.Key].Value
                            : string.Empty;
                        switch (regexMember.Value)
                        {
                            case AnalogyLogMessagePropertyName.Date:
                                if (!string.IsNullOrEmpty(value) &&
                                    DateTime.TryParseExact(value, regex.DateTimeFormat, CultureInfo.InvariantCulture,
                                        DateTimeStyles.None, out var date))
                                {
                                    m.Date = date;
                                }

                                continue;
                            case AnalogyLogMessagePropertyName.ID:
                                if (!string.IsNullOrEmpty(value) &&
                                    Guid.TryParseExact(value, regex.GuidFormat, out var guidValue))
                                {
                                    m.ID = guidValue;
                                }

                                continue;
                            case AnalogyLogMessagePropertyName.Text:
                                m.Text = value;
                                continue;
                            case AnalogyLogMessagePropertyName.Category:
                                m.Category = value;
                                continue;
                            case AnalogyLogMessagePropertyName.Source:
                                m.Source = value;
                                continue;
                            case AnalogyLogMessagePropertyName.Module:
                                m.Module = value;
                                continue;
                            case AnalogyLogMessagePropertyName.MethodName:
                                m.MethodName = value;
                                continue;
                            case AnalogyLogMessagePropertyName.FileName:
                                m.FileName = value;
                                continue;
                            case AnalogyLogMessagePropertyName.User:
                                m.User = value;
                                continue;
                            case AnalogyLogMessagePropertyName.LineNumber:
                                if (!string.IsNullOrEmpty(value) &&
                                    int.TryParse(value, out var lineNum))
                                {
                                    m.LineNumber = lineNum;
                                }

                                continue;
                            case AnalogyLogMessagePropertyName.ProcessID:
                                if (!string.IsNullOrEmpty(value) &&
                                    int.TryParse(value, out var processNum))
                                {
                                    m.ProcessID = processNum;
                                }

                                continue;
                            case AnalogyLogMessagePropertyName.Thread:
                                if (!string.IsNullOrEmpty(value) &&
                                    int.TryParse(value, out var threadNum))
                                {
                                    m.Thread = threadNum;
                                }

                                continue;
                            case AnalogyLogMessagePropertyName.Level:
                                switch (value)
                                {
                                    case "OFF":
                                        m.Level = AnalogyLogLevel.Disabled;
                                        break;
                                    case "TRACE":
                                        m.Level = AnalogyLogLevel.Trace;
                                        break;
                                    case "DEBUG":
                                        m.Level = AnalogyLogLevel.Debug;
                                        break;
                                    case "INFO":
                                        m.Level = AnalogyLogLevel.Event;
                                        break;
                                    case "WARN":
                                        m.Level = AnalogyLogLevel.Warning;
                                        break;
                                    case "ERROR":
                                        m.Level = AnalogyLogLevel.Error;
                                        break;
                                    case "FATAL":
                                        m.Level = AnalogyLogLevel.Critical;
                                        break;
                                    default:
                                        m.Level = AnalogyLogLevel.Unknown;
                                        break;
                                }

                                continue;
                            case AnalogyLogMessagePropertyName.Class:
                                if (string.IsNullOrEmpty(value))
                                    m.Class = AnalogyLogClass.General;
                                else
                                {
                                    m.Class = Enum.TryParse(value, true, out AnalogyLogClass cls) &&
                                              Enum.IsDefined(typeof(AnalogyLogClass), cls)
                                        ? cls
                                        : AnalogyLogClass.General;

                                }

                                continue;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    message = m;
                    return true;
                }

                message = null;
                return false;
            }
            catch (Exception e)
            {
                string error = $"Error parsing line: {e.Message}";
                Logger?.LogException(e, nameof(RegexParser), error);
                message = new AnalogyLogMessage(error, AnalogyLogLevel.Error, AnalogyLogClass.General,
                    nameof(RegexParser));
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckRegex(string line, RegexPattern regex, out AnalogyLogMessage message)
        {
            try
            {
                Match match = System.Text.RegularExpressions.Regex.Match(line, regex.Pattern,
                    RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
                if (match.Success)
                {
                    var m = new AnalogyLogMessage();
                    foreach (var regexMember in regexMapper)
                    {
                        string value = match.Groups[regexMember.Key].Success
                            ? match.Groups[regexMember.Key].Value
                            : string.Empty;
                        switch (regexMember.Value)
                        {
                            case AnalogyLogMessagePropertyName.Date:
                                if (!string.IsNullOrEmpty(value) &&
                                    DateTime.TryParseExact(value, regex.DateTimeFormat, CultureInfo.InvariantCulture,
                                        DateTimeStyles.None, out var date))
                                {
                                    m.Date = date;
                                }

                                break;
                            case AnalogyLogMessagePropertyName.ID:
                                if (!string.IsNullOrEmpty(value) &&
                                    Guid.TryParseExact(value, regex.GuidFormat, out var guidValue))
                                {
                                    m.ID = guidValue;
                                }

                                break;
                            case AnalogyLogMessagePropertyName.Text:
                                m.Text = value;
                                break;
                            case AnalogyLogMessagePropertyName.Category:
                                m.Category = value;
                                break;
                            case AnalogyLogMessagePropertyName.Source:
                                m.Source = value;
                                break;
                            case AnalogyLogMessagePropertyName.Module:
                                m.Module = value;
                                break;
                            case AnalogyLogMessagePropertyName.MethodName:
                                m.MethodName = value;
                                break;
                            case AnalogyLogMessagePropertyName.FileName:
                                m.FileName = value;
                                break;
                            case AnalogyLogMessagePropertyName.User:
                                m.User = value;
                                break;
                            case AnalogyLogMessagePropertyName.LineNumber:
                                if (!string.IsNullOrEmpty(value) &&
                                    int.TryParse(value, out var lineNum))
                                {
                                    m.LineNumber = lineNum;
                                }

                                break;
                            case AnalogyLogMessagePropertyName.ProcessID:
                                if (!string.IsNullOrEmpty(value) &&
                                    int.TryParse(value, out var processNum))
                                {
                                    m.ProcessID = processNum;
                                }

                                break;
                            case AnalogyLogMessagePropertyName.Thread:
                                if (!string.IsNullOrEmpty(value) &&
                                    int.TryParse(value, out var threadNum))
                                {
                                    m.Thread = threadNum;
                                }

                                break;
                            case AnalogyLogMessagePropertyName.Level:
                                switch (value)
                                {
                                    case "OFF":
                                        m.Level = AnalogyLogLevel.Disabled;
                                        break;
                                    case "TRACE":
                                        m.Level = AnalogyLogLevel.Trace;
                                        break;
                                    case "DEBUG":
                                        m.Level = AnalogyLogLevel.Debug;
                                        break;
                                    case "INFO":
                                        m.Level = AnalogyLogLevel.Event;
                                        break;
                                    case "WARN":
                                        m.Level = AnalogyLogLevel.Warning;
                                        break;
                                    case "ERROR":
                                        m.Level = AnalogyLogLevel.Error;
                                        break;
                                    case "FATAL":
                                        m.Level = AnalogyLogLevel.Critical;
                                        break;
                                    default:
                                        m.Level = AnalogyLogLevel.Unknown;
                                        break;
                                }

                                break;
                            case AnalogyLogMessagePropertyName.Class:
                                if (string.IsNullOrEmpty(value))
                                    m.Class = AnalogyLogClass.General;
                                else
                                {
                                    m.Class = Enum.TryParse(value, true, out AnalogyLogClass cls) &&
                                              Enum.IsDefined(typeof(AnalogyLogClass), cls)
                                        ? cls
                                        : AnalogyLogClass.General;

                                }

                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    message = m;
                    return true;
                }

                message = null;
                return false;
            }
            catch (Exception e)
            {
                string error = $"Error parsing line: {e.Message}";
                message = new AnalogyLogMessage(error, AnalogyLogLevel.Error, AnalogyLogClass.General,
                    nameof(RegexParser));
                return false;
            }
        }

        public async Task<List<AnalogyLogMessage>> ParseLog(string fileName, CancellationToken token,
            ILogMessageCreatedHandler messagesHandler)
        {
            _messages.Clear();
            using (StreamReader reader = File.OpenText(fileName))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    AnalogyLogMessage entry = null;
                    foreach (var logPattern in LogPatterns)
                    {
                        if (TryParse(line, logPattern, out entry))
                        {
                            break;
                        }
                    }

                    if (entry != null)
                    {
                        if (updateUIAfterEachParsedLine)
                            messagesHandler.AppendMessage(entry, fileName);
                        _current = entry;
                        _messages.Add(_current);
                    }
                    else
                    {
                        if (_current == null)
                        {
                            _current = new AnalogyLogMessage { Text = line };
                        }
                        else
                        {
                            _current.Text += Environment.NewLine + line;
                        }
                    }

                    if (token.IsCancellationRequested)
                    {
                        messagesHandler.AppendMessages(_messages, fileName);
                        return _messages;
                    }
                }
            }

            if (!updateUIAfterEachParsedLine) //update only at the end
                messagesHandler.AppendMessages(_messages, fileName);
            return _messages;
        }

    }
}
