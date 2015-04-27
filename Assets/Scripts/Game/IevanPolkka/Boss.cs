﻿using UnityEngine;
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
                MoveMain();
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
                        _coroutineManager.StartCoroutine(_Logic.MoveConstantVelocity(this, new Vector2(pivotX + shakeOffset, pivotY), shakeInterval));
                        yield return new WaitForFrames(shakeInterval);
                        _coroutineManager.StartCoroutine(_Logic.MoveConstantVelocity(this, new Vector2(pivotX, pivotY + shakeOffset), shakeInterval));
                        yield return new WaitForFrames(shakeInterval);
                        _coroutineManager.StartCoroutine(_Logic.MoveConstantVelocity(this, new Vector2(pivotX - shakeOffset, pivotY), shakeInterval));
                        yield return new WaitForFrames(shakeInterval);
                        _coroutineManager.StartCoroutine(_Logic.MoveConstantVelocity(this, new Vector2(pivotX, pivotY - shakeOffset), shakeInterval));
                        yield return new WaitForFrames(shakeInterval);
                    }
                    _Pos = new Vector2(pivotX, pivotY);
                }

                // 아야챠챠
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