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
    private Text _difficulty;
    [SerializeField]
    private Text _length;
    private BeatInfo _beatInfo;
    private const string _albumArtRoot = "AlbumArts";   // 앨범아트 스프라이트 경로

    public void Open(BeatInfo beatInfo)
    {
        // 정보 채우기
        _beatInfo = beatInfo;
        _albumArt.sprite = Resources.Load<Sprite>(_albumArtRoot + "/" + _beatInfo._namespace);
        _title.text = _beatInfo._title;
        _difficulty.text = _beatInfo._difficulty.ToString();
        _length.text = Define.ConverBeatLength(_beatInfo._length);

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
}
