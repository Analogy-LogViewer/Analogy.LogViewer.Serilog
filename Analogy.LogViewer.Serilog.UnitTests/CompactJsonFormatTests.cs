using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Serilog.UnitTests
{
    [TestClass]
    public class CompactJsonFormatTests
    {
        private string Folder { get; } = Environment.CurrentDirectory;
        [TestMethod]
        public async Task CompactJsonFormatParserTest()
        {
            CompactJsonFormatParser parser = new CompactJsonFormatParser();
            CancellationTokenSource cts = new CancellationTokenSource();
            string fileName = Path.Combine(Folder, "log files", "CompactJsonFormat.clef");
            MessageHandlerForTesting forTesting = new MessageHandlerForTesting();
            var messages = (await parser.Process(fileName, cts.Token, forTesting)).ToList();
            Assert.IsTrue(messages.Count == 4);
            Assert.IsTrue(messages[0].Text == "Hello, { Name: \"nblumhardt\", Id: 101 }");
        }

        // Test reading the (optional) source context
        [TestMethod]
        public async Task CompactJsonFormatSourceContextTest()
        {
            CompactJsonFormatParser parser = new CompactJsonFormatParser();
            CancellationTokenSource cts = new CancellationTokenSource();
            string fileName = Path.Combine(Folder, "log files", "CompactJsonFormatSourceContextTest.clef");
            MessageHandlerForTesting forTesting = new MessageHandlerForTesting();

            var messages = (await parser.Process(fileName, cts.Token, forTesting)).ToList();

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


        [TestMethod]
        public async Task CompactJsonFormatTestColumns()
        {
            CompactJsonFormatParser parser = new CompactJsonFormatParser();
            CancellationTokenSource cts = new CancellationTokenSource();
            string fileName = Path.Combine(Folder, "log files", "CompactJsonFormatTestColumns.clef");
            MessageHandlerForTesting forTesting = new MessageHandlerForTesting();
            var messages = (await parser.Process(fileName, cts.Token, forTesting)).ToList();
            Assert.AreEqual(4, messages.Count());
            // The first event doesn't have a source context
            Assert.IsTrue(messages[0].MachineName == "Test");
            Assert.IsTrue(messages[1].AdditionalInformation["CustomProperty"] == "\"Custom Value\"");

        }

        [TestMethod]
        public async Task CompactJsonFormatTestGZFile()
        {
            CompactJsonFormatParser parser = new CompactJsonFormatParser();
            CancellationTokenSource cts = new CancellationTokenSource();
            string fileName = Path.Combine(Folder, "log files", "CompactJsonFormat.gz");
            MessageHandlerForTesting forTesting = new MessageHandlerForTesting();
            var messages = (await parser.Process(fileName, cts.Token, forTesting)).ToList();
            Assert.AreEqual(4, messages.Count());
            // The first event doesn't have a source context
            Assert.IsTrue(messages[2].AdditionalInformation["Tags"] == "[\"test\", \"orange\"]");

        }
    }
}