#if UNITY_ANDROID
using UnityEngine;
using GooglePlayGames;

public class AndroidSocialLogic : ISocialLogic
{
    public void OnBeforeAuthenticate()
    {
        PlayGamesPlatform.Activate();
    }

    public void SignOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
    }
}
#endif // UNITY_ANDROID