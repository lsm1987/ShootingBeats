using UnityEngine;

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