using DurakLibrary.Common;

namespace DurakLibrary.HostServer
{
    public interface IGameStateRule
    {
        bool IsEnabled { get; set; }
        string ReadableName { get; }
        void ValidateState(CoreDurakGame core);
    }
}
