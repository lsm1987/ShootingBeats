public interface ISocialLogic
{
    string _GameIDsFileName { get; }
    bool _IsAutoSignInSet { get; }

    void Initialize();
    void SignOut();
    void ShowLeaderboardUI(string leaderboardID);
}