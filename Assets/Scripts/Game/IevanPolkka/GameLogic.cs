using UnityEngine;
using System.Collections;

namespace Game
{
    namespace IevanPolkka
    {
        // "이에반 폴카" 진행정보
        public class GameLogic : Game.BaseGameLogic
        {
            private UIStageText _uiStageText = null;    // 스테이지 내 UI로 띄워줄 텍스트. 하나만 사용

            // 특화 정보 로딩
            public override IEnumerator LoadContext()
            {
                IEnumerator loadPlayer = LoadBasicPlayer();
                while (loadPlayer.MoveNext())
                {
                    yield return loadPlayer.Current;
                }

                // 스테이지 텍스트
                if (_uiStageText == null)
                {
                    Object prefab = Resources.Load(Define._uiStageText);
                    GameObject obj = Object.Instantiate(prefab) as GameObject;
                    obj.name = prefab.name;
                    _uiStageText = obj.GetComponent<UIStageText>();
                    _uiStageText.Initialize(GameSystem._Instance._GameArea.transform);
                }
                _uiStageText.SetActive(false);
            }

            // 특화 정보 갱신
            public override void UpdatePlayContext()
            {
                if (GameSystem._Instance._Frame == 0)
                {
                    // 플레이어 생성
                    PlayerAlive player = GameSystem._Instance.CreatePlayer<PlayerAlive>();
                    player.Init("Common/Player_Black", 0.0f, -0.7f, 0.0f);

                    // 스테이지 텍스트 테스트
                    _uiStageText.Set(0.5f, -0.7f, 500.0f, 60.0f, "1111111");
                    _uiStageText.SetActive(true);
                }
            }
        }
    } // Ievan Polkka
} // Game