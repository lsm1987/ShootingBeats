using UnityEngine;

namespace Game
{
    public class Layout : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _leftBox = null;
        private RectTransform _LeftBox { get { return _leftBox; } }
        [SerializeField]
        private RectTransform _rightBox = null;
        private RectTransform _RightBox { get { return _rightBox; } }
        [SerializeField]
        private RectTransform _topBox = null;
        private RectTransform _TopBox { get { return _topBox; } }
        [SerializeField]
        private RectTransform _bottomBox = null;
        private RectTransform _BottomBox { get { return _bottomBox; } }
        [SerializeField]
        private RectTransform _gameArea = null;
        public RectTransform _GameArea { get { return _gameArea; } }

        public void Initialize(float gameAreaWidth, float topBoxHeightRate, float bottomBoxHeightRate)
        {
            _TopBox.anchorMin = new Vector2(0.5f, 1f - topBoxHeightRate);
            UIUtil.SetWidth(_TopBox, gameAreaWidth);

            _BottomBox.anchorMax = new Vector2(0.5f, bottomBoxHeightRate);
            UIUtil.SetWidth(_BottomBox, gameAreaWidth);

            _LeftBox.offsetMax = new Vector2(-gameAreaWidth / 2.0f, 0.0f);
            _RightBox.offsetMin = new Vector2(gameAreaWidth / 2.0f, 0.0f);

            _GameArea.anchorMax = new Vector2(0.5f, 1f - topBoxHeightRate);
            _GameArea.anchorMin = new Vector2(0.5f, bottomBoxHeightRate);
            UIUtil.SetWidth(_GameArea, gameAreaWidth);
        }
    }
}