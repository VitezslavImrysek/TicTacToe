using TicTacToe.Core;

namespace TicTacToe.Client
{
    public partial class GameClient : IGame
    {
        public GameBoard Board { get; set; }
        
        public void InitializeGame()
        {
            Board = new GameBoard();
        }

        public void Place(int row, int column)
        {
            _serverConnection.Place(row, column);
        }
    }
}
