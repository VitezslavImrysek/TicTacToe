using System;

namespace TicTacToe.Server
{
    public class GameCommunicationContext : IDisposable
    {
        [ThreadStatic]
        private static int _clientId;

        public GameCommunicationContext(int clientId)
        {
            _clientId = clientId;
        }

        public static int ClientId => _clientId; 

        public void Dispose()
        {
            _clientId = -1;
        }
    }
}
