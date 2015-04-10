using UnityEngine;
using System.Collections;

namespace Game
{
    namespace RainbowSocialism
    {
        // "무지개빛 사회주의" 진행 정보
        public class GameLogic : Game.GameLogic
        {
            // 특화 정보 로딩
            public override IEnumerator LoadContext()
            {
                // 플레이어기 로딩 /////////////////
                GameSystem._Instance._UILoading.SetProgress("Loading Player");
                GameSystem._Instance.PoolStackShape("Common/Player_Black", 1);
                GameSystem._Instance.PoolStackMover<PlayerAlive>(1);
                GameSystem._Instance.PoolStackShape("Common/Player_Crash", 1);
                GameSystem._Instance.PoolStackMover<PlayerCrash>(1);
                yield return null;

                // 샷 로딩 ///////////////////////
                GameSystem._Instance._UILoading.SetProgress("Loading Shots");
                GameSystem._Instance.PoolStackShape("Common/Shot_Black", 20);
                GameSystem._Instance.PoolStackMover<Shot>(20);
                yield return null;

                // 적기 로딩 /////////////////////
                GameSystem._Instance._UILoading.SetProgress("Loading Boss Shape");
                GameSystem._Instance.PoolStackShape("Common/Boss_Orange", 1);
                yield return null;
                GameSystem._Instance._UILoading.SetProgress("Loading Boss Class");
                GameSystem._Instance.PoolStackMover<Boss>(1);
                yield return null;

                // 탄 로딩 ///////////////////
                // 외양 로딩
                GameSystem._Instance._UILoading.SetProgress("Loading Bullets 1");
                GameSystem._Instance.PoolStackShape("Common/Bullet_Blue", 210);
                yield return null;
                GameSystem._Instance._UILoading.SetProgress("Loading Bullets 2");
                GameSystem._Instance.PoolStackShape("Common/Bullet_Red", 125);
                yield return null;
                // 클래스 로딩
                GameSystem._Instance._UILoading.SetProgress("Loading Bullet Classes");
                GameSystem._Instance.PoolStackMover<Bullet>(100);
                yield return null;
            }

            // 특화 정보 갱신
            public override void UpdatePlayContext()
            {
                if (GameSystem._Instance._Frame == 0)
                {
                    // 플레이어 생성
                    PlayerAlive player = GameSystem._Instance.CreatePlayer<PlayerAlive>();
                    player.Init("Common/Player_Black", 0.0f, -0.7f, 0.0f);

                    // 보스 생성
                    Boss boss = GameSystem._Instance.CreateEnemy<Boss>();
                    boss.Init("Common/Boss_Orange", -0.5f, 1.3f, 0.0f);
                }
            }
        }
    }
}