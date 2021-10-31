using TicTacToe.Client;
using TicTacToe.Server;
using TicTacToe.Server.Local;

namespace TicTacToe.ConsoleApp.Local
{
    public class LocalConsoleGame : ConsoleGameBase
    {
        public override void Run()
        {
            GameServer server = new GameServer();

            GameClient player1 = new GameClient();
            GameClient player2 = new GameClient();

            player1.Initialize(server);
            player2.Initialize(server);

            server.Initialize(
                new LocalGameClientProxy(player1, 1),
                new LocalGameClientProxy(player2, 2));

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

        private void WriteGameResult(GameClient player1, GameClient player2)
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
    }
}
