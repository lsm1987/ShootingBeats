using UnityEngine;
using System.Collections;

namespace Game
{
    namespace IevanPolkka
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

                yield return new WaitForAbsFrames(4386);
                // 간주(야바린간)
                _coroutineManager.StartCoroutine(_Logic.MoveConstantVelocity(this, new Vector2(-0.8f, 0.75f), 120));
                yield return new WaitForFrames(120);
                _coroutineManager.StartCoroutine(_Logic.MoveConstantVelocity(this, new Vector2(0.8f, 0.75f), 240));
                yield return new WaitForFrames(240);
                _coroutineManager.StartCoroutine(_Logic.MoveConstantVelocity(this, new Vector2(0.0f, 0.75f), 120));

                {
                    yield return new WaitForAbsFrames(5586);
                    // 마마마 린간 덴간 린간 덴간
                    const float pivotX = 0.0f;
                    const float pivotY = 0.75f;
                    const float pivotOffset = 0.1f;
                    // 마마마 린간 덴간
                    _coroutineManager.StartCoroutine(_Logic.MoveConstantVelocity(this, new Vector2(pivotX + pivotOffset, pivotY), 30));
                    yield return new WaitForFrames(75);
                    // 린간 덴간
                    _coroutineManager.StartCoroutine(_Logic.MoveConstantVelocity(this, new Vector2(pivotX - pivotOffset, pivotY), 30));
                    yield return new WaitForFrames(75);
                    // 린간
                    _coroutineManager.StartCoroutine(_Logic.MoveConstantVelocity(this, new Vector2(pivotX + pivotOffset, pivotY), 15));
                    yield return new WaitForFrames(30);
                    // 린간
                    _coroutineManager.StartCoroutine(_Logic.MoveConstantVelocity(this, new Vector2(pivotX, pivotY), 15));
                    yield return new WaitForFrames(30);

                    const float shakeOffset = 0.03f;
                    const int shakeInterval = 2;
                    for (int i = 0; i < 8; ++i)
                    {
                        _pos = new Vector2(pivotX + shakeOffset, pivotY);
                        yield return new WaitForFrames(shakeInterval);
                        _pos = new Vector2(pivotX, pivotY + shakeOffset);
                        yield return new WaitForFrames(shakeInterval);
                        _pos = new Vector2(pivotX - shakeOffset, pivotY);
                        yield return new WaitForFrames(shakeInterval);
                        _pos = new Vector2(pivotX, pivotY - shakeOffset);
                        yield return new WaitForFrames(shakeInterval);
                    }
                    _pos = new Vector2(pivotX, pivotY);
                }

                // 아야챠챠
                yield return new WaitForAbsFrames(5850);
                _coroutineManager.StartCoroutine(_Logic.CircleBullets(this, "Common/Bullet_Blue", 0.25f, 0.01f, 20, true, 60, 8));
                yield return new WaitForFrames(30);
                _coroutineManager.StartCoroutine(_Logic.CircleBullets(this, "Common/Bullet_Red", 0.25f, 0.02f, 20, false, 60, 8));

                // 야바린간
                yield return new WaitForAbsFrames(6330);
                _coroutineManager.StartCoroutine(_Logic.CircleBullets(this, "Common/Bullet_Blue", 0.25f, 0.01f, 20, true, 60, 8));
                _coroutineManager.StartCoroutine(_Logic.CornerAim(0.02f, 60, 2));

                // 아야챠챠
                yield return new WaitForAbsFrames(6840);
                _coroutineManager.StartCoroutine(_Logic.MoveDamp(this, Vector2.zero, 120, 0.05f));
                yield return new WaitForFrames(120);
                _coroutineManager.StartCoroutine(_Logic.CustomGapBullets(this, "Common/Bullet_Blue", 0.95f, 0.005f, 100
                    , 120, new float[] { 0.36f, 0.20f, 0.95f, 0.25f, 0.5f, 0.36f, 0.75f }));

                // 마지막 간주
                yield return new WaitForAbsFrames(7800);
                _coroutineManager.StartCoroutine(_Logic.MoveConstantVelocity(this, new Vector2(0.0f, 0.75f), 480));
                _coroutineManager.StartCoroutine(_Logic.SideAim(0, 0.02f, 60, 8));
                yield return new WaitForFrames(60 * 8);
                _coroutineManager.StartCoroutine(_Logic.SideAim(2, 0.02f, 120, 4));
                yield return new WaitForFrames(60);
                _coroutineManager.StartCoroutine(_Logic.SideAim(3, 0.02f, 120, 4));

                yield return new WaitForAbsFrames(8700);
                _Logic.CircleBullet(this, "Common/Bullet_Red", 0.25f, 0.025f, 20, false);
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

            private GameLogic _Logic
            {
                get { return GameSystem._Instance.GetLogic<GameLogic>(); }
            }
        } // Boss
    } // IevanPolkka
} // Game