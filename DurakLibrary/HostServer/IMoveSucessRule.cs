using DurakLibrary.Common;

namespace DurakLibrary.HostServer
{
    public interface IMoveSucessRule
    {
        void UpdateState(CoreDurakGame core, GameMove move);
    }
}
