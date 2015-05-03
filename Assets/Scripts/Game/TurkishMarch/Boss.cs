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
                _coroutineManager.StartCoroutine(PatternA());
                yield return new WaitForAbsFrames(511);
                _coroutineManager.StartCoroutine(PatternA());
                yield return new WaitForAbsFrames(932);
                _coroutineManager.StartCoroutine(PatternB());
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
            public IEnumerator PatternA()
            {
                _coroutineManager.StartCoroutine(_Logic.AimingLineBullets(this, "Common/Bullet_Red", 0.02f, 3, 5));
                yield return new WaitForFrames(50);
                _coroutineManager.StartCoroutine(_Logic.AimingLineBullets(this, "Common/Bullet_Red", 0.02f, 3, 5));
                yield return new WaitForFrames(50);
                _coroutineManager.StartCoroutine(_Logic.AimingNWayLineBullets(this, "Common/Bullet_Red", 0.02f, 3, 4, 0.125f, 3));
                yield return new WaitForFrames(25);
                _coroutineManager.StartCoroutine(_Logic.AimingNWayLineBullets(this, "Common/Bullet_Red", 0.02f, 3, 9, 0.125f, 3));
                
                yield return new WaitForFrames(100);
                const int circleInterval = 28;
                _coroutineManager.StartCoroutine(_Logic.CircleBullets(this, "Common/Bullet_Blue", 0.25f, 0.02f, 12, true, circleInterval, 7));
                yield return new WaitForFrames(circleInterval / 2);
                _coroutineManager.StartCoroutine(_Logic.CircleBullets(this, "Common/Bullet_Blue", 0.25f, 0.02f, 12, false, circleInterval, 6));
            }

            public IEnumerator PatternB()
            {
                _Logic.RandomSpreadBullet(this, "Common/Bullet_Red", 0.2f, 0.02f, 0.02f, 30);
                yield return new WaitForFrames(120);
                _Logic.CircleBullet(this, "Common/Bullet_Blue", 0.25f, 0.02f, 12, true);
                yield return new WaitForFrames(120);
                _Logic.RandomSpreadBullet(this, "Common/Bullet_Red", 0.2f, 0.02f, 0.02f, 30);
                yield return new WaitForFrames(120);
                _Logic.CircleBullet(this, "Common/Bullet_Blue", 0.25f, 0.02f, 12, true);
            }
            #endregion //Coroutine

            private GameLogic _Logic
            {
                get { return GameSystem._Instance.GetLogic<GameLogic>(); }
            }
        } // Boss
    } // TurkishMarch
} // Game