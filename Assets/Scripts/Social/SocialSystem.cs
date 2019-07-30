using UnityEngine;
using System.Collections.Generic;

public class SocialSystem
{
    public static SocialSystem _Instance { get; private set; }
    private Dictionary<string, string> _gameIDs = null; // 리더보드, 업적에서 사용하는 ID들

    public SocialSystem()
    {
        EGSocial.Activate();
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

    /// <summary>
    /// 자동로그인이 지정되어있는가?
    /// </summary>
    public bool IsAutoSignInSet()
    {
        return true;
    }

    private string _GameIDsFileName
    {
        get
        {
#if UNITY_ANDROID
            return "GameIDs_Android";
#elif UNITY_IOS
            return "GameIDs_iOS";
#else
            return "GameIDs_Example";
#endif
        }
    }

    /// <summary>
    /// 리더보드, 업적용 ID 읽어들이기
    /// </summary>
    private void LoadGameIDs()
    {
        if (_gameIDs == null)
        {
            _gameIDs = new Dictionary<string, string>();
            CSVReader csv = new CSVReader(_GameIDsFileName);
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