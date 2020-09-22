using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Analogy.LogViewer.Serilog.DataTypes;

namespace Analogy.LogViewer.Serilog.UnitTests
{
    [TestClass]
    public class JsonParserTests
    {
        private string folder { get; } = Environment.CurrentDirectory;

        [TestMethod]
        public async Task JsonFileParserWithCompactFormatTest()
        {
            var p = new JsonFileParser(new CompactJsonFormatMessageFields());
            CancellationTokenSource cts = new CancellationTokenSource();
            string fileName = Path.Combine(folder, "log files", "JsonFile.clef");
            MessageHandlerForTesting forTesting = new MessageHandlerForTesting();
            var messages = await p.Process(fileName, cts.Token, forTesting);
            Assert.IsTrue(messages.Count() == 2);
        }
    }
}
