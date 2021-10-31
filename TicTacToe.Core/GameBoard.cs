using System;

namespace TicTacToe.Core
{
    public class GameBoard
    {
        public GameBoard()
        {
            Board = new int[GameConstants.Rows, GameConstants.Columns];
        }

        public int[,] Board { get; }
    }
}
