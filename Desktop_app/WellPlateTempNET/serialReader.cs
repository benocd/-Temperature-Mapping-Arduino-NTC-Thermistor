using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Entity;
using System.Xml.Linq;
using System.Runtime.CompilerServices;

namespace WellPlateTempNET
{
    internal class serialReader
    {
        private static SerialPort mySerialPort = new SerialPort();
        static string curline = "";

        public serialReader(string COMport)
        {
            CreateSerialPort(COMport);
        }

        public static void CreateSerialPort(string COMport)
        {
            mySerialPort.Dispose();
            mySerialPort.PortName = COMport;
            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None; // Some device needs a different handshake.
            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            mySerialPort.Open();
        }

        public string getCurrentLine()
        {
            return curline;
        }

        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;

            string line = sp.ReadLine();
            curline = line;
        }
    }
}
