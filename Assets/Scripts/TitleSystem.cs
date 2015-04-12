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
            // 스테이지 시작
            if (Input.GetButtonDown("Start"))
            {
                OnStartClicked();
            }

            // 게임 종료
            if (Input.GetKeyDown(KeyCode.Escape))
            {
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
        LoadBeatListScene();
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
