using UnityEngine;
using UnityEngine.UI;
using System.Text;

// About 창
public class UIAbout : UIPage
{
    private const string _uiTitle = "About";

    [SerializeField]
    private Text _debugInfo = null;

    protected override void OnAwake()
    {
        base.OnAwake();
        AddHeaderPanel(_uiTitle, OnBackClicked);
        Dev_ShowDebugInfo();

        // 업적 시도
        if (SocialSystem._Instance._IsAuthenticated)
        {
            Define.ReportAchievementProgress(AchievementKey._openAbout, 100.0);
        }
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

    [System.Diagnostics.Conditional("DEVELOPMENT_BUILD"), System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void Dev_ShowDebugInfo()
    {
        StringBuilder sb = new StringBuilder("Debug Info");
        sb.AppendLine();
        sb.AppendFormat("Screen size: {0}, {1}", Screen.width, Screen.height);
        sb.AppendLine();
        sb.AppendFormat("Safe area: {0}", Screen.safeArea.ToString());

        _debugInfo.text = sb.ToString();
    }
}