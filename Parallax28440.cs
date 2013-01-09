using System;
using System.IO.Ports;
using Microsoft.SPOT;

namespace BioNex.NETMF
{
    public class Parallax28440 : IRFIDDevice, IRFIDReader
    {
        private System.IO.Ports.SerialPort _port;

        private class Commands
        {
            public static byte RFID_Read = 0x01;
        }

        #region IRFIDDevice Members

        public void Connect( int comport)
        {
            _port = new SerialPort( "COM" + comport, 9600, Parity.None, 8, StopBits.One);
            _port.ReadTimeout = 1000;
            _port.Open();
        }

        #endregion

        #region IRFIDReader Members

        public bool ReadEEPROM( out byte[] data)
        {
            try
            {
                data = ReadAddress(3);
            }
            catch (Exception ex)
            {
                data = new byte[1] { 0 };
                return false;
            }
            return true;
        }

        public bool ReadSerialNumber( out byte[] serial_number)
        {
            try
            {
                serial_number = ReadAddress(32);
            }
            catch (Exception ex)
            {
                serial_number = new byte[4] { 0, 0, 0, 0 };
                return false;
            }
            return true;
        }

        public bool ReadDeviceId( out byte[] device_id)
        {
            try
            {
                device_id = ReadAddress(33);
            }
            catch (Exception ex)
            {
                device_id = new byte[4] { 0, 0, 0, 0 };
                return false;
            }
            return true;
        }

        #endregion

        private byte[] ReadAddress( byte address)
        {
            byte[] response = WriteCommandBytes( new byte[] { Commands.RFID_Read, address }, 5);
            CheckForErrors(response);
            return new byte[] { response[1], response[2], response[3], response[4] };
        }

        private byte[] WriteCommandBytes( byte[] bytes, byte expected_return_bytes)
        {
            _port.Flush();
            // write the header first
            _port.Write( new byte[] { (byte)'!', (byte)'R', (byte)'W' }, 0, 3 );
            // now write the user-requested data
            _port.Write( bytes, 0, bytes.Length);
            byte[] temp = new byte[expected_return_bytes]; // max of 12 bytes returned by reader
            System.Threading.Thread.Sleep( 250);
            _port.Read(temp, 0, expected_return_bytes);
            NumberConversions.PrintByteArray( temp);
            return temp;
        }

        private void CheckForErrors(byte[] bytes)
        {
            switch (bytes[0])
            {
                case 0x01: // no error
                    break;
                case 0x02: // LIW
                    throw new RFIDException( "Could not find a Listen Windows (LIW) from the tag");
                case 0x03: // NAK
                    throw new RFIDException( "Received a No Acknowledge (NAK), possible communication error or invalid command/data");
                case 0x04: // NAK_OLDPW
                    throw new RFIDException( "Received a No Acknowledge (NAK) sending the current password during the SetPass command, possible incorrect password");
                case 0x05: // NAK_NEWPW
                    throw new RFIDException( "Received a No Acknowledge (NAK) sending the new password during the SetPass command");
                case 0x06: // LIW_NEWPW
                    throw new RFIDException( "Could not find a Listen Window (LIW) from the tag after setting the new password during the SetPass command");
                case 0x07: // PARITY
                    throw new RFIDException( "Parity error when reading data from the tag");
                default:
                    throw new RFIDException( "Invalid data from reader module");
            }
        }
    }
}
