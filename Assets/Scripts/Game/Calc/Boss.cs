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

            private GameLogic _Logic
            {
                get { return GameSystem._Instance.GetLogic<GameLogic>(); }
            }
        } // Boss
    } // Calc
} // Game