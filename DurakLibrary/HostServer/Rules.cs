using System;
using System.Collections.Generic;
using System.Linq;

namespace DurakLibrary.HostServer
{
    public static class Rules
    {
        public static readonly List<IGameStateRule> StateRules;
        public static readonly List<IGameInitRule> InitRules;
        public static readonly List<IGamePlayRule> PlayRules;
        public static readonly List<IMoveSucessRule> MoveSuccessRules;
        public static readonly List<IAIRule> AIRules;
        public static readonly List<IBotInvokeStateChecker> BotInvokeRules;
        public static readonly List<IClientStateSetValidator> ClientStateReqValidators;

        static Rules()
        {
            StateRules = new List<IGameStateRule>();
            Utils.FillTypeList(AppDomain.CurrentDomain, StateRules);

            InitRules = new List<IGameInitRule>();
            Utils.FillTypeList(AppDomain.CurrentDomain, InitRules);
            InitRules.OrderBy(r => r.Priority);

            PlayRules = new List<IGamePlayRule>();
            Utils.FillTypeList(AppDomain.CurrentDomain, PlayRules);

            AIRules = new List<IAIRule>();
            Utils.FillTypeList(AppDomain.CurrentDomain, AIRules);

            ClientStateReqValidators = new List<IClientStateSetValidator>();
            Utils.FillTypeList(AppDomain.CurrentDomain, ClientStateReqValidators);

            BotInvokeRules = new List<IBotInvokeStateChecker>();
            Utils.FillTypeList(AppDomain.CurrentDomain, BotInvokeRules);

            MoveSuccessRules = new List<IMoveSucessRule>();
            Utils.FillTypeList(AppDomain.CurrentDomain, MoveSuccessRules);
        }
    }
}
