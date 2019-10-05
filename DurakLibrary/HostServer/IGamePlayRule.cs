using DurakLibrary.Common;

namespace DurakLibrary.HostServer
{
    public interface IGamePlayRule
    {
        bool IsEnabled { get; set; }
        string ReadableName { get; }
        bool IsValidMove(CoreDurakGame core, GameMove move, ref string reason);
    }
}
