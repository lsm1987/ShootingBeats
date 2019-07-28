using UnityEngine;
using GooglePlayGames;
using System;
using System.Collections.Generic;

public class SocialSystem
{
    public static SocialSystem _Instance { get; private set; }
    private ISocialLogic _logic;
    private Dictionary<string, string> _gameIDs = null; // GPG 에서 사용하는 ID들
    private const string _autoSignInPrefKey = "AutoSignIn"; // 자동 로그인 Pref 키

    public SocialSystem()
    {
        _logic = CreateLogic();
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

        _logic.OnBeforeAuthenticate();

        _IsAuthenticating = true;
        Social.localUser.Authenticate((bool success) =>
        {
            _IsAuthenticating = false;
            if (success)
            {
                Debug.Log("[SocialSystem] Login successful!");

                // 로그인 성공시 추가로 할 일
                LoadGameIDs();
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
    /// 자동로그인 여부 기록
    /// </summary>
    public void SetAutoSignIn(bool set)
    {
        PlayerPrefs.SetInt(_autoSignInPrefKey, (set ? 1 : 0));
    }

    /// <summary>
    /// 자동로그인이 지정되어있는가?
    /// </summary>
    public bool IsAutoSignInSet()
    {
        return (PlayerPrefs.GetInt(_autoSignInPrefKey, 0) != 0);
    }

    /// <summary>
    /// GPG 용 ID 읽어들이기
    /// </summary>
    public void LoadGameIDs()
    {
        if (_gameIDs == null)
        {
            _gameIDs = new Dictionary<string, string>();
            CSVReader csv = new CSVReader("GameIDs");
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
}