using Microsoft.AspNetCore.SignalR.Client;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Core;

namespace TicTacToe.Client.SignalR
{
    public class GameClientHubConnection : IGame, IGameCallback
    {
        private GameClient _client;
        private string _url;
        private int _id;

        private HubConnection _connection;

        public GameClientHubConnection(GameClient client, string url, int id)
        {
            _client = client;
            _url = url;
            _id = id;
        }

        public void Open()
        {
            for (int i = 0; i < 10; i++)
            {
                var connection = new HubConnectionBuilder()
                    .WithUrl(
                        $"{_url}/GameServerHub",
                        options =>
                        {
                            options.Headers.Add("ClientID", _id.ToString());
                        })
                    .Build();

                connection.On<bool>(nameof(OnGameEnded), OnGameEnded);
                connection.On<int, int, int>(nameof(OnMarkPlaced), OnMarkPlaced);
                connection.On(nameof(OnTurnCompleted), OnTurnCompleted);
                connection.On(nameof(OnTurnStarted), OnTurnStarted);

                try
                {
                    connection.StartAsync().Wait();

                    _connection = connection;

                    break;
                }
                catch { }

                Thread.Sleep(500);
            }
        }

        public void OnGameEnded(bool won) => _client.OnGameEnded(won);

        public void OnMarkPlaced(int mark, int row, int column) => _client.OnMarkPlaced(mark, row, column);

        public void OnTurnCompleted() => _client.OnTurnCompleted();

        public void OnTurnStarted() => _client.OnTurnStarted();

        public void Place(int row, int column)
        {
            Task.Run(async () => await _connection.InvokeAsync(nameof(Place), row, column)).Wait();
        }
    }
}
