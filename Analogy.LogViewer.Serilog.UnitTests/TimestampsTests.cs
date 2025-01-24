using Analogy.LogViewer.Serilog.DataTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Serilog.UnitTests
{
    [TestClass]
    public class TimestampsTests
    {
        private string Folder { get; } = Environment.CurrentDirectory;

        [TestMethod]
        [DataRow("timestamps.clef", 4, "2016-10-12T04:46:58.0554314+03:00")]
        public async Task CompactJsonFormatFullFileTest(string fileName, int numberOfMessages, string datetimeToParse)
        {
            CompactJsonFormatParser parser = new CompactJsonFormatParser();
            CancellationTokenSource cts = new CancellationTokenSource();
            string file = Path.Combine(Folder, "log files", fileName);
            MessageHandlerForTesting forTesting = new MessageHandlerForTesting();
            var messages = (await parser.Process(file, cts.Token, forTesting)).ToList();
            Assert.IsTrue(messages.Count is 4);
            DateTimeOffset dto = DateTimeOffset.Parse(datetimeToParse);
            Assert.IsTrue(messages[0].Date.Equals(dto));
        }
    }
}