#if UNITY_IOS
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class EGIosSocialLogic : IEGSocialLogic
{
    public void Activate()
    {
        GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
    }

    public void SignOut()
    {
        Debug.LogWarning("[IosSocialLogic] SignOut - Not implemented for this platform");
    }

    public void ShowLeaderboardUI(string leaderboardID)
    {
        GameCenterPlatform.ShowLeaderboardUI(leaderboardID, UnityEngine.SocialPlatforms.TimeScope.AllTime);
    }
}
#endif // UNITY_IOS