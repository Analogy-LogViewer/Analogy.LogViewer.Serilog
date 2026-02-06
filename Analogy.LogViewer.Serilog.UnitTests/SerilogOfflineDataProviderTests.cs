using Analogy.LogViewer.Serilog.IAnalogy;
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
    public class SerilogOfflineDataProviderTests
    {
        [TestMethod]
        public async Task ParserFormatTest()
        {
            var provider = new SerilogOfflineDataProvider();
            string fileName = Path.Combine(Environment.CurrentDirectory, "log files", "formatException.log");
            MessageHandlerForTesting forTesting = new MessageHandlerForTesting();
            var messages = await provider.Process(fileName, CancellationToken.None, forTesting);
            Assert.HasCount(2, messages);
        }
    }
}