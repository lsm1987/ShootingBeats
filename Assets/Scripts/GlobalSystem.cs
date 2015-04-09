using UnityEngine;

// 전역적으로 관리되어야 하는 정보
public class GlobalSystem
{
    public static GlobalSystem _Instance { get; private set; }  // 전역 접근용
    public Config _Config { get; private set; } // 설정 접근용
    public BeatInfo _LoadingBeatInfo { get; set; }  // 스테이지에서 불러올 노래 정보

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
}

/// <summary>
/// 어플 설정 정보
/// </summary>
public class Config
{
    // 이동 민감도. 단위 %
    private const int _moveSensitivityDefault = 120; // 기본값
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