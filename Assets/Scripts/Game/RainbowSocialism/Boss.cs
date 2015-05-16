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
                _coroutineManager.StartCoroutine(_Logic.MoveConstantVelocity(this, new Vector2(0.0f, 0.75f), 40));

                yield return new WaitForAbsFrames(50);
                _coroutineManager.StartCoroutine(Simple3Wave(true));

                yield return new WaitForAbsFrames(140);
                _coroutineManager.StartCoroutine(Simple3Wave(false));

                yield return new WaitForAbsFrames(249);
                _coroutineManager.StartCoroutine(Simple4Wave());
                _coroutineManager.StartCoroutine(_Logic.MoveConstantVelocity(this, new Vector2(0.0f, 0.5f), 75));

                yield return new WaitForAbsFrames(356);
                _coroutineManager.StartCoroutine(SimpleCircles());

                yield return new WaitForAbsFrames(469);
                _coroutineManager.StartCoroutine(AimAfterSimpleCircles());

                yield return new WaitForAbsFrames(799);
                // 왼쪽 코너로 이동
                _coroutineManager.StartCoroutine(_Logic.MoveDamp(this, new Vector2(-0.6f, 0.75f), 30, 0.1f));

                yield return new WaitForAbsFrames(890);
                _coroutineManager.StartCoroutine(CornerWaves(true));

                yield return new WaitForAbsFrames(1230);
                // 오른쪽 코너로 이동
                _coroutineManager.StartCoroutine(_Logic.MoveDamp(this, new Vector2(0.6f, 0.75f), 60, 0.1f));

                yield return new WaitForAbsFrames(1320);
                _coroutineManager.StartCoroutine(CornerWaves(false));

                yield return new WaitForAbsFrames(1660);
                // 중앙으로 이동
                _coroutineManager.StartCoroutine(_Logic.MoveDamp(this, new Vector2(0.0f, 0.0f), 40, 0.1f));

                yield return new WaitForAbsFrames(1750);
                _coroutineManager.StartCoroutine(RotateCrossTwice1());

                yield return new WaitForAbsFrames(2580);
                _coroutineManager.StartCoroutine(BackwardStep());

                yield return new WaitForAbsFrames(3026);
                _coroutineManager.StartCoroutine(PigeonSolo());

                yield return new WaitForAbsFrames(4700);
                // 왼쪽 코너로 이동
                _coroutineManager.StartCoroutine(_Logic.MoveDamp(this, new Vector2(-0.6f, 0.75f), 30, 0.1f));

                yield return new WaitForAbsFrames(4730);
                _coroutineManager.StartCoroutine(CornerWaves(true));

                yield return new WaitForAbsFrames(5070);
                // 오른쪽 코너로 이동
                _coroutineManager.StartCoroutine(_Logic.MoveDamp(this, new Vector2(0.6f, 0.75f), 60, 0.1f));

                yield return new WaitForAbsFrames(5160);
                _coroutineManager.StartCoroutine(CornerWaves(false));

                yield return new WaitForAbsFrames(5500);
                // 중앙으로 이동
                _coroutineManager.StartCoroutine(_Logic.MoveDamp(this, new Vector2(0.0f, 0.0f), 40, 0.1f));

                yield return new WaitForAbsFrames(5590);
                _coroutineManager.StartCoroutine(RotateCrossTwice2());

                yield return new WaitForAbsFrames(6410);
                {
                    Effect crashEffect = GameSystem._Instance.CreateEffect<Effect>();
                    crashEffect.Init("Common/Effect_BossCrashOrange", _X, _Y, 0.0f);
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
            // 단순 3파
            private IEnumerator Simple3Wave(bool leftToRight)
            {
                // 1파
                Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init("Common/Bullet_Blue", _X, _Y, 0.75f
                    , 0.0f, 0.02f, 0.0f);
                yield return new WaitForFrames(11);

                // 2파
                b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init("Common/Bullet_Blue", _X - 0.2f, _Y, 0.75f
                    , 0.0f, 0.02f, 0.0f);
                b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init("Common/Bullet_Blue", _X + 0.2f, _Y, 0.75f
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
                    b.Init("Common/Bullet_Blue", _X, _Y, (startAngle + (endAngle - startAngle) / count * i)
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

            // 단순 원형 연속
            private IEnumerator SimpleCircles()
            {
                const int count = 9;
                for (int i = 0; i < count; ++i)
                {
                    bool halfAngleOffset = (i % 2) != 0;
                    _Logic.CircleBullet(this, "Common/Bullet_Blue", 0.0f, 0.005f, 20, halfAngleOffset);

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
                    float playerAngle = _Logic.GetPlayerAngle(this);
                    Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Red", _X, _Y, playerAngle
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
                _Logic.CircleBullet(this, "Common/Bullet_Blue", 0.0f, 0.02f, 12, false);
                yield return new WaitForFrames(26);
                _Logic.CircleBullet(this, "Common/Bullet_Blue", 0.0f, 0.02f, 12, true);
            }

            private IEnumerator CornerWaves_2Wave(bool leftCorner)
            {
                // 1탄
                Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init("Common/Bullet_Blue", _X, _Y, 0.75f
                    , 0.0f, 0.02f, 0.0f);
                yield return new WaitForFrames(32);

                float angle = ((leftCorner) ? 0.875f : 0.625f);
                _Logic.NWayBullet(this, "Common/Bullet_Blue", angle, 0.25f, 0.02f, 6);
                yield return new WaitForFrames(75);
            }

            private IEnumerator RotateCrossTwice1()
            {
                const int repeatCount = 4;
                const int interval = 105;
                _coroutineManager.StartCoroutine(RotateCrossTwice_DirectionShot(false, repeatCount, interval));
                yield return _coroutineManager.StartCoroutine(_Logic.MultipleSpiralBullets(this, "Common/Bullet_Blue", 0.125f, 0.005f, 0.05f, 4, 2, 410));
                _coroutineManager.StartCoroutine(RotateCrossTwice_DirectionShot(true, repeatCount, interval));
                yield return _coroutineManager.StartCoroutine(_Logic.MultipleSpiralBullets(this, "Common/Bullet_Blue", 0.125f, -0.005f, 0.05f, 4, 2, 410));
            }

            private IEnumerator RotateCrossTwice2()
            {
                const int repeatCount = 4 * 2;
                const int interval = 105 / 2;
                _coroutineManager.StartCoroutine(RotateCrossTwice_DirectionShot(false, repeatCount, interval));
                yield return _coroutineManager.StartCoroutine(_Logic.MultipleSpiralBullets(this, "Common/Bullet_Blue", 0.125f, 0.005f, 0.05f, 4, 2, 410));
                _coroutineManager.StartCoroutine(RotateCrossTwice_DirectionShot(true, repeatCount, interval));
                yield return _coroutineManager.StartCoroutine(_Logic.MultipleSpiralBullets(this, "Common/Bullet_Blue", 0.125f, -0.005f, 0.05f, 4, 2, 410));
            }

            private IEnumerator RotateCrossTwice_DirectionShot(bool clockwise, int repeatCount, int interval)
            {
                const int directionCount = 4;
                const float startAngle = 0.75f;
                const float angleRange = 1.0f - 1.0f / directionCount;
                for (int i = 0; i < repeatCount; ++i)
                {
                    float angle = startAngle + (1.0f / (float)repeatCount) * (float)i * (clockwise ? -1.0f : 1.0f);
                    _Logic.NWayBullet(this, "Common/Bullet_Red", angle, angleRange, 0.01f, directionCount);

                    if (i < repeatCount - 1)
                    {
                        yield return new WaitForFrames(interval);
                    }
                }
            }

            private IEnumerator BackwardStep()
            {
                // 뒷걸음질 치기는 병렬로 수행
                _coroutineManager.StartCoroutine(_Logic.MoveConstantVelocity(this, new Vector2(0.0f, 0.75f), 360));

                // 첫 탄 발사 전 딜레이
                const int interval = 45;
                yield return new WaitForFrames(interval);

                const float speed = 0.01f;
                const float angle = 0.75f;
                const float nwayAngleRange = 0.125f;
                Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init("Common/Bullet_Red", _X, _Y, angle, 0.0f, speed, 0.0f);
                yield return new WaitForFrames(interval);

                _Logic.NWayBullet(this, "Common/Bullet_Red", angle, nwayAngleRange, speed, 2);
                yield return new WaitForFrames(interval);

                b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init("Common/Bullet_Red", _X, _Y, angle, 0.0f, speed, 0.0f);
                yield return new WaitForFrames(interval);

                _Logic.NWayBullet(this, "Common/Bullet_Red", angle, nwayAngleRange, speed, 2);
                yield return new WaitForFrames(interval);

                b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init("Common/Bullet_Red", _X, _Y, angle, 0.0f, speed, 0.0f);
                yield return new WaitForFrames(interval);

                _Logic.NWayBullet(this, "Common/Bullet_Red", angle, nwayAngleRange, speed, 2);
                yield return new WaitForFrames(interval);
            }

            // 비둘기 솔로
            private IEnumerator PigeonSolo()
            {
                yield return _coroutineManager.StartCoroutine(_Logic.MultipleSpiralBullets(this, "Common/Bullet_Blue", 0.0f, 0.02f, 0.01f, 4, 5, 193));
                yield return _coroutineManager.StartCoroutine(_Logic.MultipleSpiralBullets(this, "Common/Bullet_Blue", 0.0f, -0.02f, 0.01f, 4, 5, 193));
                yield return _coroutineManager.StartCoroutine(_Logic.BiDirectionalSpiralBullets(this, "Common/Bullet_Blue", 0.0f, 0.03f, -0.02f, 0.01f, 4, 5, 400));
                yield return new WaitForFrames(90);
                yield return _coroutineManager.StartCoroutine(_Logic.BentSpiralBullets(this, "Common/Bullet_Red", 0.0f, 0.02f, 0.0f, 10, 10, -0.003f, 0.0002f, 400));
                yield return new WaitForFrames(20);
                // 중앙으로 이동
                yield return _coroutineManager.StartCoroutine(_Logic.MoveConstantVelocity(this, new Vector2(0.0f, 0.0f), 180));
                yield return new WaitForFrames(30);
                // 랜덤 뿌리기
                yield return _coroutineManager.StartCoroutine(_Logic.RandomCircleBullets(this, "Common/Bullet_Blue", 0.01f, 3, 3, 150));
            }
            #endregion //Coroutine

            private GameLogic _Logic
            {
                get { return GameSystem._Instance.GetLogic<GameLogic>(); }
            }
        } // Boss
    } // RainbowSocialism
} // Game