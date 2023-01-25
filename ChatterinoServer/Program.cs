using System;
using System.Net;
using System.Net.Sockets;

namespace ChatterinoServer
{
    internal class Program
    {
        static TcpListener _listener;
        static List<Client> _clients;
        static void Main(string[] args)
        {
            _clients= new List<Client>();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 1234);
            _listener.Start();

            while (true)
            {
                var newClient = _listener.AcceptTcpClient();
                _clients.Add(new Client(newClient));
                BroadcastConnection();
            }
        }
        
        static void BroadcastConnection()
        {
            foreach (var client in _clients)
            {
                foreach (var cli in _clients)
                {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOpCode(1);
                    broadcastPacket.WriteMessage(cli.Username);
                    broadcastPacket.WriteMessage(cli.ID.ToString());
                    client.tcpClient.Client.Send(broadcastPacket.GetPacketBytes());
                }
            }
        }

        public static void BroadcastMessage( string message)
        {
            foreach(var client in _clients)
            {
                var messagePacket = new PacketBuilder();
                messagePacket.WriteOpCode(2);
                messagePacket.WriteMessage(message);
                client.tcpClient.Client.Send(messagePacket.GetPacketBytes());
            }
        }
        public static void BroadcastDisconnect(string id)
        {
            var disconnectedClient = _clients.Where(x => x.ID.ToString() == id).FirstOrDefault();
            _clients.Remove(disconnectedClient);
            foreach (var client in _clients)
            {
                
                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(3 );
                broadcastPacket.WriteMessage(id);
                client.tcpClient.Client.Send(broadcastPacket.GetPacketBytes());
            }
            BroadcastMessage($"{disconnectedClient.Username} has disconnected");
        }
    }
}