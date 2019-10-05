using DurakLibrary.Cards;
using DurakLibrary.Common;
using System.Collections.Generic;

namespace DurakLibrary.HostServer
{
    public interface IAIRule
    {
        void Propose(Dictionary<Card, float> proposalTable, CoreDurakGame server, Player botplayer);
    }
}
