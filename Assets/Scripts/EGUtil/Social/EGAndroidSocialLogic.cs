#if UNITY_ANDROID
using UnityEngine;
using GooglePlayGames;

public class EGAndroidSocialLogic : IEGSocialLogic
{
    public void Activate()
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