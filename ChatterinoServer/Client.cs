using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatterinoServer
{
    internal class Client
    {
        public string Username { get; set; }
        public Guid ID { get; set; }
        public TcpClient tcpClient { get; set; }
        private PacketReader _packetReader { get; set; }
        public Client(TcpClient client)
        {
            tcpClient = client;
            ID = Guid.NewGuid();
            _packetReader = new PacketReader(tcpClient.GetStream());
            var opcode = _packetReader.ReadByte();
            Username = _packetReader.ReadMessage();
            Console.WriteLine($"{DateTime.Now} - user '{Username}' has connected");
            Task.Run(() => ProcessPacket());
        }
        void ProcessPacket()
        {
            while (true)
            {
                try
                {
                    var opcode = _packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 2:
                            var message = _packetReader.ReadMessage();
                            Console.WriteLine($"{DateTime.Now} - {message}");
                            Program.BroadcastMessage($"{DateTime.Now} - {Username}: {message}");
                            break;
                        default
                            : break;
                    }
                }
                catch( Exception ex )
                {
                    Console.WriteLine($"{DateTime.Now} - {ID.ToString()} has disconnected");
                    Program.BroadcastDisconnect(ID.ToString());
                    tcpClient.Close();
                    break;
                }
            }
        }
    }
}
