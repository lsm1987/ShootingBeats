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
        private GameObject _area;

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
    }
}