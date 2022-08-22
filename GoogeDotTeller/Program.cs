﻿using SharpPcap;
using System.Reflection;
using System.Text;

namespace GoogeDotTeller
{
    public class Program
    {

        /// <summary>
        /// Largely copied from Example5 of the SharpPcap
        /// </summary>
        public static void Main()
        {
            var ver = Pcap.SharpPcapVersion;
            Console.WriteLine("SharpPcap Version {0}\n", ver);

            // Retrieve the device list
            var devices = CaptureDeviceList.Instance;

            // If no devices were found print an error
            if (devices.Count < 1)
            {
                Console.WriteLine("No devices were found on this machine");
                return;
            }

            Console.WriteLine("The following devices are available on this machine:");
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();

            int i = 0;

            // Scan the list printing every entry

            Console.WriteLine("Nr) Name - Description - mac Address");
            foreach (var dev in devices)
            {
                Console.WriteLine($"{i}) {dev.Name} - {dev.Description} - {dev.MacAddress}");
                i++;
            }
            string? consoleInput = string.Empty;
            while (string.IsNullOrEmpty(consoleInput))
            {
                Console.WriteLine();
                Console.Write("-- Please choose a device to capture: ");
                consoleInput = Console.ReadLine();
                if (!int.TryParse(consoleInput, out i))
                {
                    consoleInput = string.Empty;
                }
            }
            

            using var device = devices[i];

            //Register our handler function to the 'packet arrival' event
            device.OnPacketArrival +=
                new PacketArrivalEventHandler(device_OnPacketArrival);

            //Open the device for capturing
            int readTimeoutMilliseconds = 1000;
            device.Open(DeviceModes.Promiscuous, readTimeoutMilliseconds);

            // tcpdump filter to capture only TCP/IP packets
            StringBuilder filter = new StringBuilder();

            var lines = ReadLinesFromFile("google-prefixes.txt");

            foreach (string line in lines)
            {
                filter.Append($"dst net {line} or ");
            }
            filter.Remove(filter.Length - 4, 4);
            device.Filter = filter.ToString();

            Console.WriteLine();
            Console.WriteLine
                ("-- The following tcpdump filter will be applied: \"{0}\"",
                filter);
            Console.WriteLine
                ("-- Listening on {0}, hit 'Ctrl-C' to exit...",
                device.Description);

            // Start capture packets
            device.Capture();

        }

        /// <summary>
        /// Prints the time and length of each received packet
        /// </summary>
        private static void device_OnPacketArrival(object sender, PacketCapture e)
        {
            Console.Write(".");
            Console.Write("\a");
        }

        private static List<string> ReadLinesFromFile(string embeddedFileName)
        {
            var assembly = typeof(Program).GetTypeInfo().Assembly;
            var resourceName = assembly.GetManifestResourceNames().First(s => s.EndsWith(embeddedFileName, StringComparison.CurrentCultureIgnoreCase));

            List<string> lines = new List<string>();

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException("Could not load manifest resource stream.");
                }
                using (var reader = new StreamReader(stream))
                {
                    string? line = reader.ReadLine();
                    while (line != null) {
                        lines.Add(line.Trim());
                        line = reader.ReadLine();
                    }
                }
            }
            return lines;
        }
    }

}
