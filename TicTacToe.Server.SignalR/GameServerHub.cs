using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Core;

namespace TicTacToe.Server.SignalR
{
    public class GameServerHub : Hub<IGameCallback>, IGame
    {
        public static event EventHandler<int> ClientConnected;

        public void Place(int row, int column) 
        {
            using (new GameCommunicationContext(int.Parse(GetClientId())))
            {
                GameServerSignalRHost.GameServer.Place(row, column);
            }
        }

        public override async Task OnConnectedAsync()
        {
            var id = GetClientId();

            await Groups.AddToGroupAsync(Context.ConnectionId, id);
            
            ClientConnected?.Invoke(null, int.Parse(id));
        }

        private string GetClientId()
        {
            return Context.GetHttpContext().Request.Headers["ClientID"];
        }
    }
}
