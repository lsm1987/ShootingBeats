using UnityEngine;

public class GeneralSocialLogic : ISocialLogic
{
    public string _GameIDsFileName { get { return "GameIDs_Example"; } }

    public bool _IsAutoSignInSet { get { return false; } }

    public void Initialize()
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