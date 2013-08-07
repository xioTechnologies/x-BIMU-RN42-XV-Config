using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO.Ports;
using System.Threading;

namespace x_BIMU_RN42_XV_Config
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Name + " " + Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString());
            do
            {
                foreach (string portName in SerialPort.GetPortNames())
                {
                    Console.Write("Configuring RN42-XV on " + portName + "...");
                    try
                    {
                        SerialPort serialPort = new SerialPort(portName, 115200, Parity.None, 8, StopBits.One);
                        //serialPort.Handshake = Handshake.RequestToSend;
                        //serialPort.DtrEnable = true;
                        serialPort.Open();
                        serialPort.Write("$$$");    // enter command mode
                        Thread.Sleep(100);
                        serialPort.Write("SF,1\r"); // set factory defaults
                        Thread.Sleep(1000);
                        serialPort.Write("SM,0\r"); // slave mode
                        Thread.Sleep(100);
                        serialPort.Write("ST,0\r"); // disable remote configuration
                        Thread.Sleep(100);
                        serialPort.Write("S-,x-BIMU\r");    // set serialized friendly name
                        Thread.Sleep(100);
                        serialPort.Write("S~,0\r"); // enables SPP protocol
                        Thread.Sleep(100);
                        serialPort.Write("R,1\r");  // reset device for settings to take effect
                        serialPort.Close();
                        Console.WriteLine("Compelte.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Failed (" + e.Message + ")");
                        Thread.Sleep(1000);
                    }
                }
                Console.WriteLine("Press Esc to exit or any other key to repeat...");
            }
            while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }
    }
}