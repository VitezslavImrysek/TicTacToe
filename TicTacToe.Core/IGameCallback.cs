namespace TicTacToe.Core
{
    public interface IGameCallback
    {
        void OnTurnStarted();
        void OnTurnCompleted();
        void OnMarkPlaced(int mark, int row, int column);
        void OnGameEnded(bool won);
    }
}
