using DurakLibrary.Common;

namespace DurakLibrary.HostServer
{
    public interface IGameInitRule
    {
        bool IsEnabled { get; set; }
        string ReadableName { get; }
        int Priority { get; }
        void InitState(CoreDurakGame core);
    }
}
