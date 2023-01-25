using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatterino.MVVM.Models
{
    internal class MessageModel
    {
        public string Username { get; set; }
        public string Text { get; set; }
        public DateTime SendTime { get; set; }
    }
}
