using System;
using TicTacToe.ConsoleApp.Local;
using TicTacToe.ConsoleApp.SignalR;

namespace TicTacToe.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //LocalConsoleGame game = new LocalConsoleGame();
            //game.Run();

            Console.WriteLine("Run as server?");
            Console.Write("(Y/N): ");
            var response = Console.ReadLine();

            SignalRConsoleGame game = new SignalRConsoleGame(
                response.Equals("Y", StringComparison.OrdinalIgnoreCase), 
                "http://localhost:5003");
            game.Run();
        }
    }
}
