using Chatterino.MVVM.Models;
using Chatterino.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chatterino.MVVM.ViewModels
{
    internal class MainViewModel
    {
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<string> Messages { get; set; }
        public RelayCommand ConnectCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        private Server _server;
        public MainViewModel()
        {
            Users = new ObservableCollection<UserModel>();
            Messages= new ObservableCollection<string>();
            _server = new Server();
            _server.ConnectedEvent += UserConnected;
            _server.MessageReceibedEvent += MessageReceived;
            _server.DisconnectEvent += RemoveUser;
            ConnectCommand = new RelayCommand(obj => _server.Connect(Username), obj => !string.IsNullOrEmpty(Username));
            SendMessageCommand = new RelayCommand(obj => _server.SendMessage(Message), obj => !string.IsNullOrEmpty(Message));
        }

        private void RemoveUser()
        {
            var id = _server.PacketReader.ReadMessage();
            var user = Users.Where(x => x.ID == id).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Users.Remove(user));
        }

        private void MessageReceived()
        {
            var message = _server.PacketReader.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => Messages.Add(message));
        }

        private void UserConnected()
        {
            var user = new UserModel
            {
                Username = _server.PacketReader.ReadMessage(),
                ID = _server.PacketReader.ReadMessage()
            };
            if (!Users.Any(x => x.ID == user.ID))
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }
    }
}
