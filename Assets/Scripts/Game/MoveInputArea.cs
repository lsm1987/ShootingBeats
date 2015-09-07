using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class MoveInputArea : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private RectTransform _rectTrans = null;
        private bool _touching = false;
        private int _pointerID;
        private Vector2 _lastPos; // 마지막 갱신 시 터치 월드좌표
        private Vector2 _curPos;
        [SerializeField]
        private GameObject _area;
        [SerializeField]
        private RectTransform _cursor;   // 시각화용 오브젝트

        private void Start()
        {
            _rectTrans = GetComponent<RectTransform>();
            _touching = false;
            _lastPos = Vector2.zero;
            _curPos = Vector2.zero;
        }

        public void OnPointerDown(PointerEventData data)
        {
            if (!_touching)
            {
                _touching = true;
                _pointerID = data.pointerId;

                Vector3 worldPos;
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectTrans, data.position, data.pressEventCamera, out worldPos))
                {
                    _lastPos = worldPos;
                    _curPos = worldPos;
                }
                else
                {
                    _lastPos = Vector2.zero;
                    _curPos = Vector2.zero;
                }

                if (_cursor != null)
                {
                    Vector2 localPos;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTrans, data.position, data.pressEventCamera, out localPos);
                    _cursor.gameObject.SetActive(true);
                    _cursor.localPosition = localPos;
                }
            }
        }

        public void OnDrag(PointerEventData data)
        {
            if (_touching && data.pointerId == _pointerID)
            {
                Vector3 worldPos;
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectTrans, data.position, data.pressEventCamera, out worldPos))
                {
                    _curPos = worldPos;
                }
                else
                {
                    _curPos = _lastPos;
                }

                if (_cursor != null)
                {
                    Vector2 localPos;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTrans, data.position, data.pressEventCamera, out localPos);
                    _cursor.gameObject.SetActive(true);
                    _cursor.localPosition = localPos;
                }
            }
        }

        public void OnPointerUp(PointerEventData data)
        {
            if (_touching && data.pointerId == _pointerID)
            {
                _touching = false;

                if (_cursor != null)
                {
                    _cursor.gameObject.SetActive(false);
                }
            }
        }

        private void LateUpdate()
        {
            // 이벤트 -> Update -> LateUpdate 순서로 호출
            // 게임 로직의 Update에서 Delta를 사용하므로 LateUpdate에서 다음 프레임 준비
            if (_touching)
            {
                _lastPos = _curPos;
            }
        }

        public Vector2 GetDelta()
        {
            if (_touching)
            {
                return (_curPos - _lastPos);
            }
            else
            {
                return Vector2.zero;
            }
        }

        public bool IsTouching()
        {
            return _touching;
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