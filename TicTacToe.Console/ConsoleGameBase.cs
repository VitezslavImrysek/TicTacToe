using System;
using TicTacToe.Core;

namespace TicTacToe.ConsoleApp
{
    public abstract class ConsoleGameBase
    {
        public abstract void Run();

        protected void DrawMap(GameBoard board)
        {
            var arr = board.Board;

            Console.WriteLine("  123");
            Console.WriteLine();

            for (int row = 0; row < GameConstants.Rows; row++)
            {
                Console.Write($"{row + 1} ");
                for (int column = 0; column < GameConstants.Columns; column++)
                {
                    Console.Write(arr[row, column]);
                }
                Console.WriteLine();
            }
        }

        protected (int Row, int Column) ReadPosition()
        {
            Console.Write("row: ");
            string row = Console.ReadLine();

            Console.Write("column: ");
            string column = Console.ReadLine();

            return new(int.Parse(row) - 1, int.Parse(column) - 1);
        }
    }
}
