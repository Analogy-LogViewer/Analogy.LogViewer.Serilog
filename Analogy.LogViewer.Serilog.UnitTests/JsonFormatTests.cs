using Analogy.LogViewer.Serilog.DataTypes;
using Analogy.LogViewer.Serilog.IAnalogy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Serilog.UnitTests
{
    [TestClass]
    public class JsonFormatTests
    {
        private string Folder { get; } = Environment.CurrentDirectory;

        [TestMethod]
        public async Task CompactJsonFormatFullFileTest()
        {
            JsonFileParser parser = new JsonFileParser(new CompactJsonFormatMessageFields());
            CancellationTokenSource cts = new CancellationTokenSource();
            string fileName = Path.Combine(Folder, "log files", "JsonFileCompactFormat.clef");
            MessageHandlerForTesting forTesting = new MessageHandlerForTesting();
            var messages = (await parser.Process(fileName, cts.Token, forTesting)).ToList();
            Assert.IsTrue(messages.Count == 2);
            Assert.IsTrue(messages[0].MachineName == "MY-MACHINE");
            Assert.IsTrue(messages[1].Text.StartsWith("An unknown error occurred"));
            Assert.IsTrue((messages[1].Module == "My process"));

        }

        [TestMethod]
        public async Task JsonFilePerLineTest()
        {
            var p = new JsonFormatterParser(new JsonFormatMessageFields());
            CancellationTokenSource cts = new CancellationTokenSource();
            string fileName = Path.Combine(Folder, "log files", "JsonFormatPerLine.clef");
            MessageHandlerForTesting forTesting = new MessageHandlerForTesting();
            var messages = (await p.Process(fileName, cts.Token, forTesting)).ToList();
            Assert.IsTrue(messages.Count == 2);
            //Assert.IsTrue(messages[0].Text == "Hello, { Name: \"nblumhardt\", Tags: [1, 2, 3] }, 0000007b at 06/07/2016 06:44:57","got"+ messages[0].Text);
            Assert.IsTrue(messages[0].User == "{ Name: \"nblumhardt\", Tags: [1, 2, 3] }");
        }

        [TestMethod]
        public async Task JsonFilePerFileTest()
        {
            JsonFileParser parser = new JsonFileParser(new JsonFormatMessageFields());
            CancellationTokenSource cts = new CancellationTokenSource();
            string fileName = Path.Combine(Folder, "log files", "JsonFormatPerFile.clef");
            MessageHandlerForTesting forTesting = new MessageHandlerForTesting();
            var messages = (await parser.Process(fileName, cts.Token, forTesting)).ToList();
            Assert.IsTrue(messages.Count == 2);
            //Assert.IsTrue(messages[0].Text == "Hello, { Name: \"nblumhardt\", Tags: [1, 2, 3] }, 0000007b at 06/07/2016 06:44:57","got"+ messages[0].Text);
            Assert.IsTrue(messages[0].User == "{ Name: \"nblumhardt\", Tags: [1, 2, 3] }");
        }

        [TestMethod]
        [DataRow("JsonFileCompactFormat.clef", FileFormat.CompactJsonFormatPerFile)]
        [DataRow("JsonFormatPerLine.clef", FileFormat.JsonFormatPerLine)]
        public void CompactJsonFormatTestAutomaticDetection(string fileName, FileFormat format)
        {
            string file = Path.Combine(Folder, "log files", fileName);
            var type = OfflineDataProvider.TryDetectFormat(file);
            Assert.IsTrue(type == format);
        }

        [TestMethod]
        public async Task JsonFilePerLineDateTimeWithOffsetTest()
        {
            var p = new JsonFormatterParser(new JsonFormatMessageFields());
            CancellationTokenSource cts = new CancellationTokenSource();
            string fileName = Path.Combine(Folder, "log files", "JsonFormatPerLine.clef");
            MessageHandlerForTesting forTesting = new MessageHandlerForTesting();
            var messages = (await p.Process(fileName, cts.Token, forTesting)).ToList();
            Assert.IsTrue(messages.Count == 2);
        }

        //[TestMethod]
        //public async Task ECSParserTest()
        //{
        //    JsonFormatterParser parser = new JsonFormatterParser(new ECSFormatMessageFields());
        //    CancellationTokenSource cts = new CancellationTokenSource();
        //    string fileName = @"C:\MVD2\Logs\ECS\ILIUM.log";
        //    MessageHandlerForTesting forTesting = new MessageHandlerForTesting();
        //    var messages = (await parser.Process(fileName, cts.Token, forTesting)).ToList();
        //    Assert.IsTrue(messages.Count == 2);
        //    //Assert.IsTrue(messages[0].Text == "Hello, { Name: \"nblumhardt\", Tags: [1, 2, 3] }, 0000007b at 06/07/2016 06:44:57","got"+ messages[0].Text);
        //    Assert.IsTrue(messages[0].User == "{ Name: \"nblumhardt\", Tags: [1, 2, 3] }");
        //}

        [TestMethod]
        public async Task EmptyKeyInPropertyTest()
        {
            var p = new JsonFormatterParser(new CompactJsonFormatMessageFields());
            CancellationTokenSource cts = new CancellationTokenSource();
            string fileName = Path.Combine(Folder, "log files", "empty_dictionary_key.clef");
            MessageHandlerForTesting forTesting = new MessageHandlerForTesting();
            var messages = (await p.Process(fileName, cts.Token, forTesting)).ToList();
            Assert.IsTrue(messages.Count == 1);
            Assert.IsTrue(messages[0].Text == "A dictionary: @{ FirstKey: \"FirstItem\", SecondKey: \"SecondItem\", <empty key>: \"\", FourthKey: \"FourthItem\" }");
        }

    }
}
