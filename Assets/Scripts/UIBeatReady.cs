using UnityEngine;
using UnityEngine.UI;

public class UIBeatReady : UIWindow
{
    [SerializeField]
    private Image _albumArt;
    [SerializeField]
    private Text _title;
    [SerializeField]
    private Text _author;
    [SerializeField]
    private Text _highScore;
    [SerializeField]
    private UIDifficultyIcon _difficulty;
    [SerializeField]
    private Text _length;
    [SerializeField]
    private Button _leaderboard;
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
        _leaderboard.interactable = (GlobalSystem._Instance != null && GlobalSystem._Instance._IsAuthenticated); // 로그인 되었을 때만 사용가능
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
        if (GlobalSystem._Instance == null)
        {
            GlobalSystem.CreateInstance();
        }
        GlobalSystem._Instance._LoadingBeatInfo = _beatInfo;
        Application.LoadLevel(SceneName._Stage);
    }

    /// <summary>
    /// 리더보드 열기
    /// </summary>
    public void OnLeaderboardClicked()
    {
        if (GlobalSystem._Instance != null && GlobalSystem._Instance._IsAuthenticated)
        {
            Define.OpenSongLeaderboard(_beatInfo);
        }
    }
}
