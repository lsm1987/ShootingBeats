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
                _coroutineManager.RegisterCoroutine(_Logic.MoveConstantVelocity(this, new Vector2(0.0f, 0.75f), 120));
            }

            public override void Move()
            {
                MoveMain();
                _coroutineManager.UpdateAllCoroutines();
            }

            // 실제 갱신
            private void MoveMain()
            {
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