using UnityEngine;
using System.Collections;

namespace Game
{
    namespace Calc
    {
        // 보스
        public class Boss : Enemy
        {
            private const int _score = 10; // 피격시 획득 점수
            private CoroutineManager _coroutineManager = new CoroutineManager();

            public Boss()
                : base()
            {
            }

            public override void Init(string shapeSubPath, float x, float y, float angle)
            {
                base.Init(shapeSubPath, x, y, angle);
                _coroutineManager.RegisterCoroutine(MoveMain());
            }

            public override void Move()
            {
                _coroutineManager.UpdateAllCoroutines();
            }

            // 메인 코루틴
            private IEnumerator MoveMain()
            {
                // 등장
                _coroutineManager.StartCoroutine(_Logic.MoveConstantVelocity(this, new Vector2(0.0f, 0.75f), 120));

                yield return new WaitForAbsFrames(11 * 60 + 30);
                _coroutineManager.StartCoroutine(Pattern_CircleWave_4x7());

                yield return new WaitForAbsFrames((int)(22.9f * 60));
                _coroutineManager.StartCoroutine(Pattern_SideCircle(8));

                yield return new WaitForAbsFrames((int)(33.0f * 60));
                _coroutineManager.StartCoroutine(Pattern_SideMoveDamp());
                _coroutineManager.StartCoroutine(Pattern_SideCircle(7));

                yield return new WaitForAbsFrames((int)(43.9f * 60));
                _coroutineManager.StartCoroutine(Pattern_Circle3());

                yield return new WaitForAbsFrames((int)(45f * 60));
                Pattern_Pendulumn();

                yield return new WaitForAbsFrames((int)(1f * 3600 + 7f * 60 + 0.3f * 60));
                _coroutineManager.StartCoroutine(_Logic.MoveConstantVelocity(this, new Vector2(0.0f, 0.25f), 5 * 60));

                // 폭발
                yield return new WaitForAbsFrames(8700);
                {
                    Effect crashEffect = GameSystem._Instance.CreateEffect<Effect>();
                    crashEffect.Init("Common/Effect_BossCrashMiku", _X, _Y, 0.0f);
                }
                _alive = false;
            }

            // 피격시
            public override void OnHit()
            {
                GameSystem._Instance.SetScoreDelta(_score);
            }

            public override void OnDestroy()
            {
                base.OnDestroy();
                _coroutineManager.StopAllCoroutines();
            }

            #region Coroutine
            private IEnumerator Pattern_CircleWave_4x7()
            {
                const float startAngle = 0.75f;
                const float speed = 0.01f;
                const int bulletPerCircle = 20;
                const int circlePerWave = 4;
                const int waveCount = 7;

                const int circleInterval = 20;
                const int waveInterval = 5;

                for (int wave = 0; wave < waveCount; ++wave)
                {
                    bool bBlue = wave % 2 == 0;
                    string shape = bBlue ? "Common/Bullet_Blue" : "Common/Bullet_Red";
                    bool bHalfAngleOffset = !bBlue;
                    bool bLastWave = (wave == waveCount - 1);

                    for (int circle = 0; circle < circlePerWave; ++circle)
                    {
                        _Logic.CircleBullet(this, shape, startAngle, speed, bulletPerCircle, bHalfAngleOffset);

                        bool bLastCircle = bLastWave && (circle == circlePerWave - 1);

                        if (!bLastCircle)
                        {
                            yield return new WaitForFrames(circleInterval);
                        }
                    }

                    if (!bLastWave)
                    {
                        yield return new WaitForFrames(waveInterval);
                    }
                }
            }

            private IEnumerator Pattern_SideCircle(int waveCount)
            {
                const float startAngle = 0.75f;
                const float speed = 0.01f;
                const float circleOffsetX = 0.3f;
                const int bulletPerCircle = 10;
                const int circlePerWave = 4;
                const int circleInterval = 20;

                for (int wave = 0; wave < waveCount; ++wave)
                {
                    string shape = "Common/Bullet_Blue";
                    bool bHalfAngleOffset = true;

                    for (int circle = 0; circle < circlePerWave; ++circle)
                    {
                        bool bLeft = circle % 2 == 0;
                        float x = _X + (bLeft ? -1.0f : 1.0f) * circleOffsetX;
                        float y = _Y;

                        _Logic.CircleBullet(x, y, shape, startAngle, speed, bulletPerCircle, bHalfAngleOffset);
                        yield return new WaitForFrames(circleInterval);
                    }
                }
            }

            private IEnumerator Pattern_SideMoveDamp()
            {
                const float MoveOffsetX = 0.6f;
                const float MoveOffsetY = 0.5f;
                const int MoveInterval = 190;

                _coroutineManager.StartCoroutine(_Logic.MoveDamp(this, new Vector2(MoveOffsetX * -1.0f, MoveOffsetY), 30, 0.1f));
                yield return new WaitForFrames(MoveInterval);
                _coroutineManager.StartCoroutine(_Logic.MoveDamp(this, new Vector2(MoveOffsetX, MoveOffsetY), 30 * 2, 0.1f));
                yield return new WaitForFrames(MoveInterval);
                _coroutineManager.StartCoroutine(_Logic.MoveDamp(this, new Vector2(0.0f, MoveOffsetY), 30, 0.1f));
                yield return new WaitForFrames(160);
                _coroutineManager.StartCoroutine(_Logic.MoveDamp(this, new Vector2(0.0f, 0.75f), 30, 0.1f));
            }

            private IEnumerator Pattern_Circle3()
            {
                const float startAngle = 0.75f;
                const float speed = 0.02f;
                const int bulletPerCircle = 20;
                const int circleInterval = 20;
                const string shape = "Common/Bullet_RedLarge";

                _Logic.CircleBullet(this, shape, startAngle, speed, bulletPerCircle, false);
                yield return new WaitForFrames(circleInterval);
                _Logic.CircleBullet(this, shape, startAngle, speed, bulletPerCircle, true);
                yield return new WaitForFrames(circleInterval);
                _Logic.CircleBullet(this, shape, startAngle, speed, bulletPerCircle, false);
            }

            private void Pattern_Pendulumn()
            {
                int rotationCount = 4;
                int rotationDuration = 320; // 1회 회전에 걸리는 프레임
                const float rotationRadius = 0.6f;
                const float moverStartRad = (Mathf.PI / 2.0f); // 90도 위치부터 시작

                _coroutineManager.StartCoroutine(Pattern_Pendulum_Move(rotationCount, rotationDuration, rotationRadius, moverStartRad));
                _coroutineManager.StartCoroutine(Pattern_Pendulum_Line(rotationCount, rotationDuration, rotationRadius, moverStartRad));
                _coroutineManager.StartCoroutine(Pattern_Pendulum_NWay());
            }

            private IEnumerator Pattern_Pendulum_Move(int rotationCount, int rotationDuration, float rotationRadius, float startRad)
            {
                float pivotX = _X;
                float radSpeed = (Mathf.PI * 2.0f / rotationDuration);

                for (int rotation = 0; rotation < rotationCount; ++rotation)
                {
                    for (int i = 0; i < rotationDuration; ++i)
                    {
                        float rad = startRad + radSpeed * i;
                        _X = Mathf.Cos(rad) * rotationRadius;
                        yield return null;
                    }
                }

                _X = pivotX;
            }

            private IEnumerator Pattern_Pendulum_Line(int rotationCount, int rotationDuration, float rotationRadius, float moverStartRad)
            {
                float[] offsetXs = new float[2] { -0.4f, 0.4f };
                const int interval = 4;
                const string shape = "Common/Bullet_Blue";
                const float fireStartY = 1.30f;
                const float angle = 0.75f;
                const float speed = 0.04f;

                // 발사 위치가 무버보다 후방에 있으므로 발사 위치는 무버보다 살짝 먼저 움직인다.
                float moverStartY = _Y;
                float radSpeed = (Mathf.PI * 2.0f / rotationDuration);
                int fireToMoverFrameGap = (int)((fireStartY - moverStartY) / speed);
                float fireStartRad = moverStartRad + radSpeed * fireToMoverFrameGap;

                for (int rotation = 0; rotation < rotationCount; ++rotation)
                {
                    for (int i = 0; i < rotationDuration; ++i)
                    {
                        float rad = fireStartRad + radSpeed * i;
                        float fireX = Mathf.Cos(rad) * rotationRadius;
                        int frame = rotation * rotationDuration + i;

                        if ((frame % interval) == 0)
                        {
                            foreach (float offsetX in offsetXs)
                            {
                                Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                                b.Init(shape, fireX + offsetX, fireStartY, angle, 0.0f, speed, 0.0f);
                            }
                        }

                        yield return null;
                    }
                }
            }

            private IEnumerator Pattern_Pendulum_NWay()
            {
                const int waitDuration = 620;
                yield return new WaitForFrames(waitDuration);

                const int waveCount = 8;
                const int waveInterval = 85;
                const float angleRange = 0.0625f;

                for (int wave = 0; wave < waveCount; ++wave)
                {
                    _Logic.NWayBullet(this, "Common/Bullet_Red", 0.75f, angleRange, 0.01f, 5);
                    yield return new WaitForFrames(waveInterval);
                }
            }

            /*
            private IEnumerator Pattern_PlacedCircleWave()
            {
                const int waveCount = 16;
                const int bulletPerCircle = 10;
                const float speed1 = 0.04f;
                const float angle2 = 0.75f;
                const float speed2 = 0.02f;
                const int moveDuaraion = 12;
                const int stopDuaraion = 12;
                const int waveInterval = 20;

                for (int wave = 0; wave < waveCount; ++wave)
                {
                    bool bLeft = (wave % 2 == 0);
                    float angle1 = bLeft ? 0.5f : 0.0f;

                    _Logic.PlacedCircleBullet(this, "Common/Bullet_Blue", angle1, speed1, bulletPerCircle, false, moveDuaraion, stopDuaraion, angle2, speed2);
                    yield return new WaitForFrames(waveInterval);
                }
            }
            */
            #endregion //Coroutine

            private GameLogic _Logic
            {
                get { return GameSystem._Instance.GetLogic<GameLogic>(); }
            }
        } // Boss
    } // Calc
} // Game