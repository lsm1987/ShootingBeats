using UnityEngine;

namespace Game
{
    /// <summary>
    /// 속성을 2단계로 지정하는 탄
    /// </summary>
    public class TwoPhaseBullet : Bullet
    {
        private int _phase2Frame = 0;     // 2단계로 변경할 프레임
        private float _angle2 = 0.0f;
        private float _speed2 = 0.0f;

        public void Init(string shapeSubPath, float x, float y, float angle
            , float angleRate, float speed, float speedRate
            , int phase1Duration, float angle2, float speed2)
        {
            base.Init(shapeSubPath, x, y, angle, angleRate, speed, speedRate);
            _phase2Frame = _Frame + phase1Duration;
            _angle2 = angle2;
            _speed2 = speed2;
        }

        public override void Move()
        {
            // 프레임 도달하면 속성 변경
            if (_Frame == _phase2Frame)
            {
                _angle = _angle2;
                _speed = _speed2;
            }
            base.Move();
        }
    }
}
