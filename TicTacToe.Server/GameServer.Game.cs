using TicTacToe.Core;

namespace TicTacToe.Server
{
    public partial class GameServer : IGame
    {
        private int _turnId;

        private IGameCallback Client1 => _client1.CallbackChannel;
        private IGameCallback Client2 => _client2.CallbackChannel;

        public GameBoard Board { get; set; }

        public void Place(int row, int column)
        {
            DoSafely(() => PlaceImpl(row, column));
        }

        private void PlaceImpl(int row, int column)
        {
            int clientId = GameCommunicationContext.ClientId;
            
            // Check if its players turn
            if (_turnId != clientId)
            {
                return;
            }

            // Check if the board position is valid
            var id = Board.Board[row, column];
            if (id != 0)
            {
                return;
            }

            Board.Board[row, column] = clientId;

            ForeachClient(callback => callback.OnMarkPlaced(clientId, row, column));

            if (CheckGameEnded(out int playerId))
            {
                if (playerId == 1)
                {
                    Client1.OnGameEnded(true);
                    Client2.OnGameEnded(false);
                }
                else
                {
                    Client1.OnGameEnded(false);
                    Client2.OnGameEnded(true);
                }
            }
            else
            {
                if (clientId == 1)
                {
                    _turnId = 2;
                    Client1.OnTurnCompleted();
                    Client2.OnTurnStarted();
                }
                else
                {
                    _turnId = 1;
                    Client2.OnTurnCompleted();
                    Client1.OnTurnStarted();
                }
            }
        }

        private bool CheckGameEnded(out int playerId)
        {
            for (int row = 0; row < GameConstants.Rows; row++)
            {
                for (int column = 0; column < GameConstants.Columns; column++)
                {
                    var count = GetMarkCount(row, column);
                    if (count == GameConstants.WinCount)
                    {
                        playerId = Board.Board[row, column];
                        return true;
                    }
                }
            }

            playerId = -1;
            return false;
        }

        private int GetMarkCount(int targetRow, int targetColumn)
        {
            var board = Board.Board;

            int id = board[targetRow, targetColumn];
            if (id == 0)
            {
                return 0;
            }

            int count = 0;
            for (int row = targetRow - GameConstants.WinCount - 1; row < targetRow + GameConstants.WinCount; row++)
            {
                if (row < 0 || row >= GameConstants.Rows)
                {
                    continue;
                }

                for (int column = targetColumn - GameConstants.WinCount - 1; column < targetColumn + GameConstants.WinCount; column++)
                {
                    if (column < 0 || column >= GameConstants.Columns)
                    {
                        continue;
                    }

                    if (board[row, column] == id)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
