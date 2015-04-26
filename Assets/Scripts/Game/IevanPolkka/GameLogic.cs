﻿using UnityEngine;
using System.Collections;

namespace Game
{
    namespace IevanPolkka
    {
        // "이에반 폴카" 진행정보
        public class GameLogic : Game.BaseGameLogic
        {
            private CoroutineManager _coroutineManager = new CoroutineManager();
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

                // 코루틴
                _coroutineManager.StopAllCoroutines();

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
                _uiStageText.SetAlign(TextAnchor.MiddleCenter);
                _uiStageText.SetAnchorPoistion(0.5f, 0.5f);
                _uiStageText.SetSize(_stageTextDefaultWidth, _stageTextDefaultHeight);
                _uiStageText.SetText(string.Empty);

                // UI
                GameSystem._Instance._MoveInputArea.SetVisible(false);
                GameSystem._Instance._PauseInputArea.SetVisible(false);
            }

            // 특화 정보 갱신
            public override void UpdatePlayContext()
            {
                int frame = GameSystem._Instance._Frame;
                switch(frame)
                {
                    case 0:
                        {
                            _coroutineManager.StartCoroutine(Main());
                            break;
                        }
                } // switch

                // 매프레임 갱신
                _coroutineManager.UpdateAllCoroutines();
            } // UpdatePlayContext()

            private IEnumerator Main()
            {
                // 플레이어 생성
                PlayerAlive player = GameSystem._Instance.CreatePlayer<PlayerAlive>();
                player.Init("Common/Player_Black", 0.0f, -0.7f, 0.0f);

                yield return new WaitForAbsFrames(300);
                _uiStageText.SetAlign(TextAnchor.MiddleCenter);
                _uiStageText.SetAnchorPoistion(0.5f, 0.5f);
                _uiStageText.SetText("Shooting Beats! Tutorial");
                _uiStageText.SetActive(true);

                yield return new WaitForAbsFrames(540);
                _uiStageText.SetText("with Ievan Polkka");

                yield return new WaitForAbsFrames(700);
                _uiStageText.SetText("Are you ready?");

                yield return new WaitForAbsFrames(900);
                _uiStageText.SetText("Let's start!");

                yield return new WaitForAbsFrames(960);
                _uiStageText.SetActive(false);

                yield return new WaitForAbsFrames(1026);
                // 아야챠챠
                _uiStageText.SetAlign(TextAnchor.MiddleRight);
                _uiStageText.SetAnchorPoistion(0.9f, 0.5f);
                _uiStageText.SetSize(_stageTextDefaultWidth * 0.6f, _stageTextDefaultHeight * 2.0f);
                _uiStageText.SetText("Dodge bullets\nby touch and drag");
                _uiStageText.SetActive(true);
                _coroutineManager.StartCoroutine(SideAim(0, 0.02f, 60, 16));

                // 마바 리빠빠

                yield return new WaitForFrames(60 * 4 * 2);
                // 야바린간
                _uiStageText.SetText("Colored rect is\nmove touch area");
                GameSystem._Instance._MoveInputArea.SetVisible(true);

                yield return new WaitForFrames(60 * 4);
                // 맀빠린단
                GameSystem._Instance._MoveInputArea.SetVisible(false);

                yield return new WaitForAbsFrames(1986);
                // 아야챠챠
                _uiStageText.SetText("Colored rect is\npause touch area");
                GameSystem._Instance._PauseInputArea.SetVisible(true);
                _coroutineManager.StartCoroutine(SideAim(2, 0.02f, 60, 8));

                yield return new WaitForFrames(60 * 4);
                // 마바 리빠빠
                GameSystem._Instance._PauseInputArea.SetVisible(false);

                yield return new WaitForFrames(60 * 4);
                // 야바린간
                _uiStageText.SetText("Back button\nalso can pause");
                _coroutineManager.StartCoroutine(SideAim(3, 0.02f, 60, 8));

                // 맀빠린단

                yield return new WaitForAbsFrames(2946);
                // 간주
                _uiStageText.SetActive(false);
                _coroutineManager.StartCoroutine(CornerAim(0.02f, 60, 4));

                yield return new WaitForAbsFrames(3906);
                // YO!
                _uiStageText.SetText("Each beat has\nthe Beat Core");
                _uiStageText.SetActive(true);

                yield return new WaitForAbsFrames(4380);
                // 간주(야바린간)
                _uiStageText.SetText("Shooting Beat Core\nwill increase score");

                yield return new WaitForAbsFrames(4860);
                // 아야챠챠
                _uiStageText.SetAlign(TextAnchor.MiddleCenter);
                _uiStageText.SetAnchorPoistion(0.5f, 0.5f);
                _uiStageText.SetText("Explanation is over");

                yield return new WaitForAbsFrames(5346);
                // 야바린간
                _uiStageText.SetText("Enjoy your shooting");

                yield return new WaitForAbsFrames(5580);
                // 마마마 린간 덴간 린간 덴간
                _uiStageText.SetText("Let's~");

                yield return new WaitForAbsFrames(5730);
                // 린간 린간
                _uiStageText.SetText("Spin!");
                yield return new WaitForFrames(30);
                _uiStageText.SetText("Spin!!!");
                yield return new WaitForFrames(30);
                _uiStageText.SetText("Spin!!!!!!");
                yield return new WaitForFrames(30);
                _uiStageText.SetText("<b>Spin!!!!!!!!!</b>");
                yield return new WaitForFrames(30);
                _uiStageText.SetActive(false);
            }

            #region Coroutine
            /// <summary>
            /// 사면에서 직각방향으로 발사되는 시작위치 조준탄
            /// </summary>
            /// <param name="dir">시작방향. 0:상, 1:하, 2:좌, 3: 우</param>
            private IEnumerator SideAim(int dir, float speed, int interval, int count)
            {
                for (int i = 0; i < count; ++i)
                {
                    float x, y, angle;
                    if (dir == 0)
                    {
                        x = GameSystem._Instance._Player._x;
                        y = GameSystem._Instance._MaxY;
                        angle = 0.75f;
                    }
                    else if (dir == 1)
                    {
                        x = GameSystem._Instance._Player._x;
                        y = GameSystem._Instance._MinY;
                        angle = 0.25f;
                    }
                    else if (dir == 2)
                    {
                        x = GameSystem._Instance._MinX;
                        y = GameSystem._Instance._Player._y;
                        angle = 0.0f;
                    }
                    else
                    {
                        x = GameSystem._Instance._MaxX;
                        y = GameSystem._Instance._Player._y;
                        angle = 0.50f;
                    }

                    Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Blue", x, y, angle, 0.0f, speed, 0.0f);

                    if (i < count - 1)
                    {
                        yield return new WaitForFrames(interval);
                    }
                }
            }

            /// <summary>
            /// 모서리에서 플레이어 방향으로 발사되는 조준탄
            /// </summary>
            /// <param name="interval">탄별 간격</interval>
            /// <param name="roundCount">4모서리 순회를 몇 번 할 것인가</param>
            private IEnumerator CornerAim(float speed, int interval, int roundCount)
            {
                for (int i = 0; i < roundCount; ++i)
                {
                    // 0: 우상, 1: 좌상, 2: 좌하, 3: 우하
                    for (int dir = 0; dir < 4; ++dir)
                    {
                        float x, y;
                        if (dir == 0)
                        {
                            x = GameSystem._Instance._MaxX;
                            y = GameSystem._Instance._MaxY;
                        }
                        else if (dir == 1)
                        {
                            x = GameSystem._Instance._MinX;
                            y = GameSystem._Instance._MaxY;
                        }
                        else if (dir == 2)
                        {
                            x = GameSystem._Instance._MinX;
                            y = GameSystem._Instance._MinY;
                        }
                        else
                        {
                            x = GameSystem._Instance._MaxX;
                            y = GameSystem._Instance._MinY;
                        }

                        float angle = GetPlayerAngle(x, y);
                        Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                        b.Init("Common/Bullet_Red", x, y, angle, 0.0f, speed, 0.0f);

                        if (!((i == (roundCount - 1) && (dir == (4 - 1)))))
                        {
                            yield return new WaitForFrames(interval);
                        }
                    }
                }
            }
            #endregion Coroutine
        } // GameLogic
    } // Ievan Polkka
} // Game