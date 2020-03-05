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

using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;

namespace Analogy.LogViewer.Serilog.CompactClef
{
    class RenderableScalarValue : ScalarValue
    {
        readonly Dictionary<string, string> _renderings = new Dictionary<string, string>();

        public RenderableScalarValue(object value, IEnumerable<Rendering> renderings)
            : base(value)
        {
            if (renderings == null) throw new ArgumentNullException(nameof(renderings));
            foreach (var rendering in renderings)
                _renderings[rendering.Format] = rendering.Rendered;
        }

        public override void Render(TextWriter output, string format = null, IFormatProvider formatProvider = null)
        {
            string rendering;
            if (format != null && _renderings.TryGetValue(format, out rendering))
                output.Write(rendering);
            else
                base.Render(output, format, formatProvider);
        }
    }
}