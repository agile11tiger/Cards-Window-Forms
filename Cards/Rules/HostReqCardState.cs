using DurakLibrary.Common;
using DurakLibrary.HostServer;

namespace DurakGame.Rules
{
    class HostReqCardState : IClientStateSetValidator
    {
        public void TrySetState(StateParameter parameter, CoreDurakGame core, Player sender)
        {
            if (parameter.Name == Names.AMOUNT_INIT_CARDS && parameter.ParameterType == StateParameter.Type.Int && sender.IsHost)
                core.GameState.Set(Names.AMOUNT_INIT_CARDS, parameter.GetValueInt(), true);
        }
    }
}
