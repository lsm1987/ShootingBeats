using UnityEngine;

public class GeneralSocialLogic : ISocialLogic
{
    public string GameIDsFileName { get { return "GameIDs_Example"; } }

    public void OnBeforeAuthenticate()
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