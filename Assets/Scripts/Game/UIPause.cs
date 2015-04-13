using UnityEngine;

namespace Game
{
    public class UIPause : UIWindow
    {
        public void Open()
        {
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

        /// <summary>
        /// 창 비활성화
        /// </summary>
        private void Close()
        {
            _Go.SetActive(false);
            if (GameSystem._Instance != null)
            {
                GameSystem._Instance.StopPause();
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
        /// 옵션창 열기
        /// </summary>
        public void OnOptionClicked()
        {
            _UISystem.OpenWindow(Define._uiOptionPath);
        }

        /// <summary>
        /// 음악 목록으로
        /// </summary>
        public void OnBeatListClicked()
        {
            Application.LoadLevel(SceneName._BeatList);
        }

        /// <summary>
        /// 게임으로 돌아가기
        /// </summary>
        public void OnReturnToGameClicked()
        {
            Close();
        }
    }
}