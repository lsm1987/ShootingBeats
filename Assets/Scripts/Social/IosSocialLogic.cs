using UnityEngine;

public class IosSocialLogic : ISocialLogic
{
    public string GameIDsFileName { get { return "GameIDs_iOS"; } }

    public void OnBeforeAuthenticate()
    {
        // Do nothing
    }

    public void SignOut()
    {
        Debug.LogWarning("[IosSocialLogic] SignOut - Not implemented for this platform");
    }
}