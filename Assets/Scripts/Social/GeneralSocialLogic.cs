using UnityEngine;

public class GeneralSocialLogic : ISocialLogic
{
    public void OnBeforeAuthenticate()
    {
        // Do nothing
    }

    public void SignOut()
    {
        Debug.LogWarning("[GeneralSocialLogic] SignOut - Not implemented for this platform");
    }
}