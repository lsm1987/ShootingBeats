using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSystem : SceneSystem
{
    [SerializeField]
    private Button _btnStart = null;
    [SerializeField]
    private Button _btnSignIn = null;
    [SerializeField]
    private Text _btnTextSignIn = null;
    private const string _textSignIn = "Google Play Sign In";    // 로그인 하기
    private const string _textSignOut = "Google Play Sign Out";  // 로그아웃 하기
    private const string _textSignInDoing = "Sign In...";    // 로그인 도중
    [SerializeField]
    private Button _btnAchievement = null;

    protected override void OnAwake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Define.SetFPS();
        Define.InitCommonSystems();

        // 로그인 관련
        if (SocialSystem._Instance.IsAutoSignInSet()
            && !SocialSystem._Instance._IsAuthenticated
            && !SocialSystem._Instance._IsAuthenticating)
        {
            // 자동로그인 지정되어있다면 로그인 시도
            TrySignIn();
        }
        else
        {
            RefreshUIBySignInState();
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
        SceneManager.LoadScene(SceneName._BeatList);
    }

    /// <summary>
    /// 프로그램 종료
    /// </summary>
    private void QuitAppication()
    {
        // 종료 시 프로세스가 바로 사라지지 않는 문제가 있어 추가하려했으나, 적용해보니 프로세스가 영영 사라지지 않아 사용하지 않음
        // http://stackoverflow.com/questions/28031789/unity-with-google-play-plugin-application-freeze-on-quit-application-on-android
        // https://github.com/playgameservices/play-games-plugin-for-unity/issues/310
        /*
#if UNITY_ANDROID
        if (GlobalSystem._Instance._IsAuthenticated)
        {
            GlobalSystem._Instance.SignOut();
        }
        using (AndroidJavaClass javaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject unityActivity = javaClass.GetStatic<AndroidJavaObject>("currentActivity");
            unityActivity.Call<bool>("moveTaskToBack", true);
        }
#endif
        */
        Application.Quit();
    }

    /// <summary>
    /// 로그인 시도
    /// </summary>
    private void TrySignIn()
    {
        SocialSystem._Instance.Authenticate(OnSignInResult);
        RefreshUIBySignInState();
    }

    /// <summary>
    /// 로그인 결과가 발생했을 때
    /// </summary>
    private void OnSignInResult(bool success)
    {
        if (success)
        {
            SocialSystem._Instance.SetAutoSignIn(true);    // 로그인 성공하면 자동로그인 지정
        }

        RefreshUIBySignInState();
    }

    /// <summary>
    /// 로그인 상태 따른 UI 갱신
    /// </summary>
    private void RefreshUIBySignInState()
    {
        // 시작 버튼
        _btnStart.interactable = !SocialSystem._Instance._IsAuthenticating; // 시도 중이 아닐 때 가능

        // 로그인 버튼
        if (SocialSystem._Instance._IsAuthenticating)
        {
            // 로그인 시도 중
            _btnSignIn.interactable = false;
            _btnTextSignIn.text = _textSignInDoing; // 시도 중으로 변경
        }
        else
        {
            _btnSignIn.interactable = true;
            if (SocialSystem._Instance._IsAuthenticated)
            {
                // 로그인 됨
                _btnTextSignIn.text = _textSignOut; // 로그아웃 텍스트로 변경
            }
            else
            {
                // 로그아웃 됨
                _btnTextSignIn.text = _textSignIn; // 로그인 텍스트로 변경
            }
        }

        // 업적 버튼
        _btnAchievement.interactable = SocialSystem._Instance._IsAuthenticated; // 로그인 되었을 때 가능
    }

    #region UI Event
    public void OnStartClicked()
    {
        if (SocialSystem._Instance._IsAuthenticating)
        {
            // 로그인 시도 중이면 반응하지 않음
            return;
        }
        else if (!SocialSystem._Instance._IsAuthenticated)
        {
            // 로그인되어있지 않으면 알림
            UIMessageBox box = _UISystem.OpenMessageBox();
            box.SetText("Not signed in Google play.\nHigh score will not recorded at leaderboard and cannot unlock achievement.");
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
        if (SocialSystem._Instance._IsAuthenticating)
        {
            // 이미 로그인 도중
            return;
        }
        else if (!SocialSystem._Instance._IsAuthenticated)
        {
            // 로그인 시도
            TrySignIn();
        }
        else
        {
            // 로그아웃
            SocialSystem._Instance.SignOut();
            SocialSystem._Instance.SetAutoSignIn(false);   // 유저가 직접 로그아웃 수행 시 자동로그인 해제
            RefreshUIBySignInState();
        }
    }

    public void OnAchievementClicked()
    {
        if (SocialSystem._Instance._IsAuthenticated)
        {
            // 로그인 되어있을 때만
            Social.ShowAchievementsUI();
        }
    }

    public void OnQuitClicked()
    {
        QuitAppication();
    }
    #endregion UI Event
}
