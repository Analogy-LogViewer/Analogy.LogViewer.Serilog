using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Serilog.UnitTests
{
    [TestClass]
    public class ClefTests
    {
        [TestMethod]
        public async Task ClefParserTest()
        {
            ClefParser p = new ClefParser();
            CancellationTokenSource cts = new CancellationTokenSource();
            string fileName = @"example1.clef";
            MessageHandlerForTesting forTesting = new MessageHandlerForTesting();
            var messages = await p.Process(fileName, cts.Token, forTesting);
            Assert.IsTrue(messages.Count() == 4);
        }
    }
}