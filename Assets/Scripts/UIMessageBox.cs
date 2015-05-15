using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

// 메시지 박스
// 버튼 2개 고정. 텍스트 공간 고정
public class UIMessageBox : UIWindow
{
    [SerializeField]
    private Text _text; // 본문
    [SerializeField]
    private Button _btn1;
    [SerializeField]
    private Text _btn1Text;
    [SerializeField]
    private Button _btn2;
    [SerializeField]
    private Text _btn2Text;

    // 버튼이 눌렸을 때 호출될 함수 형식
    public delegate void OnButtonClicked();

    public override bool OnKeyInput()
    {
        return true;
    }

    // 창 닫기
    private void Close()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 본문 지정
    /// </summary>
    public void SetText(string text)
    {
        _text.text = text;
    }

    /// <summary>
    /// 버튼별 정보 지정
    /// </summary>
    public void SetButton(int btnIndex, string btnText, OnButtonClicked btnFunc)
    {
        if (btnIndex == 0)
        {
            _btn1Text.text = btnText;
            _btn1.onClick.AddListener(() =>
            {
                // 클릭시 함수가 지정되어있다면 수행
                if (btnFunc != null)
                {
                    btnFunc();
                }

                // 창닫기
                Close();
            });
        }
        else if (btnIndex == 1)
        {
            _btn2Text.text = btnText;
            _btn2.onClick.AddListener(() =>
            {
                if (btnFunc != null)
                {
                    btnFunc();
                }
                Close();
            });
        }
    }
}
