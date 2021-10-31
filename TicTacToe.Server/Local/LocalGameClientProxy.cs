using TicTacToe.Core;

namespace TicTacToe.Server.Local
{
    public class LocalGameClientProxy : GameClientProxy
    {
        public LocalGameClientProxy(IGameCallback gameClient, int id) 
            : base(id)
        {
            CallbackChannel = gameClient;
        }

        public override IGameCallback CallbackChannel { get; }

        public override void Dispose()
        {

        }
    }
}
