// Copyright 2013-2015 Serilog Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Linq;

namespace Analogy.LogViewer.Serilog.CompactClef
{
    static class ClefFields
    {
        public const string Timestamp = "@t";
        public const string MessageTemplate = "@mt";
        public const string Level = "@l";
        public const string Exception = "@x";
        public const string Renderings = "@r";
        public const string EventId = "@i";
        public const string Message = "@m";

        public static readonly string[] All = { Timestamp, MessageTemplate, Level, Exception, Renderings, EventId, Message };

        const string Prefix = "@";
        const string EscapedInitialAt = "@@";

        public static string Unescape(string name)
        {
            if (name.StartsWith(EscapedInitialAt))
                return name.Substring(1);

            return name;
        }

        public static bool IsUnrecognized(string name)
        {
            return !name.StartsWith(EscapedInitialAt) &&
                name.StartsWith(Prefix) &&
                !All.Contains(name);
        }
    }

}
