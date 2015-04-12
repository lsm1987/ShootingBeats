using UnityEngine;
using UnityEngine.UI;

public class UIBeatReady : UIWindow
{
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

    public void Open(BeatInfo beatInfo)
    {
        // 정보 채우기
        _beatInfo = beatInfo;
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
        GlobalSystem._Instance._LoadingBeatInfo = _beatInfo;
        Application.LoadLevel(SceneName._Stage);
    }
}
