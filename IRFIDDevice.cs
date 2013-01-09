using System;
using Microsoft.SPOT;

namespace BioNex.NETMF
{
    public interface IRFIDDevice
    {
        /// <summary>
        /// Opens a connection to the specified COM port
        /// </summary>
        /// <remarks>
        /// It is up to the derived class to specify the serial port settings specific to its associated hardware
        /// </remarks>
        /// <param name="comport">the COM port number, e.g. 1 for COM1, 2 for COM2</param>
        void Connect( int comport);
    }

    public class RFIDException : ApplicationException
    {
        public RFIDException() : base()
        {
        }

        public RFIDException(string message) : base(message)
        {
        }

        public RFIDException(string message, Exception inner_exception) : base(message, inner_exception)
        {
        }
    }
}
