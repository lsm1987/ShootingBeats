using UnityEngine;

namespace Game
{
    namespace Calc
    {
        public class SlowPlacedBullet : PlacedBullet
        {
            private float _minSpeed = float.MinValue;
            private float _minSpeed2 = float.MinValue;
            private int _explosionAbsFrame = 0;   // 이 절대 프레임이 되면 폭발 단계로. 음수 지정 시 폭발 단계는 무시
            private float _explosionSpeed = 0.0f;

            public void Init(string shapeSubPath, float x, float y
                , float angle1, float angleRate1, float speed1, float speedRate1
                , int moveDuration, int stopDuration
                , float angle2, float angleRate2, float speed2, float speedRate2, float minSpeed2
                , int explosionAbsFrame, float explosionSpeed)
            {
                base.Init(shapeSubPath, x, y
                    , angle1, angleRate1, speed1, speedRate1
                    , moveDuration, stopDuration
                    , angle2, angleRate2, speed2, speedRate2);

                _minSpeed = float.MinValue;
                _minSpeed2 = minSpeed2;
                _explosionAbsFrame = explosionAbsFrame;
                _explosionSpeed = explosionSpeed;
            }

            protected override void OnMove2Started()
            {
                _minSpeed = _minSpeed2;
            }
            
            protected override void OnUpdateMoveState()
            {
                if (_explosionAbsFrame >= 0 && _Frame == _explosionAbsFrame)
                {
                    _minSpeed = float.MinValue;
                    _speed = _explosionSpeed;
                    _speedRate = 0.0f;
                    // 방향은 유지
                    _angleRate = 0.0f;
                }
            }

            protected override void OnSpeedUpdated()
            {
                _speed = Mathf.Max(_speed, _minSpeed);
            }
        }
    }
}