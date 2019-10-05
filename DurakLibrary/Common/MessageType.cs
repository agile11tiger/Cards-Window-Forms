namespace DurakLibrary.Common
{
    public enum MessageType
    {
        WelcomePackage = 1,

        PlayerConnected = 10,
        PlayerDisconnected = 11,
        PlayerIsReady = 12,
        PlayerChat = 13,
        PlayerDigressed = 14,
        RemovePlayers = 15,

        HostReqStart = 20,

        GameStateChanged = 30,
        FullGameStateTransfer = 31,

        RequestState = 40,

        PlayerHandChanged = 50,
        CardCountChanged = 51,

        SendMove = 60,
        InvalidMove = 61,

        CannotStart = 70
    }
}
