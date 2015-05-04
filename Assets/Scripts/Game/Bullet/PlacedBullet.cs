using UnityEngine;

namespace Game
{
    /// <summary>
    /// 설치탄
    /// <para>정지했다가 다시 움직임</para>
    /// </summary>
    public class PlacedBullet : Bullet
    {
        private int _selfFrame = 0;      // 생성 후 상대적인 프레임
        private int _moveDuration = 0;  // 처음 이동할 기간
        private float _stopDuration = 0;    // 정지해 있을 기간
        private float _angle2 = 0.0f;
        private float _speed2 = 0.0f; // 정지 후 다시 움직일 때 속도

        public void Init(string shapeSubPath, float x, float y, float angle
            , float speed1
            , int moveDuration, int stopDuarion
            , float angle2, float speed2)
        {
            base.Init(shapeSubPath, x, y, angle, 0.0f, speed1, 0.0f);
            _selfFrame = 0;
            _angle2 = angle2;
            _speed2 = speed2;
            _moveDuration = moveDuration;
            _stopDuration = stopDuarion;
        }

        /// <summary>
        /// 정지 전과 동일한 속도, 각도 사용하는 초기화
        /// </summary>
        public void Init(string shapeSubPath, float x, float y, float angle
            , float speed1
            , int moveDuration, int stopDuarion)
        {
            Init(shapeSubPath, x, y, angle
                , speed1
                , moveDuration, stopDuarion
                , angle, speed1);
        }

        /// <summary>
        /// 정지 없이 속도, 각도 변경
        /// </summary>
        public void InitNoStop(string shapeSubPath, float x, float y, float angle
            , float speed1
            , int moveDuration
            , float angle2, float speed2)
        {
            Init(shapeSubPath, x, y, angle
                , speed1
                , moveDuration, 0
                , angle2, speed2);
        }

        public override void Move()
        {
            // 이동 시간이 끝나면 속도 0으로 하여 멈춤
            if (_selfFrame == _moveDuration)
            {
                _speed = 0.0f;
            }
            
            // 전지 시간이 지나면 속도, 각도 재지정
            if (_selfFrame == (_moveDuration + _stopDuration))
            {
                _angle = _angle2;
                _speed = _speed2;
            }
            
            // 프레임 갱신
            ++_selfFrame;

            base.Move();
        }
    }
}
