using UnityEngine;

namespace Game
{
    // 게임 개발 중 테스트를 위한 정보
    public class TestInfo : MonoBehaviour
    {
        [SerializeField]
        private bool _isInvincible = false; // 무적상태인가?
        public bool _IsInvincible { get { return _isInvincible; } }
        [SerializeField]
        private int _startFrame = 0;    // 몇 프레임부터 시작할 것인가?
        public int _StartFrame { get { return _startFrame; } }
        [SerializeField]
        private BeatInfo _beatInfo = null;  // 이 씬에서 바로 시작할 때 사용할 음악 정보
        public BeatInfo _BeatInfo { get { return _beatInfo; } }
    }
}