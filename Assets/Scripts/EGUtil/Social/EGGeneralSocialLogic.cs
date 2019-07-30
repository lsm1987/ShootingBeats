using UnityEngine;

public class EGGeneralSocialLogic : IEGSocialLogic
{
    public void Activate()
    {
        // Do nothing
    }

    public void SignOut()
    {
        Debug.LogWarning("[GeneralSocialLogic] SignOut - Not implemented for this platform");
    }

    public void ShowLeaderboardUI(string leaderboardID)
    {
        Debug.LogWarning("[GeneralSocialLogic] ShowLeaderboardUI - Not implemented for this platform");
    }
}