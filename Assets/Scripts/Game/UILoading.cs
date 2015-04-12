using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    // 게임 씬 로딩
    public class UILoading : UIWindow
    {
        [SerializeField]
        private Text _progress; // 진행상태

        public override bool OnKeyInput()
        {
            // 다른 UI로 입력 넘어가지 못하게 막음
            return true;
        }

        /// <summary>
        /// 진행상태 문자열 지정
        /// </summary>
        /// <param name="text"></param>
        public void SetProgress(string text)
        {
            _progress.text = text;
        }

        /// <summary>
        /// 창 열기
        /// </summary>
        public void Open()
        {
            _progress.text = string.Empty;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 창 닫기
        /// </summary>
        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}