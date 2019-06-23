using UnityEngine;
using UnityEngine.UI;

// 노래 목록의 한 요소
public class UIBeatListItem : MonoBehaviour
{
    [SerializeField]
    private Image _albumArt = null;
    [SerializeField]
    private Text _title = null;
    [SerializeField]
    private UIDifficultyIcon _difficulty = null;
    [SerializeField]
    private Text _length = null;
    [SerializeField]
    private GameObject _cleared = null;
    private int _index; // 목록 내 인덱스
    private UIBeatList.InfoSelectedHandler _selectedHandler;

    public void SetBeatInfo(int index_, UIBeatList.InfoSelectedHandler selectedHandler_, BeatInfo beatInfo)
    {
        _index = index_;
        _selectedHandler = selectedHandler_;

        _albumArt.sprite = Define.GetAlbumArtSprite(beatInfo);
        _title.text = beatInfo._title;
        _difficulty.SetDifficulty(beatInfo._difficulty);
        _length.text = Define.ConverBeatLength(beatInfo._length);
        _cleared.SetActive(Define.IsSongCleared(beatInfo));
    }

    public void OnClicked()
    {
        // 자신이 선택되었음을 알림
        if (_selectedHandler != null)
        {
            _selectedHandler(_index);
        }
    }
}
