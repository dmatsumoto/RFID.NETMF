using System;
using Microsoft.SPOT;

namespace BioNex.NETMF
{
    public static class NumberConversions
    {
        /// <summary>
        /// Converts two character strings to a byte, e.g.
        /// "FF" => 255
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte HexStringToByte( string hex)
        {
            Debug.Assert( hex.Length == 2, "HexStringToByte only accepts two-character strings");
            char high_nibble = hex[0];
            char low_nibble = hex[1];
            return (byte)(HexCharToNibble( high_nibble) << 4 | HexCharToNibble( low_nibble));
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Converts a hexdecimal character into a byte, e.g.
        /// 'A' => 10
        /// '8' => 8
        /// 'E' => 14
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static byte HexCharToNibble( char c)
        {
            if( c >= '0' && c <= '9')
                return (byte)(c - '0');
            else if( c >= 'a' && c <= 'f')
                return (byte)(c - 'a' + 10);
            else if( c >= 'A' && c <= 'F')
                return (byte)(c - 'A' + 10);
            
            throw new Exception( "HexCharToNibble received invalid character: " + c.ToString());
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Converts a nibble to a hex character, e.g.
        /// 10 => 'A'
        /// 5 => '5'
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private static char NibbleToHexChar( byte b)
        {
            if( b >= 0 && b <= 9)
                return (char)('0' + b);
            else if( b >= 10 && b <= 15)
                return (char)('A' + b - 10);

            throw new Exception( "NibbleToHexChar received invalid nibble: " + b.ToString());
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Converts a byte to a two-character string, e.g.
        /// 255 => "FF"
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ByteToHexString( byte b)
        {            
            return new String( ByteToCharArray( b));
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Converts a byte to a char array, e.g.
        /// 255 => new byte[] { 0x46, 0x46 }
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static char[] ByteToCharArray( byte b)
        {
            char high_nibble = NibbleToHexChar( (byte)((b & 0xF0) >> 4));
            char low_nibble = NibbleToHexChar( (byte)(b & 0x0F));
            return new char[] { high_nibble, low_nibble };
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Converts a ushort to a char array, e.g.
        /// 2571 => new char { 0x30, 0x41, 0x30, 0x42 }
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static char[] ShortToCharArray( ushort s)
        {
            byte msb = (byte)((s & 0xFF00) >> 8);
            char msb_high_nibble = NibbleToHexChar( (byte)((msb & 0xF0) >> 4));
            char msb_low_nibble = NibbleToHexChar( (byte)(msb & 0x0F));

            byte lsb = (byte)(s & 0x00FF);
            char lsb_high_nibble = NibbleToHexChar( (byte)((lsb & 0xF0) >> 4));
            char lsb_low_nibble = NibbleToHexChar( (byte)(lsb & 0x0F));

            return new char[] { msb_high_nibble, msb_low_nibble, lsb_high_nibble, lsb_low_nibble };
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Converts a string to a char array, e.g.
        /// "hello" => 0x68, 0x65, 0x6C, 0x6C, 0x6F => { 0x36, 0x38, 0x36, 0x35, 0x36, 0x43, 0x36, 0x43, 0x36, 0x46, 0x00 }
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] StringToByteArray( string s)
        {
            // loop over each character in the string and convert each using ByteToCharArray
            System.Collections.ArrayList array = new System.Collections.ArrayList();
            foreach( char c in s) {
                char[] nibbles = ByteToCharArray( (byte)c);
                array.Add( (byte)nibbles[0]);
                array.Add( (byte)nibbles[1]);
            }

            // add the null termination
            array.Add( (byte)0x00);

            byte[] result = (byte[])array.ToArray( typeof(byte));
            return result;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------
        public static void PrintByteArray( byte[] array)
        {
            string array_string = "=> ";
            int len = array.Length;
            for( int i=0; i<len; i++) {
                array_string += ByteToHexString( array[i]);
                if( i<len-1)
                    array_string += " ";
            }
            Debug.Print( array_string);
        }       
        //-------------------------------------------------------------------------------------------------------------------------------------------
        public static void PrintWordArray( ushort[] array)
        {
            string array_string = "";            
            int len = array.Length;
            for( int i=0; i<len; i++) {
                array_string += array[i] + "d";
                if( i<len-1)
                    array_string += ", ";
            }
            Debug.Print( array_string);
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------        
    }
}
