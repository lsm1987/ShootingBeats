using UnityEngine;
using UnityEngine.UI;

// 옵션창
public class UIOption : UIWindow
{
    [SerializeField]
    private Slider _moveSensitivitySlider; // 이동 민감도
    [SerializeField]
    private Text _moveSensitivityValue;

    protected override void OnAwake()
    {
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
        Destroy(gameObject);
    }

    #region MoveSensitivity
    // 이동 민감도 관련 초기화
    private void InitMoveSensitivity()
    {
        // 초기값 반영
        int value = GlobalSystem._Instance._Config._MoveSensitivity;
        _moveSensitivitySlider.value = (float)value;
        _moveSensitivityValue.text = ConvertToMoveSensitivityString(value);

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
    }
    #endregion MoveSensitivity
}