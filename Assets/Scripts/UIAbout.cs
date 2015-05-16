using UnityEngine;

// About 창
public class UIAbout : UIWindow
{
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
}
