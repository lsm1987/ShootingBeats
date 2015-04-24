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
            private const float _stageTextDefaultWidth = 500.0f;
            private const float _stageTextDefaultHeight = 60.0f;

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
                _uiStageText.SetPosition(0.0f, 0.0f);
                _uiStageText.SetSize(_stageTextDefaultWidth, _stageTextDefaultHeight);
                _uiStageText.SetAlign(TextAnchor.MiddleCenter);
                _uiStageText.SetText(string.Empty);
            }

            // 특화 정보 갱신
            public override void UpdatePlayContext()
            {
                int frame = GameSystem._Instance._Frame;
                switch(frame)
                {
                    case 0:
                        {
                            // 플레이어 생성
                            PlayerAlive player = GameSystem._Instance.CreatePlayer<PlayerAlive>();
                            player.Init("Common/Player_Black", 0.0f, -0.7f, 0.0f);
                            break;
                        }
                    case 300:
                        {
                            _uiStageText.SetPosition(0.0f, 0.0f);
                            _uiStageText.SetText("Shooting Beats! Tutorial");
                            _uiStageText.SetActive(true);
                            break;
                        }
                    case 540:
                        {
                            _uiStageText.SetText("with Ievan Polkka");
                            break;
                        }
                    case 780:
                        {
                            _uiStageText.SetText("Are you ready?");
                            break;
                        }
                    case 900:
                        {
                            _uiStageText.SetText("Let's Spin!");
                            break;
                        }
                    case 960:
                        {
                            _uiStageText.SetActive(false);
                            break;
                        }
                    case 996:
                        {
                            // 16초
                            // 아야챠챠
                            _uiStageText.SetPosition(0.3f, 0.0f);
                            _uiStageText.SetSize(_stageTextDefaultWidth * 0.6f, _stageTextDefaultHeight * 2.0f);
                            _uiStageText.SetAlign(TextAnchor.MiddleRight);
                            _uiStageText.SetText("Dodge bullets\nby touch and drag");
                            _uiStageText.SetActive(true);
                            break;
                        }
                    case 1230:
                        {
                            // 마바 리빠빠
                            _uiStageText.SetActive(false);
                            break;
                        }
                    case 1477:
                        {
                            // 24초
                            // 야바린간
                            _uiStageText.SetText("Entire rect colored now is\nmove touch area");
                            _uiStageText.SetActive(true);
                            break;
                        }
                    case 1726:
                        {
                            // 맀빠린단
                            _uiStageText.SetActive(false);
                            break;
                        }
                    case 1964:
                        {
                            // 32초
                            // 아야챠챠
                            _uiStageText.SetText("Only white circle\nof player is hit area");
                            _uiStageText.SetActive(true);
                            break;
                        }
                    case 2245:
                        {
                            // 마바 리빠빠
                            _uiStageText.SetActive(false);
                            break;
                        }
                    case 2444:
                        {
                            // 40초
                            // 야바린간
                            _uiStageText.SetText("Rect colored now is\npause touch area");
                            _uiStageText.SetActive(true);
                            break;
                        }
                    case 2693:
                        {
                            // 44초
                            // 맀빠린단
                            _uiStageText.SetText("Back button\ncan pause also");
                            break;
                        }
                    case 2940:
                        {
                            // 49초
                            _uiStageText.SetActive(false);
                            break;
                        }
                } // switch
            } // UpdatePlayContext
        }
    } // Ievan Polkka
} // Game