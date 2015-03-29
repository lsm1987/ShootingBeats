﻿using UnityEngine;
using UnityEngine.UI;

public class UIBeatReady : MonoBehaviour
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

    public void Open(BeatInfo beatInfo)
    {
        // 정보 채우기
        _title.text = beatInfo._title;
        _difficulty.text = beatInfo._difficulty.ToString();
        _length.text = Util.ConverBeatLength(beatInfo._length);

        // 활성화
        gameObject.SetActive(true);
    }

    public void OnBackClicked()
    {
        // 비활성화
        gameObject.SetActive(false);
    }

    // 게임 씬으로 이동
    public void OnPlayClicked()
    {
        Application.LoadLevel("Stage");
    }
}
