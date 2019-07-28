using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class IosSocialLogic : ISocialLogic
{
    public string GameIDsFileName { get { return "GameIDs_iOS"; } }

    public void OnBeforeAuthenticate()
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