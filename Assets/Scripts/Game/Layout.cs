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
        private RectTransform _centerBox = null;
        private RectTransform _CenterBox { get { return _centerBox; } }
        [SerializeField]
        private RectTransform _topBox = null;
        private RectTransform _TopBox { get { return _topBox; } }
        [SerializeField]
        private RectTransform _bottomBox = null;
        private RectTransform _BottomBox { get { return _bottomBox; } }
        [SerializeField]
        private RectTransform _gameArea = null;
        public RectTransform _GameArea { get { return _gameArea; } }

        public void Initialize(float centerBoxWidthRate, float topBoxHeightRate, float bottomBoxHeightRate)
        {
            float sideBoxRate = (1f - centerBoxWidthRate) / 2f;
            _LeftBox.anchorMax = new Vector2(sideBoxRate, 1f);
            _RightBox.anchorMin = new Vector2(1f - sideBoxRate, 0f);

            _CenterBox.anchorMin = new Vector2(sideBoxRate, 0f);
            _CenterBox.anchorMax = new Vector2(1f - sideBoxRate, 1f);

            _TopBox.anchorMin = new Vector2(0f, 1f - topBoxHeightRate);

            _BottomBox.anchorMax = new Vector2(1f, bottomBoxHeightRate);

            _GameArea.anchorMin = new Vector2(0f, bottomBoxHeightRate);
            _GameArea.anchorMax = new Vector2(1f, 1f - topBoxHeightRate);
        }
    }
}