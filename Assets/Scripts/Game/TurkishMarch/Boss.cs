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
            private int _patternDPartDuration = 60 * 14;    // 패턴 D의 파트별 지속시간
            private readonly byte[] _patternNote =  // 음표모양 패턴
            {
                0, 0, 0, 1, 1, 0, 0,
                0, 0, 0, 1, 0, 1, 0,
                0, 0, 0, 1, 0, 0, 1,
                0, 0, 0, 1, 0, 1, 0,
                0, 0, 0, 1, 0, 0, 0,
                0, 0, 0, 1, 0, 0, 0,
                0, 1, 1, 1, 0, 0, 0,
                1, 1, 1, 1, 1, 0, 0,
                0, 1, 1, 1, 0, 0, 0,
            };
            private readonly int _patternNoteCol = 7;
            private readonly int _patternNoteRow = 9;

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
                _coroutineManager.StartCoroutine(PatternA_22());
                yield return new WaitForAbsFrames(1800);
                _coroutineManager.StartCoroutine(PatternB());
                yield return new WaitForAbsFrames(2235);
                _coroutineManager.StartCoroutine(PatternA_22());
                yield return new WaitForAbsFrames(2670);
                _coroutineManager.StartCoroutine(PatternC_1());
                yield return new WaitForAbsFrames(3590);
                _coroutineManager.StartCoroutine(PatternD_1());
                yield return new WaitForAbsFrames(5270);
                _coroutineManager.StartCoroutine(PatternD_2());
                yield return new WaitForAbsFrames(6170);
                _coroutineManager.StartCoroutine(PatternC_2());
                
                yield return new WaitForAbsFrames(7100);
                // 패턴E 빠져나올 수 있을만큼 위로
                _coroutineManager.StartCoroutine(_Logic.MoveConstantVelocity(this, new Vector2(0.0f, 0.94f), 60));
                _coroutineManager.StartCoroutine(PatternE_1());
                
                // 시간 때우기
                yield return new WaitForAbsFrames(7880);
                _coroutineManager.StartCoroutine(_Logic.AimingLineBullets(this, "Common/Bullet_Red", 0.02f, 5, 5));
                yield return new WaitForFrames(70);
                _coroutineManager.StartCoroutine(Aiming2LineBullets(this, "Common/Bullet_Red", 0.02f, 0.1f, 5, 5));
                yield return new WaitForFrames(50);
                _coroutineManager.StartCoroutine(_Logic.AimingLineBullets(this, "Common/Bullet_Red", 0.02f, 5, 9));
                yield return new WaitForFrames(110);
                _coroutineManager.StartCoroutine(_Logic.RandomAngleCircleBullets(this, "Common/Bullet_Blue", 0.02f, 12, 15, 5));

                yield return new WaitForAbsFrames(8250);
                _coroutineManager.StartCoroutine(PatternE_2());

                // 폭발
                yield return new WaitForAbsFrames(8870);
                {
                    Effect crashEffect = GameSystem._Instance.CreateEffect<Effect>();
                    crashEffect.Init("Common/Effect_BossCrashRed", _X, _Y, 0.0f);
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
            private IEnumerator PatternA_11()
            {
                yield return _coroutineManager.StartCoroutine(PatternA_a1());
                yield return new WaitForFrames(100);
                yield return _coroutineManager.StartCoroutine(PatternA_b1());
            }

            private IEnumerator PatternA_22()
            {
                yield return _coroutineManager.StartCoroutine(PatternA_a2());
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
                _coroutineManager.StartCoroutine(_Logic.RandomAngleCircleBullets(this, "Common/Bullet_Blue", 0.02f, 12, circleInterval, 7));
                yield return new WaitForFrames(circleInterval / 2);
                _coroutineManager.StartCoroutine(_Logic.RandomAngleCircleBullets(this, "Common/Bullet_Blue", 0.02f, 12, circleInterval, 6));
            }

            private IEnumerator Aiming2LineBullets(Mover mover, string shape, float speed, float gap, int interval, int repeatCount)
            {
                // 발사 시작 시 플레이어와의 각도 계산
                float angle = _Logic.GetPlayerAngle(mover);
                float rad = angle * Mathf.PI * 2.0f;
                float sin = Mathf.Sin(rad);
                float cos = Mathf.Cos(rad);
                float xOffset = 0.0f;
                float yOffset1 = gap;
                float yOffset2 = gap * -1.0f;
                float x1 = mover._X + (cos * xOffset + -1.0f * sin * yOffset1);
                float y1 = mover._Y + (sin * xOffset + cos * yOffset1);
                float x2 = mover._X + (cos * xOffset + -1.0f * sin * yOffset2);
                float y2 = mover._Y + (sin * xOffset + cos * yOffset2);

                for (int i = 0; i < repeatCount; ++i)
                {
                    Bullet b1 = GameSystem._Instance.CreateBullet<Bullet>();
                    b1.Init(shape, x1, y1, angle, 0.0f, speed, 0.0f);
                    Bullet b2 = GameSystem._Instance.CreateBullet<Bullet>();
                    b2.Init(shape, x2, y2, angle, 0.0f, speed, 0.0f);

                    if (i < repeatCount - 1)
                    {
                        yield return new WaitForFrames(interval);
                    }
                }
            }

            private IEnumerator PatternA_a2()
            {
                const int interval = 5;
                _coroutineManager.StartCoroutine(Aiming2LineBullets(this, "Common/Bullet_Red", 0.02f, 0.15f, interval, 5));
                yield return new WaitForFrames(50);
                _coroutineManager.StartCoroutine(_Logic.AimingLineBullets(this, "Common/Bullet_Red", 0.02f, interval, 5));
                yield return new WaitForFrames(50);
                _coroutineManager.StartCoroutine(Aiming2LineBullets(this, "Common/Bullet_Red", 0.02f, 0.15f, interval, 4));
                yield return new WaitForFrames(25);
                _coroutineManager.StartCoroutine(_Logic.AimingNWayLineBullets(this, "Common/Bullet_Red", 0.02f, interval, 9, 0.125f, 3));
            }

            private IEnumerator PatternA_b2()
            {
                const int circleInterval = 28;
                _coroutineManager.StartCoroutine(_Logic.RandomAngleCircleBullets(this, "Common/Bullet_Blue", 0.02f, 12, circleInterval, 4));
                yield return new WaitForFrames(circleInterval / 2);
                _coroutineManager.StartCoroutine(_Logic.RandomAngleCircleBullets(this, "Common/Bullet_Blue", 0.02f, 12, circleInterval, 4));
                yield return new WaitForFrames(circleInterval * 4);

                _coroutineManager.StartCoroutine(_Logic.PatternBullets(this, "Common/Bullet_Blue", 0.75f, 0.02f, 4, _patternNote, _patternNoteCol, _patternNoteRow, 0.08f));
            }

            private IEnumerator PatternB()
            {
                const float rollingAngleRate = 0.02f;
                const float rollingAngleRange = 0.22f;
                const int rollingRepeatCount = 9;
                const float rollingAngleOffset = (rollingAngleRate * rollingRepeatCount) / 2.0f;
                const int rollingCount = 5;
                
                // 뿌리기
                _Logic.RandomSpreadBullet(this, "Common/Bullet_Red", 0.2f, 0.02f, 0.02f, 24);

                // 반시계 회전
                yield return new WaitForFrames(100);
                _coroutineManager.StartCoroutine(_Logic.RollingNWayBullets(this, "Common/Bullet_Blue"
                    , 0.75f - rollingAngleOffset, rollingAngleRange, rollingAngleRate, 0.02f, rollingCount, 1, 5, rollingRepeatCount));

                // 뿌리기
                yield return new WaitForFrames(140);
                _Logic.RandomSpreadBullet(this, "Common/Bullet_Red", 0.2f, 0.02f, 0.02f, 24);

                // 시계 회전
                yield return new WaitForFrames(100);
                _coroutineManager.StartCoroutine(_Logic.RollingNWayBullets(this, "Common/Bullet_Blue"
                    , 0.75f + rollingAngleOffset, rollingAngleRange, -rollingAngleRate, 0.02f, rollingCount, 1, 5, rollingRepeatCount));
            }

            /// <summary>
            /// 회전하며 조준탄 뿌린 후 랜덤이동
            /// </summary>
            private IEnumerator PatternC_1()
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
            }

            private IEnumerator PatternC_2()
            {
                const float radius = 0.5f;
                const int repeatCount = 8;
                const int countPerCircle = 10;
                const float speed1_Cirlce1 = 0.04f;
                const float speed1_Cirlce2 = 0.03f;
                const float angle2 = 0.75f;
                const float speed2 = 0.02f;
                const int moveDuaraion = 12;
                const int stopDuaraion = 24;
                const int placeInterval = 12;
                for (int i = 0; i < repeatCount; ++i)
                {
                    // 설치 원형탄 2개
                    float angle = 0.75f;
                    _Logic.PlacedCircleBullet(this, "Common/Bullet_Blue", angle, speed1_Cirlce1, countPerCircle, false, moveDuaraion, stopDuaraion, angle2, speed2);
                    yield return new WaitForFrames(placeInterval);
                    _Logic.PlacedCircleBullet(this, "Common/Bullet_Red", angle, speed1_Cirlce2, countPerCircle, true, moveDuaraion, stopDuaraion - placeInterval, angle2, speed2);

                    // 마지막 뿌리기 후에는 이동하지 않음
                    if (i < repeatCount - 1)
                    {
                        yield return new WaitForFrames(60 - placeInterval);
                        Vector2 nextPos = new Vector2(
                            GameSystem._Instance.GetRandomRange(GameSystem._Instance._MinX + radius + 0.1f, GameSystem._Instance._MaxX - radius - 0.1f)
                            , GameSystem._Instance.GetRandomRange(GameSystem._Instance._MinY * 0.2f + radius + 0.1f, GameSystem._Instance._MaxY - radius - 0.1f));
                        _coroutineManager.StartCoroutine(_Logic.MoveDamp(this, nextPos, 30, 0.1f));
                        yield return new WaitForFrames(60);
                    }
                }
            }

            private IEnumerator PatternD_1()
            {
                yield return _coroutineManager.StartCoroutine(PatternD_1_Follow());
                _coroutineManager.StartCoroutine(PatternD_1_Circle()); // 원형탄과 함께 따라다니기
                yield return _coroutineManager.StartCoroutine(PatternD_1_Follow());
            }

            /// <summary>
            /// 캐릭터 따라가며 탄 설치
            /// </summary>
            private IEnumerator PatternD_1_Follow()
            {
                const float speed = 0.007f;
                const float maxAngleRate = 0.01f; // 최대 선회 각속도
                const int interval = 10; // 발사 간격
                const string shape = "Common/Bullet_Blue";
                float angle = _Logic.GetPlayerAngle(this);

                for (int i = 0; i < _patternDPartDuration; ++i)
                {
                    // 현재 위치에 패턴 종료시까지 움직이지 않는 탄 생성
                    if (i % interval == 0)
                    {
                        AwayBullet b = GameSystem._Instance.CreateBullet<AwayBullet>();
                        b.Init(shape, _X, _Y, 0.0f, 0.0f, 0.0f, 0.0f, _patternDPartDuration - i - 1, 0.03f);
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

            /// <summary>
            /// 주기적으로 원형탄 발사
            /// </summary>
            private IEnumerator PatternD_1_Circle()
            {
                for (int i = 0; i < 14; ++i)
                {
                    _Logic.CircleBullet(this, "Common/Bullet_Red", GameSystem._Instance.GetRandom01(), 0.01f, 6, true);
                    yield return new WaitForFrames(60);
                }
            }

            private IEnumerator PatternD_2()
            {
                // 안전선 발사
                PatternD_2_SafetyLine();
                // 보스 기준위치로 이동
                yield return _coroutineManager.StartCoroutine(_Logic.MoveConstantVelocity(this, new Vector2(0.0f, 0.75f), 240));
                yield return new WaitForFrames(240);
                // 반원 발사
                yield return _coroutineManager.StartCoroutine(PatternD_2_HalfCirclePlaced());
            }

            /// <summary>
            /// 이후 패턴을 위한 안전선
            /// </summary>
            private void PatternD_2_SafetyLine()
            {
                const float speed1 = 0.0045f;
                const float speed2 = 0.01f;
                const int phase1Duration = 480;
                const int count = 10;
                const string shape = "Common/Bullet_Red";

                float startX = GameSystem._Instance._MinX;
                float gapX = (GameSystem._Instance._MaxX - GameSystem._Instance._MinX) / (count - 1);
                float y = GameSystem._Instance._MaxY;

                for (int i = 0; i < count; ++i)
                {
                    // 아래로 내려오다가
                    // 페이즈 2 때 절반은 왼쪽으로, 절반은 오른쪽으로 사라짐
                    PlacedBullet b = GameSystem._Instance.CreateBullet<PlacedBullet>();
                    b.InitNoStop(shape, startX + (i * gapX), y, 0.75f, speed1
                        , phase1Duration, (i < (count / 2) ? 0.5f : 0.0f), speed2);
                }
            }

            /// <summary>
            /// 반원 배치로 2단계 탄 발사
            /// </summary>
            private IEnumerator PatternD_2_HalfCirclePlaced()
            {
                // 반원 배치로 빠르게 진행하다가 하단으로 천천히 떨어짐
                const float speed1 = 0.05f;
                const float speed2 = 0.01f;
                const int phase1Duration = 30;
                const int count = 12;
                const float angleRange = 100.0f / 360.0f;
                const float startAngleOffset = (angleRange / (float)(count - 1)) / 2.0f;
                const int interval = 20;
                const string shape = "Common/Bullet_Blue";

                for (int frame = 0; frame < (_patternDPartDuration / 2); ++frame)
                {
                    if (frame % interval == 0)
                    {
                        float startAngle = 0.75f + GameSystem._Instance.GetRandomRange(-startAngleOffset, startAngleOffset);
                        for (int i = 0; i < count; ++i)
                        {
                            PlacedBullet b = GameSystem._Instance.CreateBullet<PlacedBullet>();
                            b.InitNoStop(shape, this._X, this._Y, startAngle + angleRange * ((float)i / (count - 1) - 0.5f), speed1
                                , phase1Duration, 0.75f, speed2);
                        }
                    }
                    yield return null;
                }
            }

            private IEnumerator PatternE_1()
            {
                const int shooterCount = 2;
                const int cycle = 700;
                SpiralPlacedShooterBullet[] bs = new SpiralPlacedShooterBullet[shooterCount];
                for (int i = 0; i < shooterCount; ++i)
                {
                    string shape = (i % 2 == 0) ? "Common/Bullet_BlueLarge" : "Common/Bullet_RedLarge";
                    string bulletShape = (i % 2 == 0) ? "Common/Bullet_BlueSmall" : "Common/Bullet_RedSmall";
                    float orbitAngle = 1.0f * ((float)i / (float)shooterCount);

                    bs[i] = GameSystem._Instance.CreateBullet<SpiralPlacedShooterBullet>();
                    bs[i].Init(shape, orbitAngle, 0.002f, 0.9f
                        , 30, 30, 3, cycle
                        , bulletShape, 0.01f, 8);
                }

                // 사이클 후 슈터 삭제
                yield return new WaitForFrames(cycle - 1);
                for (int i = 0; i < shooterCount; ++i)
                {
                    bs[i]._alive = false;
                }
            }

            private IEnumerator PatternE_2()
            {
                const int shooterCount = 4;
                const int cycle = 620;
                SpiralPlacedShooterBullet[] bs = new SpiralPlacedShooterBullet[shooterCount];
                for (int i = 0; i < shooterCount; ++i)
                {
                    bool isBlue = (i % 2 == 0);
                    string shape = (isBlue) ? "Common/Bullet_BlueLarge" : "Common/Bullet_RedLarge";
                    string bulletShape = (isBlue) ? "Common/Bullet_BlueSmall" : "Common/Bullet_RedSmall";
                    float orbitAngle = ((float)1 / (float)shooterCount / 2.0f) + ((float)i / (float)shooterCount);
                    float orbitAngleRate = 0.002f;
                    float orbitRadius = (isBlue) ? 0.9f : 0.8f;
                    int shotTime = (isBlue) ? 20 : 25;
                    int waitTime = (isBlue) ? 40 : 35;
                    float bulletSpeed = (isBlue) ? 0.01f : 0.008f;

                    bs[i] = GameSystem._Instance.CreateBullet<SpiralPlacedShooterBullet>();
                    bs[i].Init(shape, orbitAngle, orbitAngleRate, orbitRadius
                        , shotTime, waitTime, 4, 700
                        , bulletShape, bulletSpeed, 6);
                }

                // 사이클 후 슈터 삭제
                yield return new WaitForFrames(cycle - 1);
                for (int i = 0; i < shooterCount; ++i)
                {
                    bs[i]._alive = false;
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