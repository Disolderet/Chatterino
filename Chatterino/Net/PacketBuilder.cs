using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatterino.Net
{
    internal class PacketBuilder
    {
        MemoryStream memoryStream;

        public PacketBuilder()
        {
            memoryStream = new MemoryStream();
        }

        public void WriteOpCode(byte opCode)
        {
            memoryStream.WriteByte(opCode);
        }

        public void WriteMessage(string message)
        {
            memoryStream.Write(BitConverter.GetBytes(message.Length));
            memoryStream.Write(Encoding.ASCII.GetBytes(message));
        }

        public byte[] GetPacketBytes()
        {
            return memoryStream.ToArray();
        }
    }
}
