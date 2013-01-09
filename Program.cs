using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace BioNex.NETMF
{
    public class Program
    {
        public static void Main()
        {
            // write your code here
            Parallax28440 reader = new Parallax28440();
            reader.Connect( 2);
            while (true)
            {
                byte[] serial_number;
                if (!reader.ReadSerialNumber(out serial_number)) 
                {
                    serial_number = new byte[] { 0, 0, 0, 0 };
                }

                Debug.Print( "Serial number: " + NumberConversions.ByteToHexString(serial_number[0]) + 
                                                 NumberConversions.ByteToHexString(serial_number[1]) +
                                                 NumberConversions.ByteToHexString(serial_number[2]) +
                                                 NumberConversions.ByteToHexString(serial_number[3]));

                /*
                byte[] device_id;
                if (!reader.ReadDeviceId(out device_id))
                {
                    device_id = new byte[] { 0, 0, 0, 0 };
                }

                Debug.Print( "Serial number: " + serial_number[0] + " " + serial_number[1] + " " + serial_number[2] + " " + serial_number[3] + " " +
                             "Device ID: " + device_id[0] + " " + device_id[1] + " " + device_id[2] + " " + device_id[3]);
                */

                Thread.Sleep( 100);
            }
        }

    }
}
