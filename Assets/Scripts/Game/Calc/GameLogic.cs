using UnityEngine;
using System.Collections;

namespace Game
{
    namespace Calc
    {
        // "Calc." 진행정보
        public class GameLogic : Game.BaseGameLogic
        {
            private CoroutineManager _coroutineManager = new CoroutineManager();

            // 특화 정보 로딩
            public override IEnumerator LoadContext()
            {
                IEnumerator loadPlayer = LoadBasicPlayer();
                while (loadPlayer.MoveNext())
                {
                    yield return loadPlayer.Current;
                }

                // 적기 로딩 /////////////////////
                GameSystem._Instance.PoolStackShape("Common/Boss_Miku", 1);
                GameSystem._Instance.PoolStackMover<Boss>(1);
                GameSystem._Instance.PoolStackShape("Common/Effect_BossCrashMiku", 1);
                GameSystem._Instance.PoolStackMover<Effect>(1);

                // 탄 로딩 ///////////////////
                // 외양 로딩
                GameSystem._Instance._UILoading.SetProgress("Loading Bullets");
                yield return null;
                GameSystem._Instance.PoolStackShape("Common/Bullet_Blue", 270);
                GameSystem._Instance.PoolStackShape("Common/Bullet_Red", 27);
                
                // 클래스 로딩
                GameSystem._Instance.PoolStackMover<Bullet>(270);

                // 코루틴
                _coroutineManager.StopAllCoroutines();
                _coroutineManager.RegisterCoroutine(Main());    // 메인 코루틴 등록
            }

            // 특화 정보 갱신
            public override void UpdatePlayContext()
            {
                _coroutineManager.UpdateAllCoroutines();
            }

            private IEnumerator Main()
            {
                // 플레이어 생성
                PlayerAlive player = GameSystem._Instance.CreatePlayer<PlayerAlive>();
                player.Init("Common/Player_Black", 0.0f, -0.7f, 0.0f);

                // 보스 생성
                Boss boss = GameSystem._Instance.CreateEnemy<Boss>();
                boss.Init("Common/Boss_Miku", -0.5f, 1.3f, 0.0f);

                yield return null;
            }

            #region Coroutine
            #endregion Coroutine
        } // GameLogic
    } // Calc
} // Game