using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Serilog.UnitTests
{
    [TestClass]
    public class ClefTests
    {
        private string folder { get; } = Environment.CurrentDirectory;
        [TestMethod]
        public async Task ClefParserTest()
        {
            ClefParser p = new ClefParser();
            CancellationTokenSource cts = new CancellationTokenSource();
            string fileName = Path.Combine(folder, "log files", "ClefExample1.clef");
            MessageHandlerForTesting forTesting = new MessageHandlerForTesting();
            var messages = await p.Process(fileName, cts.Token, forTesting);
            Assert.IsTrue(messages.Count() == 4);
        }

        // Test reading the (optional) source context
        [TestMethod]
        public async Task SourceContextTest()
        {
            ClefParser p = new ClefParser();
            CancellationTokenSource cts = new CancellationTokenSource();
            string fileName = Path.Combine(folder, "log files", "SourceContextTest.clef");
            MessageHandlerForTesting forTesting = new MessageHandlerForTesting();

            var messages = (await p.Process(fileName, cts.Token, forTesting)).ToList();

            Assert.AreEqual(2, messages.Count());

            // The first event doesn't have a source context
            var firstEvent = messages.ElementAt(0);
            Assert.AreEqual("Hello, Serilog!", firstEvent.Text);
            Assert.AreEqual(string.Empty, firstEvent.Source);
            Assert.AreEqual(1, firstEvent.ThreadId);
            Assert.IsNotNull(firstEvent.Module);
            Assert.IsNotNull(firstEvent.FileName);
            Assert.IsNotNull(firstEvent.Category);
            Assert.IsNotNull(firstEvent.User);
            Assert.IsNotNull(firstEvent.MethodName);
            // The second event should have a source context
            var secondEvent = messages.ElementAt(1);
            Assert.AreEqual("Contextual Log", secondEvent.Text);
            Assert.AreEqual("SerilogLogging.Program", secondEvent.Source);
            Assert.AreEqual(1, secondEvent.ThreadId);
            Assert.IsNotNull(secondEvent.Module);
            Assert.IsNotNull(secondEvent.FileName);
            Assert.IsNotNull(secondEvent.Category);
            Assert.IsNotNull(secondEvent.User);
            Assert.IsNotNull(secondEvent.MethodName);
        }
    }
}