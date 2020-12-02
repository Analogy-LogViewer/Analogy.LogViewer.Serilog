using System;
using System.Collections.Generic;
using System.IO;
using Serilog.Events;

namespace Analogy.LogViewer.Serilog.DataTypes
{
    class RenderableScalarValue : ScalarValue
    {
        readonly Dictionary<string, string> _renderings = new Dictionary<string, string>();

        public RenderableScalarValue(object value, List<Rendering> renderings)
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

        public override void Render(TextWriter output, string format = null, IFormatProvider formatProvider = null)
        {
            string rendering;
            if (format != null && _renderings.TryGetValue(format, out rendering))
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