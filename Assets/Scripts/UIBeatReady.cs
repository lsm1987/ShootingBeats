using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIBeatReady : UIPage
{
    [SerializeField]
    private Image _albumArt = null;
    [SerializeField]
    private LocalizeStringEvent _strEvtTitle = null;
    [SerializeField]
    private LocalizeStringEvent _strEvtAuthor = null;
    [SerializeField]
    private Text _highScore = null;
    [SerializeField]
    private UIDifficultyIcon _difficulty = null;
    [SerializeField]
    private Text _length = null;
    [SerializeField]
    private Button _leaderboard = null;
    [SerializeField]
    private Text _leaderboardText = null;
    [SerializeField]
    private Color _leaderboardTextEnabledColor = Color.black;
    [SerializeField]
    private Color _leaderboardTextDisabledColor = Color.gray;

    private BeatInfo _beatInfo;
    private readonly TableEntryReference _strKeyTitle = "BeatReady_Header";

    protected override void OnAwake()
    {
        base.OnAwake();
        AddHeaderPanel(_strKeyTitle, OnBackClicked);
    }

    public void Open(BeatInfo beatInfo)
    {
        // 정보 채우기
        _beatInfo = beatInfo;
        _albumArt.sprite = Define.GetAlbumArtSprite(_beatInfo);
        _strEvtTitle.StringReference = beatInfo._titleString;
        _strEvtAuthor.StringReference = beatInfo._authorString;
        _highScore.text = string.Format("High Score: {0}", Define.GetSongHighScore(beatInfo)); // 한꺼번에 가운데 정렬 위해 텍스트를 둘로 나누지 않음
        if (!Define.IsSongCleared(beatInfo))
        {
            _highScore.text += " (Not Cleared)";
        }
        _difficulty.SetDifficulty(beatInfo._difficulty);
        _length.text = Define.ConverBeatLength(_beatInfo._length);
        _leaderboard.interactable = (EGSocial._IsAuthenticated); // 로그인 되었을 때만 사용가능
        _leaderboard.GetComponent<MaterialUI.RippleConfig>().enabled = _leaderboard.interactable;
        _leaderboardText.color = (_leaderboard.interactable) ? _leaderboardTextEnabledColor : _leaderboardTextDisabledColor;

        // 활성화
        _Go.SetActive(true);
    }

    public override bool OnKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
        else if (Input.GetButtonDown(ButtonName._screenshot))
        {
            Define.CaptureScreenshot();
        }
        return true;
    }

    public void OnBackClicked()
    {
        Close();
    }

    /// <summary>
    /// 창 비활성화
    /// </summary>
    private void Close()
    {
        _Go.SetActive(false);
        _beatInfo = null;
    }

    // 게임 씬으로 이동
    public void OnPlayClicked()
    {
        Define.InitCommonSystems();
        GlobalSystem._Instance._LoadingBeatInfo = _beatInfo;
        SceneManager.LoadScene(SceneName._Stage);
    }

    /// <summary>
    /// 리더보드 열기
    /// </summary>
    public void OnLeaderboardClicked()
    {
        if (EGSocial._IsAuthenticated)
        {
            Define.OpenSongLeaderboard(_beatInfo);
        }
    }
}
