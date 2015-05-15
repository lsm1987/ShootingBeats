using UnityEngine;

public class TitleSystem : SceneSystem
{
    protected override void OnAwake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Define.SetFPS();
        if (GlobalSystem._Instance == null)
        {
            GlobalSystem.CreateInstance();
        }
    }

    protected override void OnUpdate()
    {
        // 키입력 처리
        if (_HasKeyInputFocus)
        {
            if (Input.GetButtonDown(ButtonName._start))
            {
                // 스테이지 시작
                OnStartClicked();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                // 게임 종료
                OnQuitClicked();
            }
        }
    }

    /// <summary>
    /// 음악 목록 씬으로
    /// </summary>
    private void LoadBeatListScene()
    {
        Application.LoadLevel(SceneName._BeatList);
    }

    /// <summary>
    /// 프로그램 종료
    /// </summary>
    private void QuitAppication()
    {
        Application.Quit();
    }

    #region UI Event
    public void OnStartClicked()
    {
        if (!GlobalSystem._Instance._IsAuthenticated)
        {
            // 로그인되어있지 않으면 알림
            UIMessageBox box = _UISystem.OpenMessageBox();
            box.SetText("Not signed in Google play.\nHigh score will not recorded at leaderboard.");
            box.SetButton(0, "OK", LoadBeatListScene);  // 진행
            box.SetButton(1, "Cancel", null);   // 중지
        }
        else
        {
            LoadBeatListScene();
        }
    }

    public void OnOptionClicked()
    {
        _UISystem.OpenWindow(Define._uiOptionPath);
    }

    public void OnQuitClicked()
    {
        QuitAppication();
    }
    #endregion UI Event
}
