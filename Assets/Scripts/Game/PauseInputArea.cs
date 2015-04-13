using UnityEngine;

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
    }
}