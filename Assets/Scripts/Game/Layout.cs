using UnityEngine;

namespace Game
{
    public class Layout : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _leftBox;
        private RectTransform _LeftBox { get { return _leftBox; } }
        [SerializeField]
        private RectTransform _rightBox;
        private RectTransform _RightBox { get { return _rightBox; } }
        [SerializeField]
        private RectTransform _bottomBox;
        private RectTransform _BottomBox { get { return _bottomBox; } }
        [SerializeField]
        private RectTransform _gameArea;
        public RectTransform _GameArea { get { return _gameArea; } }

        public void Initialize(float refDeviceWidth, float bottomBoxHeightScreenRate)
        {
            _BottomBox.anchorMax = new Vector2(0.5f, bottomBoxHeightScreenRate);
            UIUtil.SetWidth(_BottomBox, refDeviceWidth);

            _LeftBox.offsetMax = new Vector2(-refDeviceWidth / 2.0f, 0.0f);
            _RightBox.offsetMin = new Vector2(refDeviceWidth / 2.0f, 0.0f);

            _GameArea.anchorMin = new Vector2(0.5f, bottomBoxHeightScreenRate);
            UIUtil.SetWidth(_GameArea, refDeviceWidth);
        }
    }
}