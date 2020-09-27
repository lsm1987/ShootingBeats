using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSystem : SceneSystem
{
    [SerializeField]
    private Button _btnStart = null;
    [SerializeField]
    private LocalizeStringEvent _strEvtStart = null;
    private readonly TableEntryReference _strKeyStart = "Menu_Start";   // 게임 시작
    private readonly TableEntryReference _strKeySignInDoing = "Menu_SignIn";   // 로그인 도중
    [SerializeField]
    private Button _btnAchievement = null;

    protected override void OnAwake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Define.SetFPS();
        Define.InitCommonSystems();

        // 로그인 관련
        if (!SocialSystem._Instance._IsAutoSignInTried
            && !EGSocial._IsAuthenticated
            && !EGSocial._IsAuthenticating)
        {
            // 로그인 되어있지 않고, 자동로그인을 한번도 시도하지 않았다면
            SocialSystem._Instance._IsAutoSignInTried = true;
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
    /// 로그인 시도
    /// </summary>
    private void TrySignIn()
    {
        EGSocial.Authenticate(OnSignInResult);
        RefreshUIBySignInState();
    }

    /// <summary>
    /// 로그인 결과가 발생했을 때
    /// </summary>
    private void OnSignInResult(bool success)
    {
        RefreshUIBySignInState();
    }

    /// <summary>
    /// 로그인 상태 따른 UI 갱신
    /// </summary>
    private void RefreshUIBySignInState()
    {
        // 시작 버튼
        _btnStart.interactable = !EGSocial._IsAuthenticating; // 시도 중이 아닐 때 가능
        _strEvtStart.StringReference.SetReference(StringTableName._ui, EGSocial._IsAuthenticating ? _strKeySignInDoing : _strKeyStart);

        // 업적 버튼
        _btnAchievement.interactable = EGSocial._IsAuthenticated; // 로그인 되었을 때 가능
    }

    #region UI Event
    public void OnStartClicked()
    {
        if (EGSocial._IsAuthenticating)
        {
            // 로그인 시도 중이면 반응하지 않음
            return;
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

    public void OnAchievementClicked()
    {
        EGSocial.ShowAchievementsUI();
    }
    #endregion UI Event
}
