using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TicTacToe.Core;

namespace TicTacToe.Server.SignalR
{
    public class GameClientSignalRProxy : GameClientProxy, IGameCallback
    {
        private IHubContext<GameServerHub> _context;

        public GameClientSignalRProxy(IHubContext<GameServerHub> context, int id)
            : base(id)
        {
            _context = context;
        }

        public override IGameCallback CallbackChannel => this;

        public override void Dispose()
        {

        }

        public void OnGameEnded(bool won)
            => Task.Run(() => GetClient().SendAsync(nameof(OnGameEnded), won));

        public void OnMarkPlaced(int mark, int row, int column)
            => Task.Run(() => GetClient().SendAsync(nameof(OnMarkPlaced), mark, row, column));

        public void OnTurnCompleted()
            => Task.Run(() => GetClient().SendAsync(nameof(OnTurnCompleted)));

        public void OnTurnStarted()
            => Task.Run(() => GetClient().SendAsync(nameof(OnTurnStarted)));

        private IClientProxy GetClient() => _context.Clients.Group(Id.ToString()); 
    }
}
