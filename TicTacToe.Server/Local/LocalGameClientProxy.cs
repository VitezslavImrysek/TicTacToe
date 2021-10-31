using TicTacToe.Core;

namespace TicTacToe.Server.Local
{
    public class LocalGameClientProxy : GameClientProxy
    {
        public LocalGameClientProxy(IGame gameServer, IGameCallback gameClient, int id) 
            : base(gameServer)
        {
            CallbackChannel = gameClient;
            Id = id;
        }

        public override IGameCallback CallbackChannel { get; }

        public override void Dispose()
        {

        }
    }
}
