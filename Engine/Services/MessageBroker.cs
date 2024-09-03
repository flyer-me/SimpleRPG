using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Engine.EventArgs;

namespace Engine.Services
{
    public class MessageBroker
    {
        private static readonly MessageBroker _messageBroker = new MessageBroker();
        private MessageBroker()
        {
        }
        public event EventHandler<GameMessageEventArgs> OnMessageRaised;
        public static MessageBroker GetInstance()
        {
            return _messageBroker;
        }
        internal void RaiseMessage(string message)
        {
            OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message));
        }
    }
}