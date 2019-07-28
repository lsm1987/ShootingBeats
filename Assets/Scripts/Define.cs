using UnityEngine;
using GooglePlayGames;

public static class SceneName
{
    public const string _Title = "Title";
    public const string _BeatList = "BeatList";
    public const string _Stage = "Stage";
}

public static class ButtonName
{
    public const string _start = "Start";
}

public enum Difficulty
{
    Normal, Hard, Extreme,
}

public static class AchievementKey
{
    public const string _openAbout = "AboutGame";  // About 메뉴 열기
    public const string _changeMoveSensitivity = "GearShift";   // 이동 민감도 변경
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

    // UI 프리팹 리소스 경로
    public const string _uiMessageBoxPath = "UI/UIMessageBox";
    public const string _uiOptionPath = "UI/UIOption";
    public const string _uiAboutPath = "UI/UIAbout";
    public const string _uiLetterBoxBottom = "UI/LetterBoxBottom";
    public const string _uiLetterBoxLeft = "UI/LetterBoxLeft";
    public const string _uiLetterBoxRight = "UI/LetterBoxRight";
    public const string _uiStageText = "UI/UIStageText";
    public const string _uiHeaderPanel = "UI/UIHeaderPanel";
    public const string _albumArtRoot = "AlbumArts";    // 앨범아트 스프라이트 경로


    public static void InitCommonSystems()
    {
        if (GlobalSystem._Instance == null)
        {
            GlobalSystem.CreateInstance();
        }

        if (SocialSystem._Instance == null)
        {
            SocialSystem.CreateInstance();
        }
    }

    /// <summary>
    /// 음악 클리어 여부 저장용 Key 구하기
    /// </summary>
    private static string GetSongClearedPrefKey(BeatInfo beatInfo)
    {
        return string.Format("Song_{0}_Cleared", beatInfo._namespace);
    }

    /// <summary>
    /// 음악 클리어 기록이 있는가?
    /// </summary>
    public static bool IsSongCleared(BeatInfo beatInfo)
    {
        return PlayerPrefs.GetInt(GetSongClearedPrefKey(beatInfo), 0) != 0;
    }

    /// <summary>
    /// 음악 클리어 여부를 기록
    /// </summary>
    public static void SetSongCleared(BeatInfo beatInfo, bool cleared)
    {
        PlayerPrefs.SetInt(GetSongClearedPrefKey(beatInfo), ((cleared) ? 1 : 0));
    }

    /// <summary>
    /// 음악 하이스코어 저장용 Key 구하기
    /// </summary>
    private static string GetSongHighScorePrefKey(BeatInfo beatInfo)
    {
        return string.Format("Song_{0}_HighScore", beatInfo._namespace);
    }

    /// <summary>
    /// 음악 하이스코어 기록을 구한다.
    /// </summary>
    public static int GetSongHighScore(BeatInfo beatInfo)
    {
        return PlayerPrefs.GetInt(GetSongHighScorePrefKey(beatInfo), 0);
    }

    /// <summary>
    /// 음악 하이스코어를 기록
    /// </summary>
    public static void SetSongHighScore(BeatInfo beatInfo, int highScore)
    {
        PlayerPrefs.SetInt(GetSongHighScorePrefKey(beatInfo), highScore);
    }

    /// <summary>
    /// 음악 리더보드 ID 구하기
    /// </summary>
    private static string GetSongLeaderboardID(BeatInfo beatInfo)
    {
        string key = ("leaderboard" + beatInfo._namespace);
        if (SocialSystem._Instance != null)
        {
            return SocialSystem._Instance.GetGameID(key);
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 음악 리더보드 UI 열기
    /// </summary>
    public static void OpenSongLeaderboard(BeatInfo beatInfo)
    {
#if UNITY_ANDROID
        string id = GetSongLeaderboardID(beatInfo);
        PlayGamesPlatform.Instance.ShowLeaderboardUI(id);
#endif // UNITY_ANDROID
    }

    /// <summary>
    /// 음악 리더보드에 점수 올리기
    /// </summary>
    public static void ReportScoreToSongLeaderboard(BeatInfo beatInfo, int score)
    {
        string id = GetSongLeaderboardID(beatInfo);
        Social.ReportScore(score, id
            , success =>
            {
                string text = success ? "Reported score successfully" : "Failed to report score";
                text += ("(song: " + beatInfo._namespace + " score:" + score.ToString() + ")");

                if (success)
                {
                    Debug.Log(text);
                }
                else
                {
                    Debug.LogError(text);
                }
            });
    }

    /// <summary>
    /// 업적 ID 구하기
    /// </summary>
    private static string GetAchievementID(string key)
    {
        key = ("achievement" + key);
        if (SocialSystem._Instance != null)
        {
            return SocialSystem._Instance.GetGameID(key);
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 업적 진행상황 지정하기
    /// </summary>
    public static void ReportAchievementProgress(string key, double progress)
    {
        string id = GetAchievementID(key);
        Social.ReportProgress(id, progress
            , success =>
            {
                string text = success ? "Reported achievement successfully" : "Failed to report achievement";
                text += ("(key: " + key + " progress:" + progress.ToString() + ")");

                if (success)
                {
                    Debug.Log(text);
                }
                else
                {
                    Debug.LogError(text);
                }
            });
    }

    private static string GetAlbumArtPath(BeatInfo beatInfo)
    {
        return Define._albumArtRoot + "/" + beatInfo._namespace;
    }

    public static Sprite GetAlbumArtSprite(BeatInfo beatInfo)
    {
        return Resources.Load<Sprite>(GetAlbumArtPath(beatInfo));
    }
}