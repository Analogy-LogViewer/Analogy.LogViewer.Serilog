using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Serilog.UnitTests
{
    [TestClass]
    public class JsonParserTests
    {
        private string folder { get; } = Environment.CurrentDirectory;

        [TestMethod]
        public async Task ClefJsonParserTest()
        {
            var p = new ClefParser();
            CancellationTokenSource cts = new CancellationTokenSource();
            string fileName = Path.Combine(folder, "log files", "SourceContextTest.clef");
            MessageHandlerForTesting forTesting = new MessageHandlerForTesting();
            var messages = await p.Process(fileName, cts.Token, forTesting);
            Assert.IsTrue(messages.Count() == 2);
        }
    }
}
