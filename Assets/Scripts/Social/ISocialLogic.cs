public interface ISocialLogic
{
    string GameIDsFileName { get; }

    void OnBeforeAuthenticate();
    void SignOut();
}