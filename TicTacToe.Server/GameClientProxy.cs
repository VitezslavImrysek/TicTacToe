using System;
using TicTacToe.Core;

namespace TicTacToe.Server
{
    public abstract partial class GameClientProxy : IDisposable
    {
        protected GameClientProxy(IGame gameServer)
        {
            GameServer = gameServer;
        }
        
        public abstract IGameCallback CallbackChannel { get; }

        public int Id { get; protected set; }

        protected IGame GameServer { get; }

        public abstract void Dispose();
    }
}
