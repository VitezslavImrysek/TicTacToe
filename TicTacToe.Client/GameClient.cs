using TicTacToe.Core;

namespace TicTacToe.Client
{
    public partial class GameClient
    {
        private IGame _serverConnection;

        public GameClient()
        {

        }

        public void Initialize(IGame serverConnection)
        {
            _serverConnection = serverConnection;

            InitializeGame();
        }
    }
}
