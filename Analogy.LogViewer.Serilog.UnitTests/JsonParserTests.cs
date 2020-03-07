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
        public async Task ClefParserTest()
        {
            var p = new JsonParser();
            CancellationTokenSource cts = new CancellationTokenSource();
            string fileName = @"testJson.clef";
            MessageHandlerForTesting forTesting = new MessageHandlerForTesting();
            var messages = await p.Process(fileName, cts.Token, forTesting);
            Assert.IsTrue(messages.Count() == 2);
        }
    }
}
