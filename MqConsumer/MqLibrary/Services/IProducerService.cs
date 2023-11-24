using MqLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqLibrary.Services
{
    public interface IProducerService
    {
        public void SendMessage(MqUserObject user);
        public void Close();
    }
}
