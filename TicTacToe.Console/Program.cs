using TicTacToe.Client;
using TicTacToe.Core;
using TicTacToe.Server;
using TicTacToe.Server.Local;

namespace TicTacToe.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameServer server = new GameServer();
            
            GameClient player1 = new GameClient();
            GameClient player2 = new GameClient();

            player1.Initialize(server);
            player2.Initialize(server);

            server.Initialize(
                new LocalGameClientProxy(server, player1, 1), 
                new LocalGameClientProxy(server, player2, 2));

            server.Start();

            while (true)
            {
                if (player1.GameEnded)
                {
                    WriteGameResult(player1, player2);
                    break;
                }

                DrawMap(server.Board);

                if (player1.MyTurn)
                {
                    System.Console.WriteLine("Player 1 move: ");
                }
                else
                {
                    System.Console.WriteLine("Player 2 move: ");
                }

                var position = ReadPosition();

                if (player1.MyTurn)
                {
                    using (new GameCommunicationContext(1))
                    {
                        player1.Place(position.Row, position.Column);
                    }
                }
                else
                {
                    using (new GameCommunicationContext(2))
                    {
                        player2.Place(position.Row, position.Column);
                    }
                }
            }
        }

        private static void DrawMap(GameBoard board)
        {
            var arr = board.Board;

            System.Console.WriteLine("  123");
            System.Console.WriteLine();

            for (int row = 0; row < GameConstants.Rows; row++)
            {
                System.Console.Write($"{row + 1} ");
                for (int column = 0; column < GameConstants.Columns; column++)
                {
                    System.Console.Write(arr[row, column]);
                }
                System.Console.WriteLine();
            }
        }

        private static void WriteGameResult(GameClient player1, GameClient player2)
        {
            if (player1.Won)
            {
                System.Console.WriteLine("Player 1 has won!");
            }
            else
            {
                System.Console.WriteLine("Player 2 has won!");
            }
        }

        private static (int Row, int Column) ReadPosition()
        {
            System.Console.Write("row: ");
            string row = System.Console.ReadLine();

            System.Console.Write("column: ");
            string column = System.Console.ReadLine();

            return new (int.Parse(row) - 1, int.Parse(column) - 1);
        }
    }
}
