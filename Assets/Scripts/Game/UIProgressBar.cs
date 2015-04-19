using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    // 게임 진행율
    public class UIProgressBar : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _rectRemain;  // 남은 양
        [SerializeField]
        private RectTransform _rectProgress;    // 진행한 양

        private float _lastRate = 0.0f;

        /// <summary>
        /// 진행율 지정
        /// </summary>
        /// <param name="rate">진행율. 0.0 ~ 1.0</param>
        public void SetRate(float rate)
        {
            rate = Mathf.Clamp01(rate);
            if (rate != _lastRate)
            {
                _rectRemain.anchorMin = new Vector2(rate, 0.0f);
                _rectProgress.anchorMax = new Vector2(rate, 1.0f);
                _lastRate = rate;
            }
        }
    }
}