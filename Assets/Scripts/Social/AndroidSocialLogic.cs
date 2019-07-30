#if UNITY_ANDROID
using UnityEngine;
using GooglePlayGames;

public class AndroidSocialLogic : ISocialLogic
{
    public string _GameIDsFileName { get { return "GameIDs_Android"; } }

    public bool _IsAutoSignInSet { get { return true; } }

    public void Initialize()
    {
        PlayGamesPlatform.Activate();
    }

    public void SignOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
    }

    public void ShowLeaderboardUI(string leaderboardID)
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardID);
    }
}
#endif // UNITY_ANDROID