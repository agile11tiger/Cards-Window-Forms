using DurakLibrary.Common;

namespace DurakLibrary.HostServer
{
    public interface IBotInvokeStateChecker
    {
        bool ShouldInvoke(CoreDurakGame core, Player player);
    }
}
