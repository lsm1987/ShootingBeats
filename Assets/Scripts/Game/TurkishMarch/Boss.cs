using UnityEngine;
using System.Collections;

namespace Game
{
    namespace TurkishMarch
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
                
                yield return new WaitForAbsFrames(90);
                _coroutineManager.StartCoroutine(PatternA_11());
                yield return new WaitForAbsFrames(511);
                _coroutineManager.StartCoroutine(PatternA_11());
                yield return new WaitForAbsFrames(935);
                _coroutineManager.StartCoroutine(PatternB());
                yield return new WaitForAbsFrames(1370);
                _coroutineManager.StartCoroutine(PatternA_12());
                yield return new WaitForAbsFrames(1800);
                _coroutineManager.StartCoroutine(PatternB());
                yield return new WaitForAbsFrames(2235);
                _coroutineManager.StartCoroutine(PatternA_12());
                yield return new WaitForAbsFrames(2670);
                _coroutineManager.StartCoroutine(PatternC());
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
            private IEnumerator PatternA_11()
            {
                yield return _coroutineManager.StartCoroutine(PatternA_a1());
                yield return new WaitForFrames(100);
                yield return _coroutineManager.StartCoroutine(PatternA_b1());
            }

            private IEnumerator PatternA_12()
            {
                yield return _coroutineManager.StartCoroutine(PatternA_a1());
                yield return new WaitForFrames(110);
                yield return _coroutineManager.StartCoroutine(PatternA_b2());
            }

            private IEnumerator PatternA_a1()
            {
                const int interval = 5;
                _coroutineManager.StartCoroutine(_Logic.AimingLineBullets(this, "Common/Bullet_Red", 0.02f, interval, 5));
                yield return new WaitForFrames(50);
                _coroutineManager.StartCoroutine(_Logic.AimingLineBullets(this, "Common/Bullet_Red", 0.02f, interval, 5));
                yield return new WaitForFrames(50);
                _coroutineManager.StartCoroutine(_Logic.AimingNWayLineBullets(this, "Common/Bullet_Red", 0.02f, interval, 4, 0.125f, 3));
                yield return new WaitForFrames(25);
                _coroutineManager.StartCoroutine(_Logic.AimingNWayLineBullets(this, "Common/Bullet_Red", 0.02f, interval, 9, 0.125f, 3));
            }

            private IEnumerator PatternA_b1()
            {
                const int circleInterval = 28;
                _coroutineManager.StartCoroutine(_Logic.CircleBullets(this, "Common/Bullet_Blue", 0.25f, 0.02f, 12, true, circleInterval, 7));
                yield return new WaitForFrames(circleInterval / 2);
                _coroutineManager.StartCoroutine(_Logic.CircleBullets(this, "Common/Bullet_Blue", 0.25f, 0.02f, 12, false, circleInterval, 6));
            }

            private IEnumerator PatternA_b2()
            {
                const int circleInterval = 28;
                _coroutineManager.StartCoroutine(_Logic.CircleBullets(this, "Common/Bullet_Blue", 0.25f, 0.02f, 12, true, circleInterval, 4));
                yield return new WaitForFrames(circleInterval / 2);
                _coroutineManager.StartCoroutine(_Logic.CircleBullets(this, "Common/Bullet_Blue", 0.25f, 0.02f, 12, false, circleInterval, 4));
                yield return new WaitForFrames(circleInterval * 4);

                const float lineXOffset = 0.1f;
                const float lineYOffset = 0.15f;
                Vector2 pos = _pos;
                _coroutineManager.StartCoroutine(_Logic.LineBullets(pos, "Common/Bullet_blue", 0.75f, 0.02f, 5, 10));
                pos.x = _pos.x - lineXOffset;
                pos.y = _pos.y + lineYOffset;
                _coroutineManager.StartCoroutine(_Logic.LineBullets(pos, "Common/Bullet_blue", 0.75f, 0.02f, 5, 10));
                pos.x = _pos.x + lineXOffset;
                _coroutineManager.StartCoroutine(_Logic.LineBullets(pos, "Common/Bullet_blue", 0.75f, 0.02f, 5, 10));
            }

            private IEnumerator PatternB()
            {
                const float rollingAngleRate = 0.02f;
                const float rollingAngleRange = 0.22f;
                const int rollingRepeatCount = 9;
                const float rollingAngleOffset = (rollingAngleRate * rollingRepeatCount) / 2.0f;
                const int rollingCount = 5;
                
                // 뿌리기
                _Logic.RandomSpreadBullet(this, "Common/Bullet_Red", 0.2f, 0.02f, 0.02f, 30);

                // 반시계 회전
                yield return new WaitForFrames(100);
                _coroutineManager.StartCoroutine(_Logic.RollingNWayBullets(this, "Common/Bullet_Blue"
                    , 0.75f - rollingAngleOffset, rollingAngleRange, rollingAngleRate, 0.02f, rollingCount, 1, 5, rollingRepeatCount));

                // 뿌리기
                yield return new WaitForFrames(140);
                _Logic.RandomSpreadBullet(this, "Common/Bullet_Red", 0.2f, 0.02f, 0.02f, 30);

                // 시계 회전
                yield return new WaitForFrames(100);
                _coroutineManager.StartCoroutine(_Logic.RollingNWayBullets(this, "Common/Bullet_Blue"
                    , 0.75f + rollingAngleOffset, rollingAngleRange, -rollingAngleRate, 0.02f, rollingCount, 1, 5, rollingRepeatCount));
            }

            private IEnumerator PatternC()
            {
                const float radius = 0.5f;
                for (int i = 0; i < 8; ++i)
                {
                    _coroutineManager.StartCoroutine(_Logic.RollingAimingBullets(this, "Common/Bullet_Red", 0.02f, 18, radius, 1, 2));
                    yield return new WaitForFrames(60);
                    
                    Vector2 nextPos = new Vector2(
                        GameSystem._Instance.GetRandomRange(GameSystem._Instance._MinX + radius + 0.1f, GameSystem._Instance._MaxX - radius - 0.1f)
                        , GameSystem._Instance.GetRandomRange(GameSystem._Instance._MinY * 0.2f + radius + 0.1f, GameSystem._Instance._MaxY - radius - 0.1f));
                    _coroutineManager.StartCoroutine(_Logic.MoveDamp(this, nextPos, 30, 0.1f));
                    yield return new WaitForFrames(60);
                }
                yield return null;
            }
            #endregion //Coroutine

            private GameLogic _Logic
            {
                get { return GameSystem._Instance.GetLogic<GameLogic>(); }
            }
        } // Boss
    } // TurkishMarch
} // Game