using UnityEngine;

public class EGGeneralSocialLogic : IEGSocialLogic
{
    public void Activate()
    {
        // Do nothing
    }

    public void ShowLeaderboardUI(string leaderboardID)
    {
        Debug.LogWarning("[GeneralSocialLogic] ShowLeaderboardUI - Not implemented for this platform");
    }
}