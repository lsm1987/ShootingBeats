using UnityEngine;

public static class SceneName
{
    public const string _Title = "Title";
    public const string _BeatList = "BeatList";
    public const string _Stage = "Stage";
}

public static class Define
{
    public const int _fps = 60; // 갱신주기

    /// <summary>
    /// FPS 지정. 원복은 하지 않아도 됨
    /// </summary>
    public static void SetFPS()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = _fps;
    }

    // 초단위 길이를 문자열로 변경
    public static string ConverBeatLength(int sec)
    {
        int lengthMin = sec / 60; // 분
        int lengthSec = sec % 60; // 초
        return (lengthMin.ToString() + ":" + lengthSec.ToString("00"));
    }
}