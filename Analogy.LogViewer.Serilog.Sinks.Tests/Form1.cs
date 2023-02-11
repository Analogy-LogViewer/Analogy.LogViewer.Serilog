using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Analogy.LogViewer.Serilog.Sinks.Tests
{
    public partial class Form1 : Form
    {
       
        Logger log = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProcessId()
            .Enrich.WithThreadId()
            .Enrich.With<ProcessNameEnricher>()
            .WriteTo.AnalogyLogServerSink().
            
            CreateLogger();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 1000; i++)
            {
                log.Information("test {i}", i);
                await Task.Delay(250);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            log = new LoggerConfiguration()
                .WriteTo.AnalogyLogServerSink(txtAddress.Text).CreateLogger();
        }

    }
}
