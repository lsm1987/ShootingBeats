using UnityEngine;

namespace Game
{
    public class LetterBox : MonoBehaviour
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

        public void InitializeLetterBox(float refDeviceWidthRatio, float refDeviceHeightRatio
            , float gameWorldWidth, float gameWorldHeight
            , float referenceResolutionHeight)
        {
            // 기준 기기화면 상단에 가로가 가득차도록 게임영역 배치
            float gameHeightRatio = refDeviceWidthRatio * (gameWorldHeight / gameWorldWidth); // 기기에서 게임화면 높이의 비
            float bottomBoxHeightRatio = refDeviceHeightRatio - gameHeightRatio;
            float bottomBoxHeightScreenRate = bottomBoxHeightRatio / refDeviceHeightRatio;
            _BottomBox.anchorMax = new Vector2(0.5f, bottomBoxHeightScreenRate);

            float refDeviceWidth = referenceResolutionHeight * (refDeviceWidthRatio / refDeviceHeightRatio);
            UIUtil.SetWidth(_BottomBox, refDeviceWidth);
            _LeftBox.offsetMax = new Vector2(-refDeviceWidth / 2.0f, 0.0f);
            _RightBox.offsetMin = new Vector2(refDeviceWidth / 2.0f, 0.0f);
        }
    }
}