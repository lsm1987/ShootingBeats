using UnityEngine;
using System.Collections;

namespace Game
{
    namespace IevanPolkka
    {
        // "이에반 폴카" 진행정보
        public class GameLogic : Game.BaseGameLogic
        {
            // 특화 정보 로딩
            public override IEnumerator LoadContext()
            {
                IEnumerator loadPlayer = LoadBasicPlayer();
                while (loadPlayer.MoveNext())
                {
                    yield return loadPlayer.Current;
                }
            }

            // 특화 정보 갱신
            public override void UpdatePlayContext()
            {
                if (GameSystem._Instance._Frame == 0)
                {
                    // 플레이어 생성
                    PlayerAlive player = GameSystem._Instance.CreatePlayer<PlayerAlive>();
                    player.Init("Common/Player_Black", 0.0f, -0.7f, 0.0f);
                }
            }
        }
    }
}