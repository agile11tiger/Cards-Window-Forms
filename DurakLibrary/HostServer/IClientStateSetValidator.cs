using DurakLibrary.Common;

namespace DurakLibrary.HostServer
{
    public interface IClientStateSetValidator
    {
        void TrySetState(StateParameter parameter, CoreDurakGame core, Player sender);
    }
}
