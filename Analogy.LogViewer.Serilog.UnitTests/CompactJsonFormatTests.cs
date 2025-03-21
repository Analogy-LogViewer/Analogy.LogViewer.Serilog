﻿using Analogy.LogViewer.Serilog.DataTypes;
using Analogy.LogViewer.Serilog.IAnalogy;
using Analogy.LogViewer.Serilog.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
        [DataRow("CompactJsonFormat.clef", 4, "2016-10-12T04:46:58.0554314Z")]
        [DataRow("CompactJsonFormatSourceContextTest.clef", 2, "2020-06-18T18:03:19.2248275Z")]
        [DataRow("CompactJsonFormatTestColumns.clef", 4, "2020-06-26T14:21:34.7233612Z")]
        [DataRow("CompactJsonFormat.gz", 4, "2016-10-12T04:46:58.0554314Z")]
        public async Task OfflineProviderParserTimestampTest(string fileName, int numberOfMessages, string datetimeToParse)
        {
            OfflineDataProvider parser = new OfflineDataProvider();
            UserSettingsManager.UserSettings.Settings.SupportFormats= new List<string> { "*.Clef", "*.log", "*.gz", "*.zip" };
            CancellationTokenSource cts = new CancellationTokenSource();
            string file = Path.Combine(Folder, "log files", fileName);
            MessageHandlerForTesting forTesting = new MessageHandlerForTesting();
            var messages = (await parser.Process(file, cts.Token, forTesting)).ToList();
            DateTimeOffset dto = DateTimeOffset.Parse(datetimeToParse);
            Assert.IsTrue(messages.Count == numberOfMessages);
            Assert.IsTrue(messages[0].Date == dto);
        }

        [TestMethod]
        [DataRow("CompactJsonFormat.clef")]
        [DataRow("CompactJsonFormatSourceContextTest.clef")]
        [DataRow("CompactJsonFormatTestColumns.clef")]
        [DataRow("CompactJsonFormat.gz")]
        public void CompactJsonFormatTestAutomaticDetection(string fileName)
        {
            string file = Path.Combine(Folder, "log files", fileName);
            var type = OfflineDataProvider.TryDetectFormat(file);
            Assert.IsTrue(type == FileFormat.CompactJsonFormatPerLine);
        }
        [TestMethod]

        // [DataRow("rendered1.clef", 2, "test 2")]
        [DataRow("rendered2.clef", 2, "test 2")]
        public async Task OfflineProviderParserAlreadyRenderedTest(string fileName, int numberOfMessages, string text)
        {
            OfflineDataProvider parser = new OfflineDataProvider();
            UserSettingsManager.UserSettings.Settings.SupportFormats = new List<string> { "*.Clef", "*.log", "*.gz", "*.zip" };
            CancellationTokenSource cts = new CancellationTokenSource();
            string file = Path.Combine(Folder, "log files", fileName);
            MessageHandlerForTesting forTesting = new MessageHandlerForTesting();
            var messages = (await parser.Process(file, cts.Token, forTesting)).ToList();
            Assert.IsTrue(messages.Count == numberOfMessages);
            Assert.IsTrue(messages[1].Text == text);
        }

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
            Assert.IsNotNull(firstEvent.User);
            Assert.IsNotNull(firstEvent.MethodName);
            
            // The second event should have a source context
            var secondEvent = messages.ElementAt(1);
            Assert.AreEqual("Contextual Log", secondEvent.Text);
            Assert.AreEqual("SerilogLogging.Program", secondEvent.Source);
            Assert.AreEqual(1, secondEvent.ThreadId);
            Assert.IsNotNull(secondEvent.Module);
            Assert.IsNotNull(secondEvent.FileName);
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
            Assert.IsTrue(messages[1].AdditionalProperties["CustomProperty"] == "\"Custom Value\"");
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
            Assert.IsTrue(messages[2].AdditionalProperties["Tags"] == "[\"test\", \"orange\"]");
        }
    }
}