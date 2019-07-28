public interface ISocialLogic
{
    string GameIDsFileName { get; }

    void OnBeforeAuthenticate();
    void SignOut();
    void ShowLeaderboardUI(string leaderboardID);
}