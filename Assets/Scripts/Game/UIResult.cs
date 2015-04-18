﻿using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    // 게임 결과
    public class UIResult : UIWindow
    {
        private const string _resultClear = "Clear!";
        private const string _resultGameOver = "Game Over";

        [SerializeField]
        private Text _result;
        [SerializeField]
        private Text _songTitle;
        [SerializeField]
        private Text _score;
        [SerializeField]
        private Text _highScore;
        [SerializeField]
        private GameObject _newRecord;

        public override bool OnKeyInput()
        {
            // 다른 UI로 입력 넘어가지 못하게 막음
            return true;
        }

        public void SetData(bool cleared, string songTitle, int score, int highScore, bool isNewRecord)
        {
            _result.text = cleared ? _resultClear : _resultGameOver;
            _songTitle.text = songTitle;
            _score.text = score.ToString();
            _highScore.text = highScore.ToString();
            _newRecord.SetActive(isNewRecord);
        }

        public void Open()
        {
            _Go.SetActive(true);
        }

        private void Close()
        {
            _Go.SetActive(false);
        }

        public void OnRetryClicked()
        {
            // todo
        }

        public void OnBeatListClicked()
        {
            // todo
        }
    }
}