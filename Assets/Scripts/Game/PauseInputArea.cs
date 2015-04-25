using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// 일시정지 영역
    /// </summary>
    public class PauseInputArea : MonoBehaviour
    {
        public void OnClicked()
        {
            if (GameSystem._Instance != null)
            {
                GameSystem._Instance.StartPause();
            }
        }

        /// <summary>
        /// 시각화
        /// </summary>
        /// <param name="visible"></param>
        public void SetVisible(bool visible)
        {
            Image img = GetComponent<Image>();
            if (img != null)
            {
                Color color = img.color;
                color.a = (visible) ? 0.1f : 0.0f;
                img.color = color;
            }
        }
    }
}