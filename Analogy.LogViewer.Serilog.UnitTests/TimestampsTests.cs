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
        public async Task CompactJsonFormatFullFileTest()
        {
            CompactJsonFormatParser parser = new CompactJsonFormatParser();
            CancellationTokenSource cts = new CancellationTokenSource();
            string fileName = Path.Combine(Folder, "log files", "timestamps.clef");
            MessageHandlerForTesting forTesting = new MessageHandlerForTesting();
            var messages = (await parser.Process(fileName, cts.Token, forTesting)).ToList();
            Assert.IsTrue(messages.Count is 4);

            // Assert.IsTrue(messages[9].Date.Equals(new DateTimeOffset(2016, 10, 12, 04, 46, 58, 55, TimeSpan.Zero)));
        }
    }
}