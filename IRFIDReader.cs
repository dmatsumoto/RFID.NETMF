using System;
using Microsoft.SPOT;

namespace BioNex.NETMF
{
    public interface IRFIDReader
    {
        bool ReadEEPROM( out byte[] data);
        bool ReadSerialNumber( out byte[] serial_number);
        bool ReadDeviceId( out byte[] device_id);
    }
}
