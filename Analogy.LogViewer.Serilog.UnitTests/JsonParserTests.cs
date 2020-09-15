using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Serilog.UnitTests
{
    [TestClass]
    public class JsonParserTests
    {
        [TestMethod]
        public async Task ClefJsonParserTest()
        {
            var p = new JsonFormatterParser();
            CancellationTokenSource cts = new CancellationTokenSource();
            string fileName = @"Analogy.Logserver.20200913.log";
            MessageHandlerForTesting forTesting = new MessageHandlerForTesting();
            var messages = await p.Process(fileName, cts.Token, forTesting);
            Assert.IsTrue(messages.Count() == 2);
        }
    }
}
