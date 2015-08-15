using UnityEngine;

// About 창
public class UIAbout : UIWindow
{
    private const string _uiTitle = "About";

    protected override void OnAwake()
    {
        base.OnAwake();
        AddHeaderPanel(_uiTitle, OnBackClicked);
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

    /// <summary>
    /// 트위터 클릭되었을 때
    /// </summary>
    public void OnDeveloperTwitterClicked()
    {
        const string devTwitter = "https://twitter.com/lsm1987";
        Application.OpenURL(devTwitter);

        // 업적 시도
        if (GlobalSystem._Instance._IsAuthenticated)
        {
            Define.ReportAchievementProgress(AchievementKey._visitTwitter, 100.0);
        }
    }
}
