﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    // 게임 진행 관리
    // 진행 세부는 상속 클래스에서 지정
    public partial class GameSystem : SceneSystem
    {
        private static GameSystem _instance;
        public static GameSystem _Instance { get { return _instance; } }

        private const int _songFrameOverGap = 0;    // 노래 프레임이 게임 프레임을 지나쳤을 때 게임 프레임이 한 번에 따라갈 프레임 수
        private const int _gameFrameOverGap = 6;    // 게임 프레임이 노래 프레임을 지나쳤을 때 이 차이 이내면 스킵하지 않는다.
        public FSM _FSM { get; private set; }
        private ShapePoolManager _shapePoolManager = new ShapePoolManager();    // 외양 풀
        private MoverPoolManager _moverPoolManager = new MoverPoolManager();    // Mover 풀
        private const string _songRoot = "Sounds";  // 노래 파일 경로
        [SerializeField]
        private AudioSource _srcSong;    // 노래 재생할 소스
        public Player _Player { get; set; } // 활성화된 플레이어기
        public List<Player> _Players { get; private set; }    // 살아있는 플레이어기 목록
        public List<Shot> _Shots { get; private set; }    // 살아있는 샷(플레이어기가 발사) 목록
        public List<Enemy> _Enemys { get; private set; }    // 살아있는 적기 목록
        public List<Bullet> _Bullets { get; private set; }  // 살아있는 탄(적기가 발사) 목록
        public int _Frame { get; private set; }

        // UI 관련 //////////////////////////
        [SerializeField]
        private GameObject _letterBox;  // 레터박스 UI가 붙을 오브젝트
        [SerializeField]
        private MoveInputArea _moveInputArea;   // 이동 입력 영역
        public MoveInputArea _MoveInputArea { get { return _moveInputArea; } }

        // 음악별 설정 //////////////////////////
        private BeatInfo _beatInfo; // 현재 게임에서 사용할 음악 정보
        private GameLogic _logic;   // 음악별 다른 동작

        [SerializeField]
        private ScoreBoard _scoreBoard;
        private int _score = 0;
        private System.Random _random = null;   // 게임 내에서 사용할 랜덤. 시드값 항상 동일

        // 테스트용 설정 //////////////////////////
        public bool _isTestInvincible = false;  // 무적모드인가?
        public int _testStartFrame = -1;    // 몇 프레임부터 시작할 것인가?

        // 게임 옵션으로 지정 //////////////////////////////
        public float _PlayerSpeed { get { return 0.02f; } }
        public float _PlayerSlowSpeed { get { return 0.01f; } }

        // 자식 클래스에서 재정의 가능한 멤버변수 /////////////////////////////////
        // 게임 경계. 좌하단 min(-,-), 우상단 max(+,+). 짧은 쪽이 1
        // 3:4 -> 1:1.3
        public float _MaxX { get { return 1.0f; } }
        public float _MinX { get { return -1.0f; } }
        public float _MaxY { get { return 1.3f; } }
        public float _MinY { get { return -1.3f; } }

        /// <summary>
        ///  씬 진입 시
        /// </summary>
        protected override void OnAwake()
        {
            _instance = this;
            Define.SetFPS();
            InitBeatInfo();
            //_shapePoolManager.RecordMaxCreatedCount();
            _Players = new List<Player>();
            _Shots = new List<Shot>();
            _Enemys = new List<Enemy>();
            _Bullets = new List<Bullet>();
            SetScore(0);
            InitRandom();

            // 카메라
            InitializeCamera();

            // FSM 초기화
            _FSM = new FSM(this);
            _FSM.AddState(new LoadState(this));
            _FSM.AddState(new PlayState(this));

            // 로딩 시작
            _FSM.SetState(StateType.Load);
        }

        /// <summary>
        /// 씬 변경 시
        /// </summary>
        private void OnDestroy()
        {
            _instance = null;
            _shapePoolManager.LogCreatedCount();
        }

        // 고정 프레임 간격으로 갱신
        protected override void OnUpdate()
        {
            _FSM._CurrentState.OnUpdate();
        }

        private void InitBeatInfo()
        {
            _beatInfo = GlobalSystem._Instance._LoadingBeatInfo;
            _logic = Activator.CreateInstance(System.Type.GetType("Game." + _beatInfo._namespace + ".GameLogic")) as Game.GameLogic;
            if (_logic == null)
            {
                Debug.LogError("[GameSystem] Invalid namespcae:" + _beatInfo._namespace);
            }
        }

        #region Camera
        private void InitializeCamera()
        {
            // 기기 화면 비율
            int deviceW = Screen.width;
            int deviceH = Screen.height;
            float deviceR = (float)deviceW / (float)deviceH;    // 가로/세로

            // 게임 화면 비율
            float gameR = (_MaxX - _MinX) / (_MaxY - _MinY);
            //Debug.Log("deviceR:" + deviceR.ToString() + " gameR:" + gameR.ToString());

            // 메인 카메라를 게임 카메라로 사용
            Camera gameCam = Camera.main;
            
            // 게임 카메라 뷰포트 영역 지정
            if (gameR >= deviceR)
            {
                // 게임 화면 가로를 기기 가로에 가득 채움. 게임 영역은 화면 세로 상단
                // 기기W/기기H = 게임W/카메라H
                float camH = (_MaxX - _MinX) / deviceR;
                gameCam.orthographicSize = camH / 2.0f;

                float diffH = camH - (_MaxY - _MinY);
                Vector3 camPos = gameCam.transform.position;
                camPos.y = -1.0f * diffH / 2.0f;
                gameCam.transform.position = camPos;
                CreateLetterBox(true, diffH / camH);
            }
            else
            {
                // 게임 화면 세로를 기기 세로에 가득 채움. 게임 영역은 화면 가로 중단
                float camH = (_MaxY - _MinY);
                gameCam.orthographicSize = camH / 2.0f;

                // 기기W/기기H = 카메라W/게임H
                float camW = (_MaxY - _MinY) * deviceR;
                float diffW = camW - (_MaxX - _MinX);
                CreateLetterBox(false, diffW / camW);
            }
        }

        /// <summary>
        /// 레터박스 UI 추가
        /// </summary>
        /// <param name="horizontal">수평방향 레터박스인가?</param>
        /// <param name="screenRate">레터박스가 가릴 화면의 비율. 수직방향이면 절반씩 사용</param>
        public void CreateLetterBox(bool horizontal, float screenRate)
        {
            Color color = new Color(0.16f, 0.16f, 0.16f);
            if (horizontal)
            {
                GameObject obj = new GameObject();
                obj.name = "Horizontal";
                obj.transform.SetParent(_letterBox.transform, false);

                RectTransform rectTrans = obj.AddComponent<RectTransform>();
                rectTrans.anchorMin = Vector2.zero;
                rectTrans.anchorMax = new Vector2(1.0f, screenRate);
                rectTrans.offsetMin = Vector2.zero;
                rectTrans.offsetMax = Vector2.zero;
                rectTrans.pivot = new Vector2(0.5f, 0.5f);

                obj.AddComponent<CanvasRenderer>();
                Image image = obj.AddComponent<Image>();
                image.color = color;
            }
            else
            {
                // 0: 왼쪽, 1: 오른쪽
                for (int i = 0; i < 2; ++i)
                {
                    GameObject obj = new GameObject();
                    obj.name = "Vertical_" + i.ToString();
                    obj.transform.SetParent(_letterBox.transform, false);

                    RectTransform rectTrans = obj.AddComponent<RectTransform>();
                    if (i == 0)
                    {
                        rectTrans.anchorMin = Vector2.zero;
                        rectTrans.anchorMax = new Vector2(screenRate / 2.0f, 1.0f);
                    }
                    else
                    {
                        rectTrans.anchorMin = new Vector2(1.0f - screenRate / 2.0f, 0.0f);
                        rectTrans.anchorMax = Vector2.one;
                    }
                    rectTrans.offsetMin = Vector2.zero;
                    rectTrans.offsetMax = Vector2.zero;
                    rectTrans.pivot = new Vector2(0.5f, 0.5f);

                    obj.AddComponent<CanvasRenderer>();
                    Image image = obj.AddComponent<Image>();
                    image.color = color;
                }
            }
        }
        #endregion // Camera

        #region Loading
        public void StartLoading()
        {
            StartCoroutine(Loading());
        }

        // 로딩 전체 감싸기
        private IEnumerator Loading()
        {
            // 노래 로딩
            _srcSong.clip = Resources.Load<AudioClip>(_songRoot + "/" + _beatInfo._songFile);
            yield return null;

            // 특화 정보 로딩
            yield return StartCoroutine(_logic.LoadContext());

            // 여유시간
            yield return new WaitForSeconds(0.5f);
            // 로딩 끝
            _FSM.SetState(StateType.Play);
        }
        #endregion // Loading

        // 플레이 시작
        public void StartPlay()
        {
            // 노래 시작
            _srcSong.Play();
            if (_testStartFrame >= 0)
            {
                _srcSong.time = _testStartFrame / Define._fps;
            }

            // 진행 초기화
            _Frame = -1; // 첫 갱신 시 0프레임 되도록
        }

        // 엔진에서 호출하는 플레이 갱신
        public void UpdatePlay()
        {
            if (Input.GetButtonDown("Start") || Input.GetKeyDown(KeyCode.Escape))
            {
                Application.LoadLevel("BeatList");
                return;
            }

            // 프레임 갱신 하지 않을 것인가?
            bool skipUpdateFrame = false;

            // 노래 끝나면 isPlaying = false, time = 0, timeSamples = 0
            // time 은 timeSamples / clip.frequency 와 소수점 정밀도 제외하고는 동일함
            // 노래가 재생중일 때는 보정 발생할 수 있음
            if (_srcSong.isPlaying)
            {
                // 현재 노래 재생 시점을 프레임 단위로 변경
                int songFrame = (int)(_srcSong.time * (float)Define._fps);
                // 이번에 갱신할 게임 프레임
                int gameFrame = _Frame + 1;
                //Debug.Log("songFrame:" + songFrame.ToString() + " gameFrame:" + gameFrame.ToString());

                if (songFrame > gameFrame + _songFrameOverGap)
                {
                    //Debug.Log("[GameSystem] Song frame over occurred. Song:" + songFrame.ToString() + " Game:" + gameFrame.ToString());
                    // 기본 갱신 1회가 있으므로 -1회 따라잡음
                    while (songFrame > (_Frame + 1))
                    {
                        UpdatePlayFrame(true);
                    }
                    //Debug.Log("over occured!");
                }
                else if (songFrame + _gameFrameOverGap < gameFrame)
                {
                    //Debug.Log("[GameSystem] Game frame skip occurred. Song:" + songFrame.ToString() + " Game:" + gameFrame.ToString());
                    // 게임이 노래를 앞서나가면 갱신하지 않음
                    skipUpdateFrame = true;
                }
//                 else
//                 {
//                     Debug.Log("[GameSystem] Song:" + songFrame.ToString() + " Game:" + gameFrame.ToString() + " gap:" + (songFrame - gameFrame).ToString());
//                 }
            }

            if (!skipUpdateFrame)
            {
                UpdatePlayFrame(false);
            }
        }

        /// <summary>
        /// 한 프레임 갱신
        /// </summary>
        /// <param name="byFrameOver">노래 프레임 따라잡기용인가?</param>
        private void UpdatePlayFrame(bool byFrameOver)
        {
            // 프레임 갱신
            _Frame++;

            // 세부 갱신
            _logic.UpdatePlayContext();

            // 이동 물체들 갱신
            // 보통 플레이어기가 샷을 생성하고 적기가 탄을 생성하므로 이 순서로 갱신
            // Hit 체크도 업데이트 순서대로 수행
            UpdatePlayer(byFrameOver);
            UpdateShot(byFrameOver);
            UpdateEnemy(byFrameOver);
            UpdateBullet(byFrameOver);
        }

        #region Shape
        public void PoolStackShape(string subPath, int count)
        {
            _shapePoolManager.PoolStack(subPath, count);
        }

        public Shape CreateShape(string subPath)
        {
            return _shapePoolManager.Create(subPath);
        }

        public void DeleteShape(Shape shape)
        {
            _shapePoolManager.Delete(shape);
        }
        #endregion // Shape

        #region Mover
        /// <summary>
        /// 특정 타입의 Mover를 풀에 미리 생성한다.
        /// </summary>
        /// <typeparam name="T">Mover 타입</typeparam>
        /// <param name="count">미리 생성할 수</param>
        public void PoolStackMover<T>(int count) where T : Mover, new()
        {
            _moverPoolManager.PoolStack<T>(count);
        }

        private void UpdateMoverList<T>(List<T> movers, bool byFrameOver) where T : Mover
        {
            // 갱신 도중에 새 무버가 생길 수 있으므로 인덱스 순회
            for (int i = 0; i < movers.Count; ++i)
            {
                movers[i].Move();
            }

            // 역순 순회하며 삭제
            // 삭제 도중에 새 무버가 생기면 안됨
            for (int i = movers.Count - 1; i >= 0; --i)
            {
                if (!movers[i]._alive)
                {
                    T mover = movers[i];
                    mover.OnDestroy();
                    movers.RemoveAt(i);
                    _moverPoolManager.Delete(mover);
                }
            }

            // 그리기. 프레임 따라잡기용 업데이트에서는 그리지 않음
            if (!byFrameOver)
            {
                foreach (T mover in movers)
                {
                    mover.Draw();
                }
            }
        }

        #endregion //Mover

        #region Player
        public T CreatePlayer<T>() where T : Player, new()
        {
            T player = _moverPoolManager.Create<T>();
            _Players.Add(player);
            return player;
        }

        private void UpdatePlayer(bool byFrameOver)
        {
            UpdateMoverList(_Players, byFrameOver);
        }
        #endregion //Player

        #region Shot
        /// <summary>
        /// 풀에서 샷을 생성하고, 샷 업데이트 목록의 가장 뒤에 추가한다.
        /// <para>생성된 탄을 리턴</para>
        /// </summary>
        public T CreateShot<T>() where T : Shot, new()
        {
            T shot = _moverPoolManager.Create<T>();
            _Shots.Add(shot);
            return shot;
        }

        private void UpdateShot(bool byFrameOver)
        {
            UpdateMoverList(_Shots, byFrameOver);
        }
        #endregion //Shot

        #region Enemy
        /// <summary>
        /// 풀에서 적기를 생성하고, 적기 업데이트 목록의 가장 뒤에 추가한다.
        /// <para>생성된 적기를 리턴</para>
        /// </summary>
        public T CreateEnemy<T>() where T : Enemy, new()
        {
            T enemy = _moverPoolManager.Create<T>();
            _Enemys.Add(enemy);
            return enemy;
        }

        /// <summary>
        /// 적기 목록 순회하며 갱신
        /// </summary>
        private void UpdateEnemy(bool byFrameOver)
        {
            UpdateMoverList(_Enemys, byFrameOver);
        }
        #endregion //Enemy

        #region Bullet
        /// <summary>
        /// 풀에서 탄을 생성하고, 탄 업데이트 목록의 가장 뒤에 추가한다.
        /// <para>생성된 탄을 리턴</para>
        /// </summary>
        public T CreateBullet<T>() where T : Bullet, new()
        {
            T bullet = _moverPoolManager.Create<T>();
            _Bullets.Add(bullet);
            return bullet;
        }

        /// <summary>
        /// 탄 목록 순회하며 갱신
        /// </summary>
        private void UpdateBullet(bool byFrameOver)
        {
            // 특수 처리가 있어 공용 무버 갱신함수 사용하지 않음
            for (int i = 0; i < _Bullets.Count; ++i)
            {
                _Bullets[i].Move();
            }

            for (int i = _Bullets.Count - 1; i >= 0; --i)
            {
                if (!_Bullets[i]._alive)
                {
                    Bullet bullet = _Bullets[i];
                    bullet.OnDestroy();
                    _Bullets.RemoveAt(i);
                    _moverPoolManager.Delete(bullet);
                }
            }

            if (!byFrameOver)
            {
                for (int i = 0; i < _Bullets.Count; ++i)
                {
                    // 뒤에 추가된 탄일수록 위에 그림
                    _Bullets[i].Draw(i);
                }
            }
        }
        #endregion // Bullet

        #region Score
        /// <summary>
        /// 점수 지정
        /// </summary>
        private void SetScore(int score)
        {
            _score = score;
            _scoreBoard.SetScore(_score);
        }

        /// <summary>
        /// 점수 변화량 지정
        /// </summary>
        public void SetScoreDelta(int delta)
        {
            SetScore(_score + delta);
        }
        #endregion // Score

        #region Random
        private void InitRandom()
        {
            // 항상 동일한 시드값으로 랜덤 초기화
            _random = new System.Random(1);
        }

        /// <summary>
        /// 0~1 사이의 무작위 실수 구함
        /// </summary>
        /// <returns>0~1 사이 실수</returns>
        public float GetRandom01()
        {
            return (float)_random.NextDouble();
        }
        #endregion // Random

        #region Debug
        private void OnDrawGizmos()
        {
            // 경계
            {
                Vector2 lt = new Vector2(_MinX, _MaxY);
                Vector2 lb = new Vector2(_MinX, _MinY);
                Vector2 rb = new Vector2(_MaxX, _MinY);
                Vector2 rt = new Vector2(_MaxX, _MaxY);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(lt, lb);
                Gizmos.DrawLine(lb, rb);
                Gizmos.DrawLine(rb, rt);
                Gizmos.DrawLine(rt, lt);
            }
        }
        #endregion // Debug
    }
}