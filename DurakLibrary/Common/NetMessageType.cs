namespace DurakLibrary.Common
{
    public enum NetMessageType : byte
    {
        DataHosts = 1,
        HostVisibility = 2,

        PlayingPort = 10,
        PlayerID = 11,
        ConnectionApproval = 12,
        RequestRemovePlayer = 13,
        PlayerKicked = 14,

        ServerStateChanged = 30,
        Data = 40,
    }
}
