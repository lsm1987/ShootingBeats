using UnityEngine;
using System;
using System.Collections.Generic;

public class SocialSystem
{
    public static SocialSystem _Instance { get; private set; }
    private ISocialLogic _logic;
    private Dictionary<string, string> _gameIDs = null; // 리더보드, 업적에서 사용하는 ID들

    public SocialSystem()
    {
        _logic = CreateLogic();
        _logic.Initialize();
        LoadGameIDs();
    }

    public static SocialSystem CreateInstance()
    {
        if (_Instance != null)
        {
            Debug.LogError("[SocialSystem] Instance was already created");
            return null;
        }
        else
        {
            _Instance = new SocialSystem();
            return _Instance;
        }
    }

    private static ISocialLogic CreateLogic()
    {
#if UNITY_ANDROID
        return new AndroidSocialLogic();
#elif UNITY_IOS
        return new IosSocialLogic();
#else
        return new GeneralSocialLogic();
#endif
    }

    /// <summary>
    /// 로그인 시도 중
    /// </summary>
    public bool _IsAuthenticating
    {
        get;
        private set;
    }

    /// <summary>
    /// 로그인 되어있는가?
    /// </summary>
    public bool _IsAuthenticated
    {
        get
        {
            return Social.Active.localUser.authenticated;
        }
    }

    /// <summary>
    /// 로그인 수행
    /// </summary>
    public void Authenticate(Action<bool> callback)
    {
        if (_IsAuthenticated || _IsAuthenticating)
        {
            Debug.LogWarning("[SocialSystem] Ignoring repeated call to Authenticate().");
            return;
        }

        _IsAuthenticating = true;
        Social.localUser.Authenticate((bool success) =>
        {
            _IsAuthenticating = false;
            if (success)
            {
                Debug.Log("[SocialSystem] Login successful!");
            }
            else
            {
                Debug.LogWarning("[SocialSystem] Failed to sign in.");
            }

            // 결과 발생시 호출부에서 실행할 내용
            if (callback != null)
            {
                callback(success);
            }
        });
    }

    /// <summary>
    /// 로그아웃 수행
    /// </summary>
    public void SignOut()
    {
        _logic.SignOut();
    }

    /// <summary>
    /// 자동로그인이 지정되어있는가?
    /// </summary>
    public bool IsAutoSignInSet()
    {
        return _logic._IsAutoSignInSet;
    }

    /// <summary>
    /// 리더보드, 업적용 ID 읽어들이기
    /// </summary>
    private void LoadGameIDs()
    {
        if (_gameIDs == null)
        {
            _gameIDs = new Dictionary<string, string>();
            CSVReader csv = new CSVReader(_logic._GameIDsFileName);
            while (true)
            {
                string[] line = csv.ReadLine();
                if (line != null)
                {
                    // 키,값
                    _gameIDs.Add(line[0], line[1]);
                }
                else
                {
                    // 더 읽을 내용 없음
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 지정한 key에 해당하는 ID를 구한다. 찾지 못하면 null 리턴
    /// </summary>
    public string GetGameID(string key)
    {
        string value = null;
        if (_gameIDs != null && _gameIDs.TryGetValue(key, out value))
        {
            return value;
        }
        else
        {
            return null;
        }
    }

    public void ShowAchievementsUI()
    {
        if (_IsAuthenticated)
        {
            // 로그인 되어있을 때만
            Social.ShowAchievementsUI();
        }
        else
        {
            Debug.LogWarning("[SocialSystem] ShowAchievementsUI - Not signed in.");
        }
    }

    public void ShowLeaderboardUI(string leaderboardID)
    {
        if (_IsAuthenticated)
        {
            _logic.ShowLeaderboardUI(leaderboardID);
        }
        else
        {
            Debug.LogWarning("[SocialSystem] ShowLeaderboardUI - Not signed in.");
        }
    }

    public void ReportScore(long score, string board)
    {
        Social.ReportScore(score, board, success =>
        {
            string text = "[SocialSystem] ";
            text += success ? "Reported score successfully" : "Failed to report score";
            text += ("(board: " + board + " score:" + score.ToString() + ")");

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

    public void ReportProgress(string achievementID, double progress)
    {
        Social.ReportProgress(achievementID, progress, success =>
        {
            string text = "[SocialSystem] ";
            text += success ? "Reported achievement successfully" : "Failed to report achievement";
            text += ("(achievementID: " + achievementID + " progress:" + progress.ToString() + ")");

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
}