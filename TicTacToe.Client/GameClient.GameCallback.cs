using TicTacToe.Core;

namespace TicTacToe.Client
{
    public partial class GameClient : IGameCallback
    {
        public bool MyTurn { get; set; }
        public bool GameEnded { get; set; }
        public bool Won { get; set; }

        public void OnGameEnded(bool won)
        {
            GameEnded = true;
            Won = won;
        }

        public void OnMarkPlaced(int mark, int row, int column)
        {
            Board.Board[row, column] = mark;
        }

        public void OnTurnCompleted()
        {
            MyTurn = false;
        }

        public void OnTurnStarted()
        {
            MyTurn = true;
        }
    }
}
