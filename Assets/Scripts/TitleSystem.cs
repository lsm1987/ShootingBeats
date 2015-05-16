using UnityEngine;
using UnityEngine.UI;

public class TitleSystem : SceneSystem
{
    [SerializeField]
    private Text _btnTextSignIn;
    private const string _textSignIn = "Google Play Sign In";    // 로그인 하기
    private const string _textSignOut = "Google Play Sign Out";  // 로그아웃 하기
    private const string _textSignInDoing = "Sign In...";    // 로그인 도중

    protected override void OnAwake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Define.SetFPS();
        if (GlobalSystem._Instance == null)
        {
            GlobalSystem.CreateInstance();
        }

        // 로그인 관련
        if (GlobalSystem._Instance.IsAutoSignInSet()
            && !GlobalSystem._Instance._IsAuthenticated
            && !GlobalSystem._Instance._IsAuthenticating)
        {
            // 자동로그인 지정되어있다면 로그인 시도
            TrySignIn();
        }
        else
        {
            SetSignInButtonText(GlobalSystem._Instance._IsAuthenticated);
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

    /// <summary>
    /// 로그인 시도
    /// </summary>
    private void TrySignIn()
    {
        _btnTextSignIn.text = _textSignInDoing; // 시도 중으로 변경
        GlobalSystem._Instance.Authenticate(OnSignInResult);
    }

    /// <summary>
    /// 로그인 결과가 발생했을 때
    /// </summary>
    private void OnSignInResult(bool success)
    {
        SetSignInButtonText(success);
    }

    private void SetSignInButtonText(bool athenticated)
    {
        if (athenticated)
        {
            // 로그인 중이라면 로그아웃 텍스트로 변경
            _btnTextSignIn.text = _textSignOut;
        }
        else
        {
            // 로그아웃 중이라면 로그인 텍스트로 변경
            _btnTextSignIn.text = _textSignIn;
        }
    }

    #region UI Event
    public void OnStartClicked()
    {
        if (GlobalSystem._Instance._IsAuthenticating)
        {
            // 로그인 시도 중이면 반응하지 않음
            return;
        }
        else if (!GlobalSystem._Instance._IsAuthenticated)
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

    public void OnAboutClicked()
    {
        _UISystem.OpenWindow(Define._uiAboutPath);
    }

    public void OnSignInClicked()
    {
        if (GlobalSystem._Instance._IsAuthenticating)
        {
            // 이미 로그인 도중
            return;
        }
        else if (!GlobalSystem._Instance._IsAuthenticated)
        {
            // 로그인 시도
            TrySignIn();
        }
        else
        {
            // 로그아웃
            _btnTextSignIn.text = _textSignIn;  // 로그인으로 변경
            GlobalSystem._Instance.SignOut();
        }
    }

    public void OnQuitClicked()
    {
        QuitAppication();
    }
    #endregion UI Event
}
