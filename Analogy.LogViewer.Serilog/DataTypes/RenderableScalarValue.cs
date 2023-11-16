using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;

namespace Analogy.LogViewer.Serilog.DataTypes
{
    internal class RenderableScalarValue : ScalarValue
    {
       private readonly Dictionary<string, string> _renderings = new Dictionary<string, string>();

        public RenderableScalarValue(object? value, List<Rendering> renderings)
            : base(value)
        {
            if (renderings == null)
            {
                throw new ArgumentNullException(nameof(renderings));
            }

            foreach (var rendering in renderings)
            {
                _renderings[rendering.Format] = rendering.Rendered;
            }
        }

        public override void Render(TextWriter output, string? format = null, IFormatProvider? formatProvider = null)
        {
            if (format != null && _renderings.TryGetValue(format, out var rendering))
            {
                output.Write(rendering);
            }
            else
            {
                base.Render(output, format, formatProvider);
            }
        }
    }
}