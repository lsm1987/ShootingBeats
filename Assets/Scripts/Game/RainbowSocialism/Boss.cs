using UnityEngine;
using System.Collections;

namespace Game
{
    namespace RainbowSocialism
    {
        // 보스
        public class Boss : Enemy
        {
            private int _score = 10; // 피격시 획득 점수
            CoroutineManager _coroutineManager = new CoroutineManager();

            public Boss()
                : base()
            {
            }

            public override void Move()
            {
                MoveMain();
                _coroutineManager.UpdateAllCoroutines();
            }

            // 실제 갱신
            private void MoveMain()
            {
                int frame = _Frame;
                if (frame == 0)
                {
                    _coroutineManager.StartCoroutine(MoveConstantVelocity(new Vector2(0.0f, 0.75f), 40));
                }
                else if (frame == 50)
                {
                    _coroutineManager.StartCoroutine(Simple3Wave(true));
                }
                else if (frame == 148)
                {
                    _coroutineManager.StartCoroutine(Simple3Wave(false));
                }
                else if (frame == 249)
                {
                    _coroutineManager.StartCoroutine(Simple4Wave());
                    _coroutineManager.StartCoroutine(MoveConstantVelocity(new Vector2(0.0f, 0.5f), 75));
                }
                else if (frame == 356)
                {
                    _coroutineManager.StartCoroutine(SimpleCircles());
                }
                else if (frame == 469)
                {
                    _coroutineManager.StartCoroutine(AimAfterSimpleCircles());
                }
                else if (frame == 799)
                {
                    _coroutineManager.StartCoroutine(MoveDamp(new Vector2(-0.6f, 0.75f), 30, 0.1f));
                }
                else if (frame == 890)
                {
                    _coroutineManager.StartCoroutine(CornerWaves(true));
                }
                else if (frame == 1230)
                {
                    _coroutineManager.StartCoroutine(MoveDamp(new Vector2(0.6f, 0.75f), 60, 0.1f));
                }
                else if (frame == 1320)
                {
                    _coroutineManager.StartCoroutine(CornerWaves(false));
                }
                else if (frame == 1660)
                {
                    _coroutineManager.StartCoroutine(MoveDamp(new Vector2(0.0f, 0.0f), 40, 0.1f));
                }
                else if (frame == 1750)
                {
                    _coroutineManager.StartCoroutine(RotateCrossTwice());
                }
            }

            // 피격시
            public override void OnHit()
            {
                GameSystem._Instance.SetScoreDelta(_score);
            }

            #region Coroutine
            // 지정한 위치까지 등속 이동
            private IEnumerator MoveConstantVelocity(Vector3 arrivePos, int duration)
            {
                int startFrame = _Frame;
                Vector2 startPos = _Pos;

                while (_Frame < (startFrame + duration))
                {
                    float time = (float)(_Frame - startFrame) / (float)duration;
                    _Pos = Vector2.Lerp(startPos, arrivePos, time);
                    yield return null;
                }

                // 마지막 프레임
                _Pos = arrivePos;
            }

            // 지정한 위치까지 비례감속 이동
            private IEnumerator MoveDamp(Vector3 arrivePos, int duration, float damp)
            {
                int startFrame = _Frame;

                while (_Frame < (startFrame + duration))
                {
                    _Pos = Vector2.Lerp(_Pos, arrivePos, damp);
                    yield return null;
                }

                // 마지막 프레임
                _Pos = arrivePos;
            }

            // 단순 3파
            private IEnumerator Simple3Wave(bool leftToRight)
            {
                // 1파
                Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init("Common/Bullet_Blue", _x, _y, 0.75f
                    , 0.0f, 0.02f, 0.0f);
                yield return new WaitForFrames(11);

                // 2파
                b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init("Common/Bullet_Blue", _x - 0.2f, _y, 0.75f
                    , 0.0f, 0.02f, 0.0f);
                b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init("Common/Bullet_Blue", _x + 0.2f, _y, 0.75f
                    , 0.0f, 0.02f, 0.0f);
                yield return new WaitForFrames(12);

                // 3파
                const float angle1 = 0.625f;
                const float angle2 = 0.875f;
                float startAngle = (leftToRight) ? angle1 : angle2;
                float endAngle = (leftToRight) ? angle2 : angle1;
                const int count = 7;
                for (int i = 0; i < count; ++i)
                {
                    b = GameSystem._Instance.CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Blue", _x, _y, (startAngle + (endAngle - startAngle) / count * i)
                        , 0.0f, 0.02f, 0.0f);

                    if (i < count - 1)
                    {
                        yield return new WaitForFrames(7);
                    }
                }
            }

            // 단순 4파
            private IEnumerator Simple4Wave()
            {
                Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init("Common/Bullet_Red", -0.2f, 1.3f, 0.75f
                    , 0.0f, 0.03f, 0.0f);
                b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init("Common/Bullet_Red", 0.2f, 1.3f, 0.75f
                    , 0.0f, 0.03f, 0.0f);
                yield return new WaitForFrames(25);

                b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init("Common/Bullet_Red", -0.4f, 1.3f, 0.75f
                    , 0.0f, 0.03f, 0.0f);
                b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init("Common/Bullet_Red", 0.4f, 1.3f, 0.75f
                    , 0.0f, 0.03f, 0.0f);
                yield return new WaitForFrames(25);

                b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init("Common/Bullet_Red", -0.6f, 1.3f, 0.75f
                    , 0.0f, 0.03f, 0.0f);
                b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init("Common/Bullet_Red", 0.6f, 1.3f, 0.75f
                    , 0.0f, 0.03f, 0.0f);
                yield return new WaitForFrames(25);

                b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init("Common/Bullet_Red", -0.8f, 1.3f, 0.75f
                    , 0.0f, 0.03f, 0.0f);
                b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init("Common/Bullet_Red", 0.8f, 1.3f, 0.75f
                    , 0.0f, 0.03f, 0.0f);
            }

            private void ShootCircleBullet(string shape, int count, float speed, bool halfAngleOffset)
            {
                float angleOffset = (halfAngleOffset) ? (1.0f / count / 2.0f) : 0.0f;
                for (int i = 0; i < count; ++i)
                {
                    Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                    b.Init(shape, _x, _y, (1.0f / count * i) + angleOffset
                        , 0.0f, speed, 0.0f);
                }
            }

            // 단순 원형 연속
            private IEnumerator SimpleCircles()
            {
                const int count = 9;
                for (int i = 0; i < count; ++i)
                {
                    bool halfAngleOffset = (i % 2) != 0;
                    ShootCircleBullet("Common/Bullet_Blue", 20, 0.005f, halfAngleOffset);

                    if (i < count - 1)
                    {
                        yield return new WaitForFrames(13);
                    }
                }
            }

            private IEnumerator AimAfterSimpleCircles()
            {
                const int count = 6;
                for (int i = 0; i < count; ++i)
                {
                    float playerAngle = GetPlayerAngle();
                    Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Red", _x, _y, playerAngle
                        , 0.0f, 0.02f, 0.0f);

                    if (i < count - 1)
                    {
                        yield return new WaitForFrames(55);
                    }
                }
            }

            // 각 모서리에서 진행할 웨이브들
            private IEnumerator CornerWaves(bool leftCorner)
            {
                // 2파 1
                yield return _coroutineManager.StartCoroutine(CornerWaves_2Wave(leftCorner));
                // 3파 2
                yield return _coroutineManager.StartCoroutine(CornerWaves_2Wave(leftCorner));
                // 4파
                {
                    float x = (leftCorner) ? 1.0f : -1.0f;
                    float angle = (leftCorner) ? 0.5f : 0.0f;
                    for (int i = 0; i < 4; ++i)
                    {
                        Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                        b.Init("Common/Bullet_Red", x, 0.5f - 1.5f / 3.0f * i, angle
                            , 0.0f, 0.01f, 0.0f);
                        yield return new WaitForFrames(25);
                    }
                }
                // 연타
                ShootCircleBullet("Common/Bullet_Blue", 12, 0.02f, false);
                yield return new WaitForFrames(26);
                ShootCircleBullet("Common/Bullet_Blue", 12, 0.02f, true);
            }

            private IEnumerator CornerWaves_2Wave(bool leftCorner)
            {
                // 1탄
                Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init("Common/Bullet_Blue", _x, _y, 0.75f
                    , 0.0f, 0.02f, 0.0f);
                yield return new WaitForFrames(32);

                float angle = ((leftCorner) ? 0.875f : 0.625f);
                ShootNWay("Common/Bullet_Blue", 0.02f, 6, angle, 0.25f);
                yield return new WaitForFrames(75);
            }

            private void ShootNWay(string shape, float speed, int count, float angle, float angleRange)
            {
                for (int i = 0; i < count; ++i)
                {
                    Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Blue", _x, _y, angle + angleRange * ((float)i / (count - 1) - 0.5f)
                        , 0.0f, 0.02f, 0.0f);
                }
            }

            private IEnumerator ShootIntervalMultipleSpiral(float angle, float angleRate, float speed, int count, int interval, int duration)
            {
                int frame = 0;
                int startFrame = _Frame;
                float shotAngle = angle;

                while (_Frame < startFrame + duration)
                {
                    if (frame == 0)
                    {
                        // 지정된 발사 수 만큼 발사
                        for (int i = 0; i < count; ++i)
                        {
                            Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                            b.Init("Common/Bullet_Blue", _x, _y, shotAngle + ((float)i / count)
                                , 0.0f, speed, 0.0f);
                        }

                        shotAngle += angleRate;
                        shotAngle -= Mathf.Floor(shotAngle);
                    }

                    // 타이머 갱신
                    frame = (frame + 1) % interval;
                    yield return null;
                }
            }

            private IEnumerator RotateCrossTwice()
            {
                _coroutineManager.StartCoroutine(RotateCrossTwice_DirectionShot(false));
                yield return _coroutineManager.StartCoroutine(ShootIntervalMultipleSpiral(0.125f, 0.005f, 0.05f, 4, 2, 410));
                _coroutineManager.StartCoroutine(RotateCrossTwice_DirectionShot(true));
                yield return _coroutineManager.StartCoroutine(ShootIntervalMultipleSpiral(0.125f, -0.005f, 0.05f, 4, 2, 410));
            }

            private IEnumerator RotateCrossTwice_DirectionShot(bool clockwise)
            {
                const int count = 4;
                float startAngle = 0.75f + (0.125f * (clockwise ? -1.0f : 1.0f));
                for (int i = 0; i < count; ++i)
                {
                    Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Red", _x, _y, startAngle + 0.25f * (float)i * (clockwise ? -1.0f : 1.0f)
                        , 0.0f, 0.01f, 0.0f);

                    if (i < count - 1)
                    {
                        yield return new WaitForFrames(105);
                    }
                }
            }
            #endregion //Coroutine
        } // Boss
    }
}