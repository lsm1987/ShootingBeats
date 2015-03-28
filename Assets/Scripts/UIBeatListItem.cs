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

    public void SetBeatInfo(BeatInfo beatInfo)
    {
        _title.text = beatInfo._title;
        _difficulty.text = beatInfo._difficulty.ToString();
        int lengthMin = beatInfo._length / 60; // 분
        int lengthSec = beatInfo._length % 60; // 초
        _length.text = lengthMin.ToString() + ":" + lengthSec.ToString("00");
    }
}
