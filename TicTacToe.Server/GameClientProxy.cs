using System;
using TicTacToe.Core;

namespace TicTacToe.Server
{
    public abstract partial class GameClientProxy : IDisposable
    {
        protected GameClientProxy(int clientId)
        {
            Id = clientId;
        }
        
        public abstract IGameCallback CallbackChannel { get; }

        public int Id { get; private set; }

        public abstract void Dispose();
    }
}
