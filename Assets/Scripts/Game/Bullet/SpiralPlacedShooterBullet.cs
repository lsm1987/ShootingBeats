using UnityEngine;

namespace Game
{
    /// <summary>
    /// 소용돌이 설치탄을 발사하는 탄
    /// </summary>
    public class SpiralPlacedShooterBullet : Bullet
    {
        private float _orbitAngle; // 탄도 상의 슈터 각도
        private float _orbitAngleRate;  // 탄도 상의 슈터 각속도
        private float _orbitRadius; // 탄도 반경
        private int _shotTime;  // 한 그룹을 발사할 때 얼마동안 발사 할 것인가?
        private int _waitTime;  // 그룹 내 발사 후 대기시간
        private int _interval;  // 그룹 내 발사 간격
        private int _cycle; // 전체 주기
        private int _time;
        private string _bulletShape;
        private float _bulletSpeed;
        private int _groupCount;
        private const float _playerSize = 0.10f; // 플레이어기 크기(Hit 영역과는 다르다)

        public void Init(string shape, float orbitAngle, float orbitAngleRate, float orbitRadius,
            int shotTime, int waitTime, int interval, int cycle,
            string bulletShape, float bulletSpeed, int groupCount)
        {
            _orbitAngle = orbitAngle;
            _orbitAngleRate = orbitAngleRate;
            _orbitRadius = orbitRadius;
            _shotTime = shotTime;
            _waitTime = waitTime;
            _interval = interval;
            _cycle = cycle;
            _time = 0;
            _bulletShape = bulletShape;
            _bulletSpeed = bulletSpeed;
            _groupCount = groupCount;

            // 탄도 상의 각도와 반경으로 슈터 좌표 초기화
            float rad = _orbitAngle * Mathf.PI * 2.0f;
            float x = Mathf.Cos(rad) * _orbitRadius;
            float y = Mathf.Sin(rad) * _orbitRadius;

            base.Init(shape, x, y, 0.0f, 0.0f, 0.0f, 0.0f);
        }

        public override void Move()
        {
            // 탄도 상의 각도와 반경으로 슈터 좌표 설정
            float rad = _orbitAngle * Mathf.PI * 2.0f;
            _X = Mathf.Cos(rad) * _orbitRadius;
            _Y = Mathf.Sin(rad) * _orbitRadius;

            // 탄도 상의 각도 갱신
            _orbitAngle += _orbitAngleRate;
            _orbitAngle -= Mathf.Floor(_orbitAngle);

            // 화면 바깥쪽을 향해서 방향탄 발사
            if (_time % 5 == 0)
            {
                Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init(_bulletShape, _X, _Y, _orbitAngle, 0.0f, _bulletSpeed * 1.5f, 0.0f);
            }

            // 설치탄 발사
            int count = _time / (_shotTime + _waitTime);    // 그룹 번호
            int time = _time % (_shotTime + _waitTime); // 그룹 내 시간
            float baseTime = (_orbitRadius - _playerSize) / _bulletSpeed;   // 탄이 화면 중앙에 도달할 때 까지의 시간

            // 지정돤 그룹 수만큼 반복
            if (count < _groupCount)
            {
                // 발사 시간만큼 정해진 간격으로 탄 발사
                if ((time < _shotTime) && (time % _interval == 0))
                {
                    // 설치탄 생성
                    // 나중에 쏜 탄일수록 화면 중앙에 가깝게 함
                    PlacedBullet b = GameSystem._Instance.CreateBullet<PlacedBullet>();
                    b.Init(_bulletShape, _X, _Y, _orbitAngle + 0.5f, _bulletSpeed
                        , (int)(baseTime * count / _groupCount)
                        , (int)(baseTime + (_shotTime + _waitTime) * (_groupCount - count)));
                }
            }

            // 타이머 갱신
            _time = (_time + 1) % _cycle;

            // 삭제는 외부에서 지정
        }
    }
}
