using UnityEngine;
using System.Collections;

namespace Game
{
    namespace TurkishMarch
    {
        // "터키 행진곡" 진행정보
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

                yield return null;
            }
        } // GameLogic
    } // TurkishMarch
} // Game