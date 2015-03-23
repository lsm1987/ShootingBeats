using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    // 게임 내 UI 관리
    public class UISystem : MonoBehaviour
    {
        // 레터박스 UI가 붙을 오브젝트
        [SerializeField]
        private GameObject _letterBox;
        
        // 이동 입력 영역
        [SerializeField]
        private MoveInputArea _moveInputArea;
        public MoveInputArea _MoveInputArea { get { return _moveInputArea; } }

        /// <summary>
        /// 레터박스 UI 추가
        /// </summary>
        /// <param name="horizontal">수평방향 레터박스인가?</param>
        /// <param name="screenRate">레터박스가 가릴 화면의 비율. 수직방향이면 절반씩 사용</param>
        public void CreateLetterBox(bool horizontal, float screenRate)
        {
            Color color = new Color(0.16f, 0.16f, 0.16f);
            if (horizontal)
            {
                GameObject obj = new GameObject();
                obj.name = "Horizontal";
                obj.transform.SetParent(_letterBox.transform, false);
                
                RectTransform rectTrans = obj.AddComponent<RectTransform>();
                rectTrans.anchorMin = Vector2.zero;
                rectTrans.anchorMax = new Vector2(1.0f, screenRate);
                rectTrans.offsetMin = Vector2.zero;
                rectTrans.offsetMax = Vector2.zero;
                rectTrans.pivot = new Vector2(0.5f, 0.5f);

                obj.AddComponent<CanvasRenderer>();
                Image image = obj.AddComponent<Image>();
                image.color = color;
            }
            else
            {
                // 0: 왼쪽, 1: 오른쪽
                for (int i = 0; i < 2; ++i)
                {
                    GameObject obj = new GameObject();
                    obj.name = "Vertical_" + i.ToString();
                    obj.transform.SetParent(_letterBox.transform, false);

                    RectTransform rectTrans = obj.AddComponent<RectTransform>();
                    if (i == 0)
                    {
                        rectTrans.anchorMin = Vector2.zero;
                        rectTrans.anchorMax = new Vector2(screenRate / 2.0f, 1.0f);
                    }
                    else
                    {
                        rectTrans.anchorMin = new Vector2(1.0f - screenRate / 2.0f, 0.0f);
                        rectTrans.anchorMax = Vector2.one;
                    }
                    rectTrans.offsetMin = Vector2.zero;
                    rectTrans.offsetMax = Vector2.zero;
                    rectTrans.pivot = new Vector2(0.5f, 0.5f);

                    obj.AddComponent<CanvasRenderer>();
                    Image image = obj.AddComponent<Image>();
                    image.color = color;
                }
            }
        } // CreateLetterBox()
    }
}