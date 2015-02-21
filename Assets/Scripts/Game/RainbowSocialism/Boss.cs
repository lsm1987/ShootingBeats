﻿using UnityEngine;
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

            private void SpawnCircleBullet(string shape, int count, float speed, bool halfAngleOffset)
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
                    SpawnCircleBullet("Common/Bullet_Blue", 20, 0.005f, halfAngleOffset);

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

            #endregion //Coroutine
        } // Boss
    }
}