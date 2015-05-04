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
                yield return new WaitForAbsFrames(3590);
                _coroutineManager.StartCoroutine(PatternD());
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

            /// <summary>
            /// 회전하며 조준탄 뿌린 후 랜덤이동
            /// </summary>
            private IEnumerator PatternC()
            {
                const float radius = 0.5f;
                const int repeatCount = 8;
                for (int i = 0; i < repeatCount; ++i)
                {
                    _coroutineManager.StartCoroutine(_Logic.RollingAimingBullets(this, "Common/Bullet_Red", 0.02f, 18, radius, 1, 2));

                    // 마지막 뿌리기 후에는 이동하지 않음
                    if (i < repeatCount - 1)
                    {
                        yield return new WaitForFrames(60);
                        Vector2 nextPos = new Vector2(
                            GameSystem._Instance.GetRandomRange(GameSystem._Instance._MinX + radius + 0.1f, GameSystem._Instance._MaxX - radius - 0.1f)
                            , GameSystem._Instance.GetRandomRange(GameSystem._Instance._MinY * 0.2f + radius + 0.1f, GameSystem._Instance._MaxY - radius - 0.1f));
                        _coroutineManager.StartCoroutine(_Logic.MoveDamp(this, nextPos, 30, 0.1f));
                        yield return new WaitForFrames(60);
                    }
                }
                yield return null;
            }

            /// <summary>
            /// 캐릭터 따라가며 탄 설치
            /// </summary>
            private IEnumerator PatternD()
            {
                const float speed = 0.01f;
                const float maxAngleRate = 0.01f; // 최대 선회 각속도
                const int interval = 5; // 발사 간격
                const int duration = 60 * 14;
                const string shape = "Common/Bullet_Blue";
                float angle = _Logic.GetPlayerAngle(this);

                for (int i = 0; i < duration; ++i)
                {
                    // 현재 위치에 움직이지 않는 탄 생성
                    if (i % interval == 0)
                    {
                        Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                        b.Init(shape, _X, _Y, 0.0f, 0.0f, 0.0f, 0.0f);
                    }

                    // 선회 각속도 계산
                    float angleRate = _Logic.GetPlayerAngle(this) - angle;
                    // 선회 각속도를 0~1 범위로 제한
                    angleRate -= Mathf.Floor(angleRate);

                    // 선회 각속도가 최대 선회 각속도 이하면
                    // 선회 각속도로 선회
                    if (angleRate <= maxAngleRate || (1.0f - angleRate) <= maxAngleRate)
                    {
                        angle += angleRate;
                    }
                    // 선회 각속도가 최대 선회 각속도보다 크면
                    // 최대 선회 각속도로 선회
                    else
                    {
                        angle += (angleRate < 0.5f) ? maxAngleRate : -maxAngleRate;
                    }
                    angle -= Mathf.Floor(angle);

                    // 계산한 각도를 사용해 좌표 갱신
                    float rad = angle * Mathf.PI * 2.0f;
                    _X += speed * Mathf.Cos(rad);
                    _Y += speed * Mathf.Sin(rad);

                    yield return null;
                }
            }
            #endregion //Coroutine

            private GameLogic _Logic
            {
                get { return GameSystem._Instance.GetLogic<GameLogic>(); }
            }
        } // Boss
    } // TurkishMarch
} // Game