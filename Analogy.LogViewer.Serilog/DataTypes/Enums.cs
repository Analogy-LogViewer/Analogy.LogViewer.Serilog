namespace Analogy.LogViewer.Serilog.DataTypes
{
    public enum FileFormat
    {
        Unknown,
        CompactJsonFormatPerLine,
        JsonFormatPerLine,
        CompactJsonFormatPerFile,
        JsonFormatFile,
    }

    public enum FileFormatDetection
    {
        Automatic,
        Manual,
    }
}