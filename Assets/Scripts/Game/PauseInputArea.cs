using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// 일시정지 영역
    /// </summary>
    public class PauseInputArea : MonoBehaviour
    {
        [SerializeField]
        private GameObject _area = null;

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
            if (_area != null)
            {
                _area.SetActive(visible);
            }
        }

        public void SetArea(float heightRate)
        {
            var rectTrans = GetComponent<RectTransform>();

            if (rectTrans)
            {
                float safeAreaAnchorMaxY = (UIUtil.SafeArea.y + UIUtil.SafeArea.height) / Screen.height;
                rectTrans.anchorMin = new Vector2(0.0f, 1.0f - heightRate);
                rectTrans.anchorMax = new Vector2(1.0f, safeAreaAnchorMaxY);
            }
        }
    }
}