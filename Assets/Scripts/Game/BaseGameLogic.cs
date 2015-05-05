using UnityEngine;
using System.Collections;

namespace Game
{
    /// <summary>
    /// 음악별 다른 동작 정의
    /// </summary>
    public abstract class BaseGameLogic
    {
        // 특화 정보 로딩
        public abstract IEnumerator LoadContext();

        // 특화 정보 갱신
        public abstract void UpdatePlayContext();

        // 기본 플레이어기 로딩
        protected IEnumerator LoadBasicPlayer()
        {
            // 플레이어기 로딩 /////////////////
            GameSystem._Instance._UILoading.SetProgress("Loading Player");
            yield return null;
            GameSystem._Instance.PoolStackShape("Common/Player_Black", 1);
            GameSystem._Instance.PoolStackMover<PlayerAlive>(1);
            GameSystem._Instance.PoolStackShape("Common/Player_Crash", 1);
            GameSystem._Instance.PoolStackMover<PlayerCrash>(1);

            // 샷 로딩 ///////////////////////
            GameSystem._Instance._UILoading.SetProgress("Loading Shots");
            yield return null;
            GameSystem._Instance.PoolStackShape("Common/Shot_Black", 36);
            GameSystem._Instance.PoolStackMover<Shot>(36);
        }

        #region Util
        /// <summary>
        /// 지정한 좌표로부터 플레이어로 향하는 각도 구하기
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public float GetPlayerAngle(float x, float y)
        {
            // Atan2 의 결과가 라디안이므로 0~1로 변경
            Vector2 playerPos = GameSystem._Instance._Player._pos;
            return Mathf.Atan2(playerPos.y - y, playerPos.x - x) / Mathf.PI / 2.0f;
        }

        /// <summary>
        /// 지정한 무버로부터 플레이어로 향하는 각도 구하기
        /// </summary>
        /// <param name="startMover"></param>
        /// <returns></returns>
        public float GetPlayerAngle(Mover startMover)
        {
            return GetPlayerAngle(startMover._X, startMover._Y);
        }
        #endregion Util

        #region Pattern
        /// <summary>
        /// 무버를 지정한 위치까지 등속 이동
        /// </summary>
        public IEnumerator MoveConstantVelocity(Mover mover, Vector2 arrivePos, int duration)
        {
            Vector2 delta = (arrivePos - mover._pos) / (float)duration; // 한 프레임에 움직일 거리

            for (int i = 0; i < duration - 1; ++i)
            {
                mover._pos += delta;
                yield return null;
            }

            // 마지막 프레임
            mover._pos = arrivePos;
        }

        /// <summary>
        /// 무버를 지정한 위치까지 비례감속 이동
        /// </summary>
        public IEnumerator MoveDamp(Mover mover, Vector2 arrivePos, int duration, float damp)
        {
            for (int i = 0; i < duration - 1; ++i)
            {
                mover._pos = Vector2.Lerp(mover._pos, arrivePos, damp);
                yield return null;
            }

            // 마지막 프레임
            mover._pos = arrivePos;
        }

        /// <summary>
        /// N-Way 탄
        /// </summary>
        public void NWayBullet(Mover mover, string shape, float angle, float angleRange, float speed, int count)
        {
            if (count > 1)
            {
                for (int i = 0; i < count; ++i)
                {
                    // (angle - angleRange / 2) ~ (angle + angleRange / 2) 범위에서 count 만큼 생성
                    Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                    b.Init(shape, mover._X, mover._Y, angle + angleRange * ((float)i / (count - 1) - 0.5f), 0.0f, speed, 0.0f);
                }
            }
            else if (count == 1)
            {
                // 탄 수가 하나일 때는 발사 각도로 1개 발사
                Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init(shape, mover._X, mover._Y, angle, 0.0f, speed, 0.0f);
            }
        }

        /// <summary>
        /// 무작위로 구멍 뚫린 N-Way
        /// </summary>
        public IEnumerator GapBullets(Mover mover, string shape, float angleRange, float speed, int count
            , int interval, int repeatCount)
        {
            for (int i = 0; i < repeatCount; ++i)
            {
                float angle = GameSystem._Instance.GetRandom01();
                NWayBullet(mover, shape, angle, angleRange, speed, count);
                yield return new WaitForFrames(interval);
            }
        }

        public IEnumerator CustomGapBullets(Mover mover, string shape, float angleRange, float speed, int count
            , int interval, float[] angles)
        {
            for (int i = 0; i < angles.Length; ++i)
            {
                NWayBullet(mover, shape, angles[i], angleRange, speed, count);
                yield return new WaitForFrames(interval);
            }
        }

        /// <summary>
        /// 원형탄
        /// </summary>
        public void CircleBullet(Mover mover, string shape, float angle, float speed, int count, bool halfAngleOffset)
        {
            float angleStart = angle + ((halfAngleOffset) ? (1.0f / count / 2.0f) : 0.0f);
            for (int i = 0; i < count; ++i)
            {
                Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init(shape, mover._X, mover._Y, angleStart + (1.0f / count * i), 0.0f, speed, 0.0f);
            }
        }

        public IEnumerator CircleBullets(Mover mover, string shape, float angle, float speed, int count, bool halfAngleOffset
            , int interval, int repeatCount)
        {
            for (int i = 0; i < repeatCount; ++i)
            {
                CircleBullet(mover, shape, angle, speed, count, halfAngleOffset);
                yield return new WaitForFrames(interval);
            }
        }

        /// <summary>
        /// 설치 원형탄
        /// </summary>
        public void PlacedCircleBullet(Mover mover, string shape, float angle, float speed, int count, bool halfAngleOffset
            , int moveDuration, int stopDuration, float angle2, float speed2)
        {
            float angleStart = angle + ((halfAngleOffset) ? (1.0f / count / 2.0f) : 0.0f);
            for (int i = 0; i < count; ++i)
            {
                PlacedBullet b = GameSystem._Instance.CreateBullet<PlacedBullet>();
                b.Init(shape, mover._X, mover._Y, angleStart + (1.0f / count * i), speed, moveDuration, stopDuration, angle2, speed2);
            }
        }

        /// <summary>
        /// 선회가속 원형탄
        /// </summary>
        public void BentCircleBullet(Mover mover, string shape, float angle, float speed, int count, float bulletAngleRate, float bulletSpeedRate, bool halfAngleOffset)
        {
            float angleStart = angle + ((halfAngleOffset) ? (1.0f / count / 2.0f) : 0.0f);
            for (int i = 0; i < count; ++i)
            {
                Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init(shape, mover._X, mover._Y, angleStart + (1.0f / count * i), bulletAngleRate, speed, bulletSpeedRate);
            }
        }

        public IEnumerator BentCircleBullets(Mover mover, string shape, float angle, float speed, int count, float bulletAngleRate, float bulletSpeedRate, bool halfAngleOffset
            , int interval, int repeatCount)
        {
            for (int i = 0; i < repeatCount; ++i)
            {
                BentCircleBullet(mover, shape, angle, speed, count, bulletAngleRate, bulletSpeedRate, halfAngleOffset);
                yield return new WaitForFrames(interval);
            }
        }

        // 랜덤 원형탄
        public IEnumerator RandomCircleBullets(Mover mover, string shape, float speed, int count, int interval, int duration)
        {
            for (int frame = 0; frame < duration; ++frame)
            {
                if ((frame % interval) == 0)
                {
                    for (int i = 0; i < count; ++i)
                    {
                        Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                        b.Init(shape, mover._X, mover._Y, GameSystem._Instance.GetRandom01()
                            , 0.0f, speed, 0.0f);
                    }
                }
                yield return null;
            }
        }

        /// <summary>
        /// 시작각도 랜덤인 원형탄
        /// </summary>
        public IEnumerator RandomAngleCircleBullets(Mover mover, string shape, float speed, int count, int interval, int repeatCount)
        {
            for (int i = 0; i < repeatCount; ++i)
            {
                CircleBullet(mover, shape, GameSystem._Instance.GetRandom01(), speed, count, true);
                yield return new WaitForFrames(interval);
            }
        }

        /// <summary>
        /// 소용돌이탄
        /// </summary>
        public IEnumerator SpiralBullets(Mover mover, string shape, float angle, float angleRate, float speed, int interval, int duration)
        {
            for (int frame = 0; frame < duration; ++frame)
            {
                if ((frame % interval) == 0)
                {
                    Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                    b.Init(shape, mover._X, mover._Y, angle, 0.0f, speed, 0.0f);

                    angle += angleRate;
                    angle -= Mathf.Floor(angle);
                }
                yield return null;
            }
        }

        /// <summary>
        /// 다방향 소용돌이탄
        /// </summary>
        public IEnumerator MultipleSpiralBullets(Mover mover, string shape, float angle, float angleRate, float speed, int count, int interval, int duration)
        {
            float shotAngle = angle;
            
            for (int frame = 0; frame < duration; ++frame)
            {
                if ((frame % interval) == 0)
                {
                    // 지정된 발사 수 만큼 발사
                    for (int i = 0; i < count; ++i)
                    {
                        Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                        b.Init(shape, mover._X, mover._Y, shotAngle + ((float)i / count)
                            , 0.0f, speed, 0.0f);
                    }

                    shotAngle += angleRate;
                    shotAngle -= Mathf.Floor(shotAngle);
                }
                yield return null;
            }
        }

        /// <summary>
        /// 양회전 소용돌이탄
        /// </summary>
        public IEnumerator BiDirectionalSpiralBullets(Mover mover, string shape
            , float angle, float angleRate1, float angleRate2
            , float speed, int count, int interval, int duration)
        {
            const int directionCount = 2;   // 회전방향의 수
            float[] shotAngle = new float[directionCount] { angle, angle };
            float[] shotAngleRate = new float[directionCount] { angleRate1, angleRate2 };

            for (int frame = 0; frame < duration; ++frame)
            {
                if ((frame % interval) == 0)
                {
                    // 회전이 다른 2종류의 소용돌이탄 발사
                    for (int j = 0; j < directionCount; ++j)
                    {
                        // 지정된 발사 수 만큼 발사
                        for (int i = 0; i < count; ++i)
                        {
                            Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                            b.Init(shape, mover._X, mover._Y, shotAngle[j] + ((float)i / count)
                                , 0.0f, speed, 0.0f);
                        }

                        shotAngle[j] += shotAngleRate[j];
                        shotAngle[j] -= Mathf.Floor(shotAngle[j]);
                    }
                }
                yield return null;
            }
        }

        /// <summary>
        /// 선회가속 소용돌이탄
        /// </summary>
        public IEnumerator BentSpiralBullets(Mover mover, string shape
            , float angle, float angleRate, float speed, int count, int interval
            , float bulletAngleRate, float bulletSpeedRate, int duration)
        {
            float shotAngle = angle;
            
            for (int frame = 0; frame < duration; ++frame)
            {
                if ((frame % interval) == 0)
                {
                    // 지정된 발사 수 만큼 발사
                    for (int i = 0; i < count; ++i)
                    {
                        Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                        b.Init(shape, mover._X, mover._Y, shotAngle + ((float)i / count)
                            , bulletAngleRate, speed, bulletSpeedRate);
                    }

                    shotAngle += angleRate;
                    shotAngle -= Mathf.Floor(shotAngle);
                }
                yield return null;
            }
        }

        /// <summary>
        /// 직선탄
        /// </summary>
        public IEnumerator LineBullets(Vector2 pos, string shape, float angle, float speed, int interval, int repeatCount)
        {
            for (int i = 0; i < repeatCount; ++i)
            {
                Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init(shape, pos.x, pos.y, angle, 0.0f, speed, 0.0f);

                if (i < repeatCount - 1)
                {
                    yield return new WaitForFrames(interval);
                }
            }
        }

        /// <summary>
        /// 조준 직선탄
        /// </summary>
        public IEnumerator AimingLineBullets(Mover mover, string shape, float speed, int interval, int repeatCount)
        {
            // 발사 시작 시 플레이어와의 각도 계산
            float angle = GetPlayerAngle(mover);

            for (int i = 0; i < repeatCount; ++i)
            {
                Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init(shape, mover._X, mover._Y, angle, 0.0f, speed, 0.0f);

                if (i < repeatCount - 1)
                {
                    yield return new WaitForFrames(interval);
                }
            }
        }

        /// <summary>
        /// 조준 N-Way 직선탄
        /// </summary>
        public IEnumerator AimingNWayLineBullets(Mover mover, string shape
            , float speed, int interval, int shotCount, float angleRange, int wayCount)
        {
            // 발사 시작 시 플레이어와의 각도 계산
            float angle = GetPlayerAngle(mover);

            for (int i = 0; i < shotCount; ++i)
            {
                NWayBullet(mover, shape, angle, angleRange, speed, wayCount);

                if (i < shotCount - 1)
                {
                    yield return new WaitForFrames(interval);
                }
            }
        }

        /// <summary>
        /// 회전 N-Way 탄
        /// </summary>
        public IEnumerator RollingNWayBullets(Mover mover, string shape
            , float angle, float angleRange, float angleRate
            , float speed, int count, int groupCount, int interval
            , int repeatCount)
        {
            for (int repeat = 0; repeat < repeatCount; ++repeat)
            {
                // 그룹 수만큼 n-way 탄 발사
                for (int group = 0; group < groupCount; ++group)
                {
                    // 360도를 n-way 수로 등분하여 n-way탄 발사 방향 결정
                    float nwayAngle = angle + (float)group / groupCount;
                    NWayBullet(mover, shape, nwayAngle, angleRange, speed, count);
                }

                // 발사 각속도 변화
                angle += angleRate;
                angle -= Mathf.Floor(angle);

                if (repeat < repeatCount - 1)
                {
                    yield return new WaitForFrames(interval);
                }
            }
        }

        /// <summary>
        /// 랜덤 확산탄
        /// </summary>
        public void RandomSpreadBullet(Mover mover, string shape
            , float angleRange, float speed, float speedRange, int count)
        {
            // 한 번에 뿌리지만 속도가 달라 여러번 나눠서 뿌리는 것 같은 효과
            float angle = GetPlayerAngle(mover);
            for (int i = 0; i < count; ++i)
            {
                // 탄 별로 각도와 속도를 랜덤으로 설정
                float bulletAngle = angle + angleRange * (GameSystem._Instance.GetRandom01() - 0.5f);
                float bulletSpeed = speed + speedRange * GameSystem._Instance.GetRandom01();
                Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init(shape, mover._X, mover._Y, bulletAngle, 0.0f, bulletSpeed, 0.0f);
            }
        }

        /// <summary>
        /// 회전하며 조준탄 뿌리기
        /// </summary>
        public IEnumerator RollingAimingBullets(Mover mover, string shape
            , float speed, int count, float radius, int interval, int repeatCount)
        {
            float angle = GetPlayerAngle(mover) + 0.25f;    // 시작 각도는 플레이어 방향과 직각
            for (int repeat = 0; repeat < repeatCount; ++repeat)
            {
                for (int i = 0; i < count; ++i)
                {
                    // 발사 위치
                    float spawnAngleRadian = (2.0f * Mathf.PI) * (angle - (1.0f / count * i));  // 반시계방향
                    float spawnX = mover._X + radius * Mathf.Cos(spawnAngleRadian);
                    float spawnY = mover._Y + radius * Mathf.Sin(spawnAngleRadian);
                    // 발사위치로부터 플레이어 방향
                    float bulletAngle = GetPlayerAngle(spawnX, spawnY);

                    Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                    b.Init(shape, spawnX, spawnY, bulletAngle, 0.0f, speed, 0.0f);

                    yield return new WaitForFrames(interval);
                }
            }
        }

        /// <summary>
        /// 배열 패턴 탄
        /// </summary>
        public IEnumerator PatternBullets(Mover mover, string shape
            , float angle, float speed, int interval, byte[] pattern, int col, int row, float colSpace)
        {
            // 한줄씩 발사
            float rad = angle * Mathf.PI * 2.0f;
            float sin = Mathf.Sin(rad);
            float cos = Mathf.Cos(rad);
            float centerCol = ((float)col - 1.0f) / 2.0f;   // 가운데 칸의 인덱스 (예: 2칸 -> [0] "[0.5]" [1])
            
            // 아랫줄 부터 출력
            for (int r = row - 1; r >= 0; --r)
            {
                // 이번 줄 첫번째 인덱스
                int startIndex = col * r;

                // 왼쪽 탄을 위에 보이도록 하기 위해 오른쪽부터 읽음
                for (int i = col - 1; i >= 0; --i)
                {
                    // 0 외의 부분에 탄 발사
                    int index = startIndex + i;
                    if (pattern[index] != 0)
                    {
                        // 화면 아래로 향할 때(0.75, 270도) 패턴 그대로 출력
                        // 즉, 0도일 떄는 colOffset이 Y축으로 작동
                        float xOffset = 0.0f;
                        float yOffset = ((float)i - centerCol) * colSpace; // 가운데 칸으로부터 얼마나 떨어졌는가?
                        float x = mover._X + (cos * xOffset + -1.0f * sin * yOffset); // 벡터 회전공식 참고. http://en.wikipedia.org/wiki/Rotation_(mathematics)
                        float y = mover._Y + (sin * xOffset + cos * yOffset);
                        Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                        b.Init(shape, x, y, angle, 0.0f, speed, 0.0f);
                    }
                }

                if (r > 0)
                {
                    yield return new WaitForFrames(interval);
                }
            }
        }

        public void AimimgBullet(Mover mover, string shape, float speed)
        {
            Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
            b.Init(shape, mover._X, mover._Y, GetPlayerAngle(mover), 0.0f, speed, 0.0f);
        }
        #endregion Pattern
    }
}