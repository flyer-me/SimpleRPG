using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPG.Core
{
    public class GameMessageEventArgs : EventArgs
    {
        public string Message { get; }
        public GameMessageEventArgs(string message)
        {
            Message = message;
        }
    }
    public class MessageBroker
    {
        private static readonly MessageBroker _messageBroker = new MessageBroker();
        private MessageBroker()
        {
        }
        public event EventHandler<GameMessageEventArgs> MessageRaised;
        public static MessageBroker GetInstance()
        {
            return _messageBroker;
        }
        public void RaiseMessage(string message)
        {
            OnMessageRaised(new GameMessageEventArgs(message));
        }
        public void OnMessageRaised(GameMessageEventArgs e)
        {
            MessageRaised?.Invoke(this, e);
        }
    }
}