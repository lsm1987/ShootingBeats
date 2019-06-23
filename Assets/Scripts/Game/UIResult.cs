using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    // 게임 결과
    public class UIResult : UIWindow
    {
        private const string _resultClear = "Clear!";
        private const string _resultGameOver = "Game Over";

        [SerializeField]
        private Text _result = null;
        [SerializeField]
        private Text _songTitle = null;
        [SerializeField]
        private Text _score = null;
        [SerializeField]
        private Text _highScore = null;
        [SerializeField]
        private GameObject _newRecord = null;
        [SerializeField]
        private Button _leaderboard = null;
        private BeatInfo _beatInfo;

        public override bool OnKeyInput()
        {
            if (Input.GetButtonDown(ButtonName._start) || Input.GetKeyDown(KeyCode.Escape))
            {
                OnRetryClicked();
            }
            return true;
        }

        public void SetData(BeatInfo beatInfo, bool cleared, int score, int highScore, bool isNewRecord)
        {
            _beatInfo = beatInfo;
            _result.text = cleared ? _resultClear : _resultGameOver;
            _songTitle.text = _beatInfo._title;
            _score.text = score.ToString();
            _highScore.text = highScore.ToString();
            _newRecord.SetActive(isNewRecord);
            _leaderboard.interactable = (GlobalSystem._Instance != null && GlobalSystem._Instance._IsAuthenticated); // 로그인 되었을 때만 사용가능
        }

        public void Open()
        {
            _Go.SetActive(true);
        }

        private void Close()
        {
            _Go.SetActive(false);
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

        /// <summary>
        /// 스테이지 재시작
        /// </summary>
        public void OnRetryClicked()
        {
            Close();
            if (GameSystem._Instance != null)
            {
                GameSystem._Instance.Retry();
            }
        }

        /// <summary>
        /// 음악 목록으로
        /// </summary>
        public void OnBeatListClicked()
        {
            SceneManager.LoadScene(SceneName._BeatList);
        }
    }
}