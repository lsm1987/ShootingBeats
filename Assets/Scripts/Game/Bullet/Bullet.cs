using UnityEngine;

namespace Game
{
    // 탄
    public class Bullet : Mover
    {
        public float _angleRate; // 각속도. 단위 도
        public float _speed; // 속도
        public float _speedRate; // 가속도

        public Bullet()
            : base()
        {
        }

        public void Init(string shapeSubPath, float x, float y, float angle
            , float angleRate, float speed, float speedRate)
        {
            base.Init(shapeSubPath, x, y, angle);
            _angleRate = angleRate;
            _speed = speed;
            _speedRate = speedRate;
        }
        
        public override void Move()
        {
            float rad = _angle * Mathf.PI * 2.0f; // 라디안으로 변환
            
            _X += _speed * Mathf.Cos(rad); // 이동
            _Y += _speed * Mathf.Sin(rad);

            _angle += _angleRate; // 각도에 각속도 가산
            _speed += _speedRate; // 속도에 가속도 가산

            if (!IsInStage()) // 스테이지 영역 밖으로 나가면 삭제
            {
                _alive = false;
            }
        }
    }
}