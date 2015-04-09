using UnityEngine;
using UnityEngine.UI;

// 노래 목록의 한 요소
public class UIBeatListItem : MonoBehaviour
{
    [SerializeField]
    private Text _title;
    [SerializeField]
    private Text _difficulty;
    [SerializeField]
    private Text _length;
    private int _index; // 목록 내 인덱스
    private UIBeatList.InfoSelectedHandler _selectedHandler;

    public void SetBeatInfo(int index_, UIBeatList.InfoSelectedHandler selectedHandler_, BeatInfo beatInfo)
    {
        _index = index_;
        _selectedHandler = selectedHandler_;

        _title.text = beatInfo._title;
        _difficulty.text = beatInfo._difficulty.ToString();
        _length.text = Define.ConverBeatLength(beatInfo._length);
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
