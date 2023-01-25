using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Chatterino.Net
{
    internal class Server
    {
        private TcpClient _tcpClient;
        public PacketReader PacketReader;
        public event Action ConnectedEvent;
        public event Action MessageReceibedEvent;
        public event Action DisconnectEvent;
        public Server()
        {
            _tcpClient = new TcpClient();
        }

        public void Connect(string Username)
        {
            if (!_tcpClient.Connected)
            {
                _tcpClient.Connect("127.0.0.1", 1234);
                PacketReader = new PacketReader(_tcpClient.GetStream());

                if (!string.IsNullOrEmpty(Username))
                {
                    var connectionPacket = new PacketBuilder();
                    connectionPacket.WriteOpCode(0);
                    connectionPacket.WriteMessage(Username);
                    _tcpClient.Client.Send(connectionPacket.GetPacketBytes());
                }
                ReadPackets ();
            }
        }

        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var opcode = PacketReader.ReadByte();
                    switch (opcode)
                    {
                        case 1:
                            ConnectedEvent?.Invoke();
                            break;
                        case 2:
                            MessageReceibedEvent?.Invoke();
                            break;
                        case 3:
                            DisconnectEvent?.Invoke();
                            break;
                        default:
                            break;
                    }

                }
            });
        }

        public void SendMessage(string message)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(2);
            messagePacket.WriteMessage(message);
            _tcpClient.Client.Send(messagePacket.GetPacketBytes());
        }
    }
}
