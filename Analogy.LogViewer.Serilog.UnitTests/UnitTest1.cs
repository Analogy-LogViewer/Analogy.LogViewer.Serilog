using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Analogy.LogViewer.Serilog.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            ClefParser p = new ClefParser();
            CancellationTokenSource cts=new CancellationTokenSource();
            await p.Process(@"..\logs\example1.clef", cts.Token, null);
        }
    }
}
