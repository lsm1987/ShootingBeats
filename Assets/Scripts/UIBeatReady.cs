using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIBeatReady : UIPage
{
    [SerializeField]
    private Image _albumArt = null;
    [SerializeField]
    private Text _title = null;
    [SerializeField]
    private Text _author = null;
    [SerializeField]
    private Text _highScore = null;
    [SerializeField]
    private UIDifficultyIcon _difficulty = null;
    [SerializeField]
    private Text _length = null;
    [SerializeField]
    private Button _leaderboard = null;
    private BeatInfo _beatInfo;
    private const string _uiTitle = "Beat Ready";

    protected override void OnAwake()
    {
        base.OnAwake();
        AddHeaderPanel(_uiTitle, OnBackClicked);
    }

    public void Open(BeatInfo beatInfo)
    {
        // 정보 채우기
        _beatInfo = beatInfo;
        _albumArt.sprite = Define.GetAlbumArtSprite(_beatInfo);
        _title.text = _beatInfo._title;
        _author.text = _beatInfo._author;
        _highScore.text = string.Format("High Score: {0}", Define.GetSongHighScore(beatInfo)); // 한꺼번에 가운데 정렬 위해 텍스트를 둘로 나누지 않음
        if (!Define.IsSongCleared(beatInfo))
        {
            _highScore.text += " (Not Cleared)";
        }
        _difficulty.SetDifficulty(beatInfo._difficulty);
        _length.text = Define.ConverBeatLength(_beatInfo._length);
        _leaderboard.interactable = (SocialSystem._Instance != null && SocialSystem._Instance._IsAuthenticated); // 로그인 되었을 때만 사용가능
        _leaderboard.GetComponent<MaterialUI.RippleConfig>().enabled = _leaderboard.interactable;

        // 활성화
        _Go.SetActive(true);
    }

    public override bool OnKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
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
        if (SocialSystem._Instance != null && SocialSystem._Instance._IsAuthenticated)
        {
            Define.OpenSongLeaderboard(_beatInfo);
        }
    }
}
