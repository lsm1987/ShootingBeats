using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

// 옵션창
public class UIOption : UIPage
{
    [SerializeField]
    private Slider _moveSensitivitySlider = null; // 이동 민감도
    [SerializeField]
    private Text _moveSensitivityValue = null;

    private readonly TableEntryReference _strKeyTitle = "Option_Header";

    protected override void OnAwake()
    {
        base.OnAwake();
        AddHeaderPanel(_strKeyTitle, OnBackClicked);
        InitMoveSensitivity();
    }

    public override bool OnKeyInput()
    {
        // 창 닫기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBack();
        }
        return true;
    }

    // Back 버튼이 클릭되었을 때
    public void OnBackClicked()
    {
        OnBack();
    }

    // 창 닫기
    private void OnBack()
    {
        Destroy(_Go);
    }

    #region MoveSensitivity
    /// <summary>
    /// 이동 민감도 값을 설정으로부터 읽어들이기
    /// </summary>
    private void LoadMoveSensitivity()
    {
        int value = GlobalSystem._Instance._Config._MoveSensitivity;
        _moveSensitivitySlider.minValue = Config._moveSensitivityMin;
        _moveSensitivitySlider.maxValue = Config._moveSensitivityMax;
        _moveSensitivitySlider.value = (float)value;
        _moveSensitivityValue.text = ConvertToMoveSensitivityString(value);
    }

    // 이동 민감도 관련 초기화
    private void InitMoveSensitivity()
    {
        LoadMoveSensitivity();

        // 슬라이더 이벤트 핸들러 등록. 인스펙터에서 등록하면 값을 인자로 자동 전달 못함
        _moveSensitivitySlider.onValueChanged.AddListener(OnMoveSensitivityValueSliderValueChanged);
    }

    // 이동 민감도 슬라이더 값을 게임에 쓸 값으로 변환한다.
    private static int ConvertMoveSensitivityValue(float sliderValue)
    {
        int value = (int)sliderValue;
        value -= (value % 10); // 일의 자리 버림
        return value;
    }

    // 이동 민감도 값을 텍스트로 변환한다.
    private static string ConvertToMoveSensitivityString(int value)
    {
        return value.ToString() + " %";
    }

    // 이동 민감도 슬라이더 값이 바뀌었을 때
    public void OnMoveSensitivityValueSliderValueChanged(float sliderValue)
    {
        int value = ConvertMoveSensitivityValue(sliderValue);
        GlobalSystem._Instance._Config._MoveSensitivity = value;
        _moveSensitivityValue.text = ConvertToMoveSensitivityString(value);

        // 최대값 지정 시 업적 시도
        if (value == Config._moveSensitivityMax)
        {
            if (EGSocial._IsAuthenticated)
            {
                Define.ReportAchievementProgress(AchievementKey._changeMoveSensitivity, 100.0);
            }
        }
    }
    #endregion MoveSensitivity

    #region ResetConfig
    /// <summary>
    /// 설정 초기화
    /// </summary>
    public void OnResetConfigClicked()
    {
        // 값 초기화
        GlobalSystem._Instance._Config.ResetToDefault();
        // UI에 반영
        LoadMoveSensitivity();
    }
    #endregion ResetConfig
}