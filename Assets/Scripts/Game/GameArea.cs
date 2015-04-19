using UnityEngine;

namespace Game
{
    /// <summary>
    /// 게임영역에 일치하는 UI 영역
    /// </summary>
    public class GameArea : MonoBehaviour
    {
        /// <summary>
        /// 레터박스에 가려지지 않은 게임영역에 맞춰 UI 조정
        /// </summary>
        /// <param name="horizontalLetterBox">수평방향 레터박스인가?</param>
        /// <param name="letterBoxScreenRate">레터박스가 가릴 화면의 비율. 수직방향이면 절반씩 사용</param>
        public void SetArea(bool isHorizontalLetterBox, float letterBoxScreenRate)
        {
            RectTransform rectTrans = GetComponent<RectTransform>();
            if (isHorizontalLetterBox)
            {
                rectTrans.anchorMin = new Vector2(0.0f, letterBoxScreenRate);
                rectTrans.anchorMax = Vector2.one;
            }
            else
            {
                rectTrans.anchorMin = new Vector2(letterBoxScreenRate / 2.0f, 0.0f);
                rectTrans.anchorMax = new Vector2(1.0f - letterBoxScreenRate / 2.0f, 1.0f);
            }
            rectTrans.pivot = new Vector2(0.5f, 0.5f);
        }
    }
}