using System;
using System.Threading.Tasks;
using TicTacToe.Core;

namespace TicTacToe.Server
{
    public partial class GameServer
    {
        private object _lock = new object();
        private GameClientProxy _client1;
        private GameClientProxy _client2;

        public GameServer()
        {

        }

        public void Initialize(GameClientProxy client1, GameClientProxy client2)
        {
            _client1 = client1;
            _client2 = client2;

            Board = new GameBoard();
        }

        public void Start()
        {
            _turnId = _client1.Id;
            _client1.CallbackChannel.OnTurnStarted();
        }

        private void ForeachClient(Action<IGameCallback> action)
        {
            Task.Run(() => action(_client1.CallbackChannel));
            Task.Run(() => action(_client2.CallbackChannel));
        }

        private void DoSafely(Action action)
        {
            lock (_lock)
            {
                action();
            }
        }
    }
}
