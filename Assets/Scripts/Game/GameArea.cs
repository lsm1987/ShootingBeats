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
        public void Initialize(float refDeviceWidth, float bottomBoxHeightScreenRate)
        {
            RectTransform rectTrans = GetComponent<RectTransform>();
            rectTrans.anchorMin = new Vector2(0.5f, bottomBoxHeightScreenRate);
            UIUtil.SetWidth(rectTrans, refDeviceWidth);
        }
    }
}