using System;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Client;
using TicTacToe.Client.SignalR;
using TicTacToe.Server;
using TicTacToe.Server.SignalR;

namespace TicTacToe.ConsoleApp.SignalR
{
    public class SignalRConsoleGame : ConsoleGameBase
    {
        private readonly bool _host;
        private readonly string _url;

        private GameServer _server;
        private GameServerSignalRHost _serverHost;

        public SignalRConsoleGame(bool host, string url)
        {
            _host = host;
            _url = url;
        }

        public override void Run()
        {
            if (_host)
            {
                StartGameServer();
            }

            // Create game client and connect to server.
            var player = CreateGameClient(_host ? 1 : 2);

            // Start game loop
            RunGameLoop(player);

            if (_host)
            {
                StopGameServer();
            }
        }

        private void RunGameLoop(GameClient player)
        {
            while (true)
            {
                Console.Clear();

                if (player.GameEnded)
                {
                    WriteGameResult(player);
                    break;
                }
                
                if (!player.MyTurn)
                {
                    continue;
                }

                DrawMap(player.Board);

                Console.WriteLine("Your move: ");
                
                var position = ReadPosition();

                player.Place(position.Row, position.Column);
            }
        }

        private GameClient CreateGameClient(int playerId)
        {
            GameClient client = new GameClient();

            GameClientHubConnection serverConnection = new GameClientHubConnection(client, _url, playerId);
            serverConnection.Open();

            client.Initialize(serverConnection);

            return client;
        }

        private void StartGameServer()
        {
            GameServer server = new GameServer();

            GameServerSignalRHost serverHost = new GameServerSignalRHost(server, _url);
            var hubContext = serverHost.Run();

            int connections = 0;

            TaskCompletionSource tcs = new TaskCompletionSource();
            GameServerHub.ClientConnected += (s, e) =>
            {
                int c = Interlocked.Increment(ref connections);

                if (c == 2) tcs.SetResult();
            };

            Task.Run(async () => 
            {
                await tcs.Task;

                server.Initialize(
                    new GameClientSignalRProxy(hubContext, 1),
                    new GameClientSignalRProxy(hubContext, 2));

                server.Start();
            });

            _server = server;
            _serverHost = serverHost;
        }

        private void StopGameServer()
        {
            _serverHost.Dispose();
            _serverHost = null;

            _server = null;
        }

        private void WriteGameResult(GameClient player)
        {
            if (player.Won)
            {
                Console.WriteLine("You have won!");
            }
            else
            {
                Console.WriteLine("You have lost");
            }
        }
    }
}
