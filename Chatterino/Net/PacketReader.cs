using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chatterino.Net
{
    internal class PacketReader : BinaryReader
    {
        private NetworkStream _networkStream;
        public PacketReader(NetworkStream networkStream) : base(networkStream)
        {
            _networkStream = networkStream;
        }

        public string ReadMessage()
        {
            byte[] buffer;
            var length = ReadInt32();
            buffer = new byte[length];
            _networkStream.Read(buffer, 0, length);

            return Encoding.ASCII.GetString(buffer);
        }
    }
}
