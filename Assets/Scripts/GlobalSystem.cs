using UnityEngine;
using GooglePlayGames;
using System;
using System.Collections.Generic;

// 전역적으로 관리되어야 하는 정보
public class GlobalSystem
{
    public static GlobalSystem _Instance { get; private set; }  // 전역 접근용
    public Config _Config { get; private set; } // 설정 접근용
    public BeatInfo _LoadingBeatInfo { get; set; }  // 스테이지에서 불러올 노래 정보
    private Dictionary<string, string> _gameIDs = null; // GPG 에서 사용하는 ID들
    private const string _autoSignInPrefKey = "AutoSignIn"; // 자동 로그인 Pref 키

    public GlobalSystem()
    {
        _Config = new Config();
    }

    /// <summary>
    /// 새 인스턴스를 생성한다. 이미 생성되어있었다면 null 리턴
    /// </summary>
    /// <returns></returns>
    public static GlobalSystem CreateInstance()
    {
        if (_Instance != null)
        {
            Debug.LogError("[GlobalSystem] Instance was already created");
            return null;
        }
        else
        {
            _Instance = new GlobalSystem();
            return _Instance;
        }
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
            Debug.LogWarning("Ignoring repeated call to Authenticate().");
            return;
        }

        PlayGamesPlatform.Activate();

        _IsAuthenticating = true;
        Social.localUser.Authenticate((bool success) =>
        {
            _IsAuthenticating = false;
            if (success)
            {
                Debug.Log("Login successful!");

                // 로그인 성공시 추가로 할 일
                LoadGameIDs();
            }
            else
            {
                Debug.LogWarning("Failed to sign in with Google Play Games.");
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
        ((PlayGamesPlatform)Social.Active).SignOut();
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

/// <summary>
/// 어플 설정 정보
/// </summary>
public class Config
{
    // 이동 민감도. 단위 %
    public const int _moveSensitivityMin = 50; // 최소
    public const int _moveSensitivityMax = 200; // 최대
    public const int _moveSensitivityDefault = 120; // 기본값
    private int _moveSensitivity = _moveSensitivityDefault;
    private const string _moveSensitivityKey = "Config_MoveSensitivity"; // 설정 저장용 key
    public int _MoveSensitivity
    {
        get
        {
            return _moveSensitivity;
        }
        set
        {
            _moveSensitivity = value;
            PlayerPrefs.SetInt(_moveSensitivityKey, _moveSensitivity);
        }
    }

    public Config()
    {
        // 설정 읽어들이기
        _moveSensitivity = PlayerPrefs.GetInt(_moveSensitivityKey, _moveSensitivityDefault);
    }

    /// <summary>
    /// 기본값으로 초기화
    /// </summary>
    public void ResetToDefault()
    {
        PlayerPrefs.DeleteKey(_moveSensitivityKey);
        _moveSensitivity = _moveSensitivityDefault;
    }
}