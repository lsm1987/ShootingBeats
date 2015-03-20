using UnityEngine;
using System.Collections;

namespace Game
{
    namespace RainbowSocialism
    {
        // "무지개빛 사회주의" 진행 정보
        public class GameSystem : Game.GameSystem
        {
            // 노래 경로
            protected override string _SongPath
            {
                get { return "Sounds/RainbowSocialism"; }
                //get { return "Sounds/Test"; }
            }

            // 특화 정보 로딩
            protected override IEnumerator LoadContext()
            {
                // 플레이어기 로딩 /////////////////
                PoolStackShape("Common/Player_Black", 1);
                PoolStackMover<PlayerAlive>(1);
                PoolStackShape("Common/Player_Crash", 1);
                PoolStackMover<PlayerCrash>(1);
                yield return null;

                // 샷 로딩 ///////////////////////
                PoolStackShape("Common/Shot_Black", 20);
                PoolStackMover<Shot>(20);
                yield return null;

                // 적기 로딩 /////////////////////
                PoolStackShape("Common/Boss_Orange", 1);
                PoolStackMover<Boss>(1);
                yield return null;

                // 탄 로딩 ///////////////////
                // 외양 로딩
                PoolStackShape("Common/Bullet_Blue", 100);
                yield return null;
                PoolStackShape("Common/Bullet_Red", 100);
                yield return null;
                // 클래스 로딩
                PoolStackMover<Bullet>(100);
                yield return null;
            }

            // 특화 정보 갱신
            protected override void UpdatePlayContext()
            {
                if (_Frame == 0)
                {
                    // 플레이어 생성
                    PlayerAlive player = CreatePlayer<PlayerAlive>();
                    player.Init("Common/Player_Black", 0.0f, -0.7f, 0.0f);

                    // 보스 생성
                    Boss boss = CreateEnemy<Boss>();
                    boss.Init("Common/Boss_Orange", -0.5f, 1.3f, 0.0f);
                }
            }
        }
    }
}