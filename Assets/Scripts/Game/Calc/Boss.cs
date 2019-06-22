using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    namespace Calc
    {
        // 보스
        public class Boss : Enemy
        {
            private const int _score = 10; // 피격시 획득 점수
            private CoroutineManager _coroutineManager = new CoroutineManager();

            #region Pattern Const
            // Circle Pendulum 패턴
            // n번째로 쏜 라인 탄이 몇 프레임에 본체와 같은 Y 위치가 되는가?
            // 본체: y=0.6sin((pi/2)+((2pi)/320)t)+0.15
            // 라인: y=-0.04(t-4n)+1.3
            private readonly float[] _pendulumFramePerLineInteval = {
                    14.3407f    // 0
                    , 18.7557f
                    , 23.2915f
                    , 27.9532f
                    , 32.745f
                    , 37.6693f
                    , 42.726f
                    , 47.9123f
                    , 53.2215f
                    , 58.6424f
                    , 64.1594f  // 10
                    , 69.7521f
                    , 75.3958f
                    , 81.0631f
                    , 86.7249f
                    , 92.3526f
                    , 97.9195f
                    , 103.403f
                    , 108.783f
                    , 114.047f
                    , 119.186f  // 20
                    , 124.193f
                    , 129.067f
                    , 133.81f
                    , 138.424f
                    , 142.914f
                    , 147.285f
                    , 151.544f
                    , 155.696f
                    , 159.75f
                    , 163.71f   // 30
                    , 167.584f
                    , 171.377f
                    , 175.096f
                    , 178.745f
                    , 182.331f
                    , 185.858f
                    , 189.331f
                    , 192.753f
                    , 196.131f
                    , 199.467f  // 40
                    , 202.765f
                    , 206.03f
                    , 209.263f
                    , 212.469f
                    , 215.651f
                    , 218.812f
                    , 221.954f
                    , 225.081f
                    , 228.196f
                    , 231.3f    // 50
                    , 234.397f
                    , 237.489f
                    , 240.579f
                    , 243.67f
                    , 246.764f
                    , 249.863f
                    , 252.971f
                    , 256.09f
                    , 259.222f
                    , 262.371f  // 60
                    , 265.539f
                    , 268.73f
                    , 271.946f
                    , 275.191f
                    , 278.467f
                    , 281.779f
                    , 285.13f
                    , 288.525f
                    , 291.966f
                    , 295.458f  // 70
                    , 299.006f
                    , 302.615f
                    , 306.29f
                    , 310.036f
                    , 313.859f
                    , 317.764f
                    , 321.759f
                    , 325.849f
                    , 330.041f
                };

            // 하트 모양 패턴
            private readonly byte[,] _patternHeart =
            {
                { 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0 },
                { 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0 },
                { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
            };

            // 노트 패턴. 슬롯 목록
            private readonly List<List<int>> _noteListSlots = new List<List<int>> {
                new List<int> { 0 },
                new List<int> { 1 },
                new List<int> { 2 },
                new List<int> { 3 },
                null,
                null,
                null,
                null,
                new List<int> { 3 },
                null,
                new List<int> { 2 },
                null,
                null,
                null,
                new List<int> { 4 },
                null,
                new List<int> { 3 },
                null,
                null,
                null,
                new List<int> { 3 },
                null,
                new List<int> { 2 },
                null,
                null,
                null,
                new List<int> { 2 },
                null,
                new List<int> { 1 },
                null,
                null,
                null,
                new List<int> { 1 },
                null,
                new List<int> { 0 },
                null,
                null,
                null,
                new List<int> { 3 },
                null,
                new List<int> { 2 },
                null,
                null,
                null,
                null,
                null,
                null,
                new List<int> { 0 },    // 파트2
                new List<int> { 1 },
                new List<int> { 2 },
                new List<int> { 3 },
                null,
                null,
                null,
                null,
                new List<int> { 3 },
                null,
                new List<int> { 2 },
                null,
                null,
                null,
                new List<int> { 1 },
                null,
                new List<int> { 0 },
                null,
                null,
                null,
                new List<int> { 3 },
                null,
                new List<int> { 4 },
                null,
                null,
                null,
                new List<int> { 0 },
                new List<int> { 1 },
                new List<int> { 2 },
                null,
                null,
                new List<int> { 1 },
                new List<int> { 2 },
                new List<int> { 3 },
                null,
                null,
                new List<int> { 2 },
                new List<int> { 3 },
                new List<int> { 4 },
                null,
                null,
                new List<int> { 1, 3 },
                new List<int> { 0, 4 },
            };
            #endregion Pattern Const

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
                _coroutineManager.StartCoroutine(Pattern_PlacedCircleWave(false, 0.0f));

                yield return new WaitForAbsFrames(1374);
                _coroutineManager.StartCoroutine(Pattern_SideCircle());

                yield return new WaitForAbsFrames((int)(43.9f * 60));
                _coroutineManager.StartCoroutine(Pattern_Circle3());

                yield return new WaitForAbsFrames(2700);
                Pattern_Pendulumn();

                // 원래 타이밍 4040
                // 화면 밖에서 내려오는 시간 고려해서 좀 더 빠르게
                yield return new WaitForAbsFrames(4030);
                _coroutineManager.StartCoroutine(Pattern_NoteList());

                yield return new WaitForAbsFrames(4690);
                _coroutineManager.StartCoroutine(Pattern_PlacedCircleWave(true, 0.0005f));

                yield return new WaitForAbsFrames(5270);
                _coroutineManager.StartCoroutine(Pattern_SlowCircleWaveExplosion());

                yield return new WaitForAbsFrames(6690);
                _coroutineManager.StartCoroutine(Pattern_Pendulumn2());

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
            private IEnumerator Pattern_PlacedCircleWave(bool bSkipCircleHalf, float angleRate)
            {
                const float angle = 0.75f;
                const float speed1 = 0.03f;
                const float speed2 = 0.01f;
                const int phase1Duration = 30;

                const int bulletPerCircle = 40;
                const int circlePerWave = 7;
                const int waveCount = 7;

                const int circleInterval = 10;
                const int waveInterval = 12;

                for (int wave = 0; wave < waveCount; ++wave)
                {
                    bool bBlue = wave % 2 == 0;
                    string shape = bBlue ? "Common/Bullet_Blue" : "Common/Bullet_Red";
                    bool bHalfAngleOffset = !bBlue;
                    float angleStart = angle + ((bHalfAngleOffset) ? (1.0f / bulletPerCircle / 2.0f) : 0.0f);
                    float waveAngleRate = bBlue ? angleRate : -angleRate;

                    for (int circle = 0; circle < circlePerWave; ++circle)
                    {
                        for (int i = 0; i < bulletPerCircle; ++i)
                        {
                            float angle1 = angleStart + (1.0f / bulletPerCircle * i);
                            bool bSkip = bSkipCircleHalf && (circle % 2 == 1);

                            if (!bSkip)
                            {
                                PlacedBullet b = GameSystem._Instance.CreateBullet<PlacedBullet>();
                                b.Init(shape, _X, _Y
                                    , angle1, 0.0f, speed1, 0.0f
                                    , phase1Duration, 0
                                    , angle1, waveAngleRate, speed2, 0.0f);
                            }
                        }

                        yield return new WaitForFrames(circleInterval);
                    }

                    yield return new WaitForFrames(waveInterval);
                }

                _Logic.NWayBullet(this, "Common/Bullet_RedLarge", 0.75f, 0.5f, 0.01f, 20);
            }

            private IEnumerator Pattern_SideCircle()
            {
                yield return _coroutineManager.StartCoroutine(Pattern_SideCircle_Turn(15, false));
                yield return _coroutineManager.StartCoroutine(Pattern_SideCircle_Turn(14, true));
                yield return _coroutineManager.StartCoroutine(Pattern_SideCircle_Both(4));
            }

            private IEnumerator Pattern_SideCircle_Turn(int waveCount, bool bMixed)
            {
                const float circleOffsetX = 0.3f;
                const int bulletPerCircle = 20;
                const int circlePerWave = 2;
                const int circleInterval = 20;
                const bool bHalfAngleOffset = true;

                for (int wave = 0; wave < waveCount; ++wave)
                {
                    bool bEvenWave = bMixed && (wave % 2 == 1);
                    float speed = bEvenWave ? 0.015f : 0.01f;
                    string shape = bEvenWave ? "Common/Bullet_Red" : "Common/Bullet_Blue";
                    float angle = GameSystem._Instance.GetRandom01();

                    for (int circle = 0; circle < circlePerWave; ++circle)
                    {
                        bool bLeft = circle % 2 == 0;
                        float x = _X + (bLeft ? -1.0f : 1.0f) * circleOffsetX;
                        float y = _Y;

                        _Logic.CircleBullet(x, y, shape, angle, speed, 0.0f, bulletPerCircle, bHalfAngleOffset);
                        yield return new WaitForFrames(circleInterval);
                    }
                }
            }

            private IEnumerator Pattern_SideCircle_Both(int waveCount)
            {
                const float circleOffsetX = 0.3f;
                const int bulletPerCircle = 10;
                const int circlePerWave = 2;
                const int waveInterval = 20;

                for (int wave = 0; wave < waveCount; ++wave)
                {
                    bool bEvenWave = (wave % 2 == 1);
                    float speed = 0.03f;
                    string shape = "Common/Bullet_BlueLarge";
                    bool bHalfAngleOffset = (bEvenWave) ? false : true;

                    for (int circle = 0; circle < circlePerWave; ++circle)
                    {
                        bool bLeft = circle % 2 == 0;
                        float x = _X + (bLeft ? -1.0f : 1.0f) * circleOffsetX;
                        float y = _Y;

                        _Logic.CircleBullet(x, y, shape, 0.75f, speed, 0.0f, bulletPerCircle, bHalfAngleOffset);
                    }

                    yield return new WaitForFrames(waveInterval);
                }
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

                _coroutineManager.StartCoroutine(Pattern_Pendulum_Move(rotationCount, rotationDuration, rotationRadius, moverStartRad, false));
                _coroutineManager.StartCoroutine(Pattern_Pendulum_Line(rotationCount, rotationDuration, rotationRadius, moverStartRad));
                _coroutineManager.StartCoroutine(Pattern_Pendulum_NWay());
            }

            private IEnumerator Pattern_Pendulumn2()
            {
                const int rotationCount = 4;
                const int rotationDuration = 320; // 1회 회전에 걸리는 프레임
                const float rotationRadius = 0.6f;
                const float moverStartRad = (Mathf.PI / 2.0f); // 90도 위치부터 시작

                _coroutineManager.StartCoroutine(Pattern_Pendulum_Move(rotationCount, rotationDuration, rotationRadius, moverStartRad, true));
                _coroutineManager.StartCoroutine(Pattern_CirclePendulum_Line(rotationCount, rotationDuration, rotationRadius, moverStartRad));
                _coroutineManager.StartCoroutine(Pattern_Pendulum_NWay());

                yield return new WaitForFrames(rotationDuration * 3);
                _coroutineManager.StartCoroutine(Pattern_CirclePendulum_Draw(_patternHeart, 0.1f, rotationDuration + 120));
            }

            private IEnumerator Pattern_Pendulum_Move(int rotationCount, int rotationDuration, float rotationRadius, float startRad, bool moveY)
            {
                Vector2 startPos = _pos;
                Vector2 centerPos = new Vector2(startPos.x, startPos.y - rotationRadius);
                float radSpeed = (Mathf.PI * 2.0f / rotationDuration);

                for (int rotation = 0; rotation < rotationCount; ++rotation)
                {
                    for (int i = 0; i < rotationDuration; ++i)
                    {
                        float rad = startRad + radSpeed * i;
                        _X = centerPos.x + rotationRadius * Mathf.Cos(rad);

                        if (moveY)
                        {
                            _Y = centerPos.y + rotationRadius * Mathf.Sin(rad);
                        }

                        yield return null;
                    }
                }

                _pos = startPos;
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

            private IEnumerator Pattern_CirclePendulum_Line(int rotationCount, int rotationDuration, float rotationRadius, float moverStartRad)
            {
                float[] offsetXs = new float[2] { -0.4f, 0.4f };
                const int interval = 4;
                const string shape = "Common/Bullet_Blue";
                const float fireStartY = 1.30f;
                const float angle = 0.75f;
                const float speed = 0.04f;

                float pendulumCenterX = _X;
                float radSpeed = (Mathf.PI * 2.0f / rotationDuration);

                for (int rotation = 0; rotation < rotationCount; ++rotation)
                {
                    for (int rotationFrame = 0; rotationFrame < rotationDuration; ++rotationFrame)
                    {
                        if ((rotationFrame % interval) == 0)
                        {
                            int pendulumFrameIdx = rotationFrame / interval;

                            if (pendulumFrameIdx < _pendulumFramePerLineInteval.Length)
                            {
                                float pendulumFrame = _pendulumFramePerLineInteval[pendulumFrameIdx];
                                float pendulumRad = moverStartRad + radSpeed * pendulumFrame;
                                float pendulumX = pendulumCenterX + rotationRadius * Mathf.Cos(pendulumRad);

                                foreach (float offsetX in offsetXs)
                                {
                                    Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                                    b.Init(shape, pendulumX + offsetX, fireStartY, angle, 0.0f, speed, 0.0f);
                                }
                            }
                            else
                            {
                                Debug.LogError(
                                    string.Format("Invalid Idx. pendulumFrameIdx: {0}, pendulumFramePerLineInteval.Length: {1}"
                                    , pendulumFrameIdx, _pendulumFramePerLineInteval.Length)
                                    );
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

            private IEnumerator Pattern_CirclePendulum_Draw(byte[,] pattern, float patternSpace, int waitEndFrame)
            {
                const int interval = 8;
                Vector2 patternCenterPos = new Vector2(0.0f, 0.0f);
                const float speed1 = 0.01f; // 목표지점까지 날아가는 속도
                const float speed2 = 0.025f; // 정지 종료 후 밖으로 날아가는 속도
                const string shape = "Common/Bullet_Red";

                int row = pattern.GetLength(0);
                int col = pattern.GetLength(1);
                List<Vector2Int> indexes = new List<Vector2Int>();

                for (int r = 0; r < row; ++r)
                {
                    for (int c = 0; c < col; ++c)
                    {
                        if (pattern[r, c] != 0)
                        {
                            indexes.Add(new Vector2Int(r, c));
                        }
                    }
                }

                // indexes 랜덤하게 섞기
                BaseGameLogic.Shuffle(indexes);

                int waitEndAbsFrame = _Frame + waitEndFrame;
                // 좌표계 우상단이 (+, +)
                float patternWidth = patternSpace * (col - 1);
                float patternHeight = patternSpace * (row - 1);
                Vector2 patternLeftTopPos = new Vector2(patternCenterPos.x - (patternWidth / 2.0f), patternCenterPos.y + (patternHeight / 2.0f));

                foreach (var patternIndex in indexes)
                {
                    // 목표 지점
                    int r = patternIndex.x;
                    int c = patternIndex.y;
                    Vector2 targetPos = patternLeftTopPos + new Vector2(c * patternSpace, r * -patternSpace);

                    // 현재 위치에서 발사한 탄이 목표지점에 도달하기까지 걸리는 시간
                    int moveDuration = (int)(Vector2.Distance(_pos, targetPos) / speed1);

                    // 목표 지점 도달 후 정지 해 있을 시간
                    int stopDuration = waitEndAbsFrame - (_Frame + moveDuration);

                    // 정지 종료 후 패턴 중앙의 반대방향으로
                    float angle2 = BaseGameLogic.CalcluatePointToPointAngle(patternCenterPos, targetPos);

                    PosPlacedBullet b = GameSystem._Instance.CreateBullet<PosPlacedBullet>();
                    b.Init(shape, _pos.x, _pos.y, targetPos, speed1, 0.0f, moveDuration, stopDuration, angle2, 0.0f, speed2, 0.0f);

                    yield return new WaitForFrames(interval);
                }
            }

            private IEnumerator Pattern_RotateCross()
            {
                int inverval = 20;
                _coroutineManager.StartCoroutine(_Logic.MultipleSpiralBullets(this, "Common/Bullet_BlueLarge", 0.75f, -0.1f, 0.01f, 4, inverval, 5 * 60));
                //_coroutineManager.StartCoroutine(_Logic.MultipleSpiralBullets(this, "Common/Bullet_RedLarge", 0.125f, -0.1f, 0.01f, 4, inverval, 5 * 60));
                yield return new WaitForFrames(inverval);
                //
                //yield return new WaitForFrames(inverval / 2);
                //_coroutineManager.StartCoroutine(_Logic.MultipleSpiralBullets(this, "Common/Bullet_Blue", 0.125f, -0.1f, 0.01f, 4, inverval * 2, 5 * 60));
            }

            private IEnumerator Pattern_RotateCross_Rain(int waveCount, int interval)
            {
                int bulletCount = 3;
                float rangeY = GameSystem._Instance._MaxY - 0.05f;
                float rangeXMin = GameSystem._Instance._MinX + 0.1f;
                float rangeXMax = GameSystem._Instance._MaxX - 0.1f;

                for (int wave = 0; wave < waveCount; ++wave)
                {
                    for (int bullet = 0; bullet < bulletCount; ++bullet)
                    {
                        float x = GameSystem._Instance.GetRandomRange(rangeXMin, rangeXMax);
                        float y = rangeY;

                        Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                        b.Init("Common/Bullet_Red", x, y, 0.75f, 0.0f, 0.01f, 0.0f);
                    }

                    yield return new WaitForFrames(interval);
                }
            }

            private IEnumerator Pattern_NoteList()
            {
                int interval = 7;

                foreach (var slotsOnTick in _noteListSlots)
                {
                    if (slotsOnTick != null)
                    {
                        foreach (var slot in slotsOnTick)
                        {
                            Pattern_Node(slot);
                        }
                    }

                    yield return new WaitForFrames(interval);
                }
            }

            private void Pattern_Node(int slot)
            {
                float boardXMin = GameSystem._Instance._MinX;
                float boardXMax = GameSystem._Instance._MaxX;
                float startY = GameSystem._Instance._MaxY + 0.05f;

                const int slotCount = 5;
                float slotWidth = (boardXMax - boardXMin) / slotCount;
                float slotXMin = boardXMin + slotWidth * slot;
                float slotXMax = slotXMin + slotWidth;

                const int bulletPerSlot = 5;
                const float bulletRadius = 0.04f;
                float bulletXMin = slotXMin + bulletRadius;
                float bulletXMax = slotXMax - bulletRadius;
                float bulletXSpace = (bulletXMax - bulletXMin) / (bulletPerSlot - 1);

                for (int i = 0; i < bulletPerSlot; ++i)
                {
                    float x = bulletXMin + bulletXSpace * i;
                    float y = startY;

                    Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Red", x, y, 0.75f, 0.0f, 0.012f, 0.0f);
                }
            }

            private IEnumerator Pattern_SlowCircleWaveExplosion()
            {
                Vector2 startPos = _pos;

                _coroutineManager.StartCoroutine(_Logic.MoveConstantVelocity(this, new Vector2(0.0f, 0.0f), 420));
                yield return new WaitForFrames(90);
                yield return _coroutineManager.StartCoroutine(Pattern_SlowCircleWave(7, 0, 0, 0.0f, -1, 0.0f));

                const int explosionAbsFrame = 6513;
                _coroutineManager.StartCoroutine(Pattern_SlowCircleWave(6, -4, -5, -0.0001f, 6513, 0.04f));
                yield return new WaitForAbsFrames(explosionAbsFrame);
                _coroutineManager.StartCoroutine(_Logic.MoveDamp(this, startPos, 60, 0.1f));

                yield return new WaitForFrames(90);
                _Logic.NWayBullet(this, "Common/Bullet_RedLarge", 0.75f, 0.3f, 0.02f, 10);
                yield return new WaitForFrames(12);
                _Logic.NWayBullet(this, "Common/Bullet_RedLarge", 0.75f, 0.3f, 0.02f, 11);
            }

            private IEnumerator Pattern_SlowCircleWave(int waveCount, int bulletPerCircleRate, int phase1DurationRate, float speedRate2
                , int explosionAbsFrame, float explosionSpeed)
            {
                const int initialBulletPerCircle = 40;
                const int waveInterval = 84;

                const string shape = "Common/Bullet_Blue";
                const float speed1 = 0.03f;
                const float speed2 = 0.01f;
                const int initialPhase1Duration = 24;

                for (int wave = 0; wave < waveCount; ++wave)
                {
                    int bulletPerCircle = Mathf.Max(initialBulletPerCircle + wave * bulletPerCircleRate, 1);
                    int phase1Duration = Mathf.Max(initialPhase1Duration + wave * phase1DurationRate, 0);
                    float angleStart = GameSystem._Instance.GetRandom01();

                    for (int i = 0; i < bulletPerCircle; ++i)
                    {
                        float angle1 = angleStart + (1.0f / bulletPerCircle * i);

                        SlowPlacedBullet b = GameSystem._Instance.CreateBullet<SlowPlacedBullet>();
                        b.Init(shape, _X, _Y
                            , angle1, 0.0f, speed1, 0.0f
                            , phase1Duration, 0
                            , angle1, 0.0f, speed2, speedRate2, 0.0f
                            , explosionAbsFrame, explosionSpeed);
                    }

                    yield return new WaitForFrames(waveInterval);
                }
            }
            #endregion //Coroutine

            private GameLogic _Logic
            {
                get { return GameSystem._Instance.GetLogic<GameLogic>(); }
            }
        } // Boss
    } // Calc
} // Game