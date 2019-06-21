using UnityEngine;

namespace Game
{
    /// <summary>
    /// 좌표 설치탄
    /// <para>특정 좌표에 정지했다가 다시 움직임</para>
    /// </summary>
    public class PosPlacedBullet : Bullet
    {
        private int _selfFrame = 0;     // 생성 후 상대적인 프레임
        private Vector2 _targetPos;     // 목표로 할 좌표
        private int _moveDuration = 0;  // 처음 이동할 기간
        private int _stopDuration = 0;  // 정지해 있을 기간
        private float _angle2 = 0.0f;
        private float _angleRate2 = 0.0f;
        private float _speed2 = 0.0f;   // 정지 후 다시 움직일 때 속도
        private float _speedRate2 = 0.0f;

        public void Init(string shapeSubPath, float x, float y
            , Vector2 targetPos, float speed1, float speedRate1
            , int moveDuration, int stopDuration
            , float angle2, float angleRate2, float speed2, float speedRate2)
        {
            // 목표지점을 향하는 각도
            float angle1 = BaseGameLogic.CalcluatePointToPointAngle(new Vector2(x, y), targetPos);

            base.Init(shapeSubPath, x, y, angle1, 0.0f, speed1, speedRate1);
            _selfFrame = 0;
            _targetPos = targetPos;
            _moveDuration = moveDuration;
            _stopDuration = stopDuration;
            _angle2 = angle2;
            _angleRate2 = angleRate2;
            _speed2 = speed2;
            _speedRate2 = speedRate2;
        }

        public override void Move()
        {
            // 이동 시간이 끝나면 속도 0으로 하여 멈추고 좌표 강제지정
            if (_selfFrame == _moveDuration)
            {
                _speed = 0.0f;
                _X = _targetPos.x;
                _Y = _targetPos.y;
            }
            
            // 정지 시간이 지나면 속도, 각도 재지정
            if (_selfFrame == (_moveDuration + _stopDuration))
            {
                _angle = _angle2;
                _angleRate = _angleRate2;
                _speed = _speed2;
                _speedRate = _speedRate2;

                OnMove2Started();
            }

            OnUpdateMoveState();

            // 프레임 갱신
            ++_selfFrame;

            base.Move();
        }

        protected virtual void OnMove2Started() { }
        protected virtual void OnUpdateMoveState() { }
    }
}
