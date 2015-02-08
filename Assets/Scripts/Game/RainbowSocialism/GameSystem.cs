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
            }

            // 특화 정보 로딩
            protected override IEnumerator LoadContext()
            {
                // 외양 로딩
                PoolStackShape("Common/DpBlueBulletC", 50);
                yield return null;
                PoolStackShape("Common/DpRedBulletC", 50);
                yield return null;

                // 탄 로딩
                PoolStackBullet<Bullet>(100);
                yield return null;
            }
        }
    }
}