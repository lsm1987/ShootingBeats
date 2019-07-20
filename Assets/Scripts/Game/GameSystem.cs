using UnityEngine;
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

        private const int _songFrameOverTolerance = 0;  // 노래 프레임이 게임 프레임을 지나쳐 보정이 필요하다고 판단하기까지 여유 프레임 수
                                                        // (예: 0 -> 게임을 10프레임 업데이트 할 차례인데 노래가 11프레임이면 11 > 10 + 0 -> 보정 필요)
        private const int _additionalBySongFrameOver = 1;  // 노래 게임이 게임 프레임을 지나쳤을 때,
                                                                    // 노래를 따라잡기위해 게임을 최대 몇 프레임이나 추가로 업데이트 할 것인가
        private const int _songFrameOverToMuch = 60;    // 노래 프레임이 너무 지나쳐 한 번에 다 따라잡아야 하는 경우의 기준
        private const int _gameFrameOverGap = 6;    // 게임 프레임이 노래 프레임을 지나쳤을 때 이 차이 이내면 스킵하지 않는다.
        
        private enum StateType   // 진행상태 타입
        {
            Invalid,    // 최초 무효상태
            Load,
            Play,
            Result, // 결과창
        }
        private StateType _stateType = StateType.Invalid;   // 현재 진행상태
        private bool _cameraInitialized = false;    // 카메라 초기화되었는가?
        private ShapePoolManager _shapePoolManager = new ShapePoolManager();    // 외양 풀
        private MoverPoolManager _moverPoolManager = new MoverPoolManager();    // Mover 풀
        private const string _songRoot = "Sounds";  // 노래 파일 경로
        [SerializeField]
        private AudioSource _srcSong = null;    // 노래 재생할 소스
        [SerializeField]
        private AudioSource _srcEffect = null; // 사운드 이펙트 재생할 소스
        public Player _Player { get; set; } // 활성화된 플레이어기
        public List<Player> _Players { get; private set; }    // 살아있는 플레이어기 목록
        public List<Shot> _Shots { get; private set; }    // 살아있는 샷(플레이어기가 발사) 목록
        public List<Enemy> _Enemys { get; private set; }    // 살아있는 적기 목록
        public List<Bullet> _Bullets { get; private set; }  // 살아있는 탄(적기가 발사) 목록
        public List<Effect> _Effects { get; private set; }
        public int _Frame { get; private set; } // 현재 갱신중인 프레임 번호
        public bool _FrameByOver { get; private set; }  // 지나쳐버린 노래 따라잡기 위한 프레임 갱신인가?
        private int _totalFrame = 0; // 총 몇 프레임짜리 스테이지인가?

        // UI 관련 //////////////////////////
        [SerializeField]
        private UILoading _uiLoading = null;   // 로딩 UI
        public UILoading _UILoading { get { return _uiLoading; } }
        [SerializeField]
        private UIProgressBar _uiProgressBar = null;
        [SerializeField]
        private Layout _layout = null;
        private Layout _Layout { get { return _layout; } }
        public RectTransform _LayoutGameArea { get { return _Layout._GameArea; } }
        [SerializeField]
        private MoveInputArea _moveInputArea = null;   // 이동 입력 영역
        public MoveInputArea _MoveInputArea { get { return _moveInputArea; } }
        [SerializeField]
        private PauseInputArea _pauseInputArea = null;   // 일시정지 입력 영역
        public PauseInputArea _PauseInputArea { get { return _pauseInputArea; } }
        [SerializeField]
        private UIPause _uiPause = null;   // 일시정지 UI
        private UIPause _UIPause { get { return _uiPause; } }
        [SerializeField]
        private UIResult _uiResult = null; // 결과 UI
        private UIResult _UIResult { get { return _uiResult; } }

        // 음악별 설정 //////////////////////////
        private BeatInfo _beatInfo; // 현재 게임에서 사용할 음악 정보
        private BaseGameLogic _logic;   // 음악별 다른 동작

        [SerializeField]
        private ScoreBoard _scoreBoard = null;
        private int _score = 0;
        private System.Random _random = null;   // 게임 내에서 사용할 랜덤. 시드값 항상 동일
        private bool _isPaused = false;     // 일시정지 중인가?
        private float _timeScaleBeforePause = 1.0f; // 일시정지 전 타임스케일
        private bool _isGameOvered = false; // 게임오버 판정이 발생했는가?
        private int _gameOveredFrame = 0;   // 게임오버 발생한 프레임
        private const int _gameOverResultDelay = (int)(Define._fps * 2.0f);    // 게임오버 발생 후 결과UI 나오기까지 지연 프레임

        // 테스트용 설정 //////////////////////////
        public TestInfo _TestInfo { get; private set; }

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
        private float _Width { get { return (_MaxX - _MinX); } }
        private float _Height { get { return (_MaxY - _MinY); } }

        // 기준으로 삼는 화면비
        private const float _refDeviceWidthRatio = 9.0f;
        private const float _refDeviceHeightRatio = 16.0f;

        private const float _topBoxMinHeightScreenRate = 0.05f;  // 상단 박스의 화면 내 최소 세로 비중

        /// <summary>
        ///  씬 진입 시 한번 실행
        /// </summary>
        protected override void OnAwake()
        {
            _instance = this;
            Define.SetFPS();
            if (GlobalSystem._Instance == null)
            {
                GlobalSystem.CreateInstance();
            }
            //_shapePoolManager.RecordMaxCreatedCount();
            //_moverPoolManager.RecordMaxCreatedCount();
            _Players = new List<Player>();
            _Shots = new List<Shot>();
            _Enemys = new List<Enemy>();
            _Bullets = new List<Bullet>();
            _Effects = new List<Effect>();

            StartLoading();
        }

        /// <summary>
        /// 씬 변경 시
        /// </summary>
        private void OnDestroy()
        {
            _instance = null;
            _shapePoolManager.LogCreatedCount();
            _moverPoolManager.LogCreatedCount();
            if (_isPaused)
            {
                Time.timeScale = _timeScaleBeforePause;
            }
        }

        // 고정 프레임 간격으로 갱신
        protected override void OnUpdate()
        {
            switch(_stateType)
            {
                case StateType.Play:
                    {
                        UpdatePlay();
                        break;
                    }
            }
        }

        /// <summary>
        /// 상태 변경
        /// </summary>
        /// <param name="nextState"></param>
        private void SetState(StateType nextState)
        {
            // 현재 상태 새로 지정
            _stateType = nextState;
        }

        #region Loading
        /// <summary>
        /// 로딩 시작
        /// </summary>
        private void StartLoading()
        {
            StartCoroutine(Loading());
        }

        /// <summary>
        /// 로딩 전체 감싸기
        /// </summary>
        /// <returns></returns>
        private IEnumerator Loading()
        {
            // 상태 변경
            SetState(StateType.Load);

            // 로딩 UI
            _UILoading.Open();

            // 구성요소 초기화
            InitTestInfo();
            InitBeatInfo();
            SetScore(0);
            InitRandom();
            InitCamera();
            InitGameOvered();
            _uiProgressBar.SetRate(0.0f);

            // 노래 로딩
            if (_srcSong.clip == null)
            {
                _srcSong.clip = Resources.Load<AudioClip>(_songRoot + "/" + _beatInfo._namespace);
                _totalFrame = (int)(_srcSong.clip.length * (float)Define._fps);
                yield return null;
            }
            else
            {
                _srcSong.Stop();
            }

            // 특화 정보 로딩
            RemoveAllMover();
            yield return StartCoroutine(_logic.LoadContext());

            // 여유 프레임
            _UILoading.Close();
            yield return null;
            
            // 로딩 끝
            StartPlay();
        }

        private void InitTestInfo()
        {
            if (_TestInfo == null)
            {
                _TestInfo = GetComponent<TestInfo>();
            }
        }

        private void InitBeatInfo()
        {
            if (_beatInfo == null)
            {
                _beatInfo = GlobalSystem._Instance._LoadingBeatInfo;
                if (_beatInfo == null && _TestInfo != null)
                {
                    // 목록으로부터 음악이 선택되지 않았다면 테스트용 정보 이용
                    _beatInfo = _TestInfo._BeatInfo;
                }
                if (_beatInfo == null)
                {
                    Debug.LogError("[GameSystem] Invalid BeatInfo");
                }
            }

            if (_logic == null)
            {
                _logic = Activator.CreateInstance(System.Type.GetType("Game." + _beatInfo._namespace + ".GameLogic")) as Game.BaseGameLogic;
                if (_logic == null)
                {
                    Debug.LogError("[GameSystem] Invalid namespcae:" + _beatInfo._namespace);
                }
            }
        }

        private void InitCamera()
        {
            if (_cameraInitialized)
            {
                // 이미 현재 노래에 대해 초기화 되었음
                return;
            }

            // 종횡비 = 가로 / 세로
            float refDeviceAspect = _refDeviceWidthRatio / _refDeviceHeightRatio;
            float screenAspect = (float)Screen.width / Screen.height;
            float layoutAspect = Mathf.Min(refDeviceAspect, screenAspect);  // 세로가 더 긴쪽을 레이아웃 종횡비로 사용

            float gameHeightLayoutRate = layoutAspect * (_Height / _Width); // 레이아웃 가로가 가득 차도록 게임화면을 배치했을 때 세로 비중
            float topBoxHeightLayoutRate = _topBoxMinHeightScreenRate + (UIUtil.SafeArea.y / Screen.height);
            float bottomBoxHeightLayoutRate = 1.0f - gameHeightLayoutRate - topBoxHeightLayoutRate;

            // 레터박스는 기준 화면비보다 가로가 길 때 발생
            float centerBoxWidthRate = (screenAspect > refDeviceAspect) ? refDeviceAspect / screenAspect :  1.0f;

            _Layout.Initialize(centerBoxWidthRate, topBoxHeightLayoutRate, bottomBoxHeightLayoutRate);

            // 메인 카메라를 게임 카메라로 사용
            Camera gameCam = Camera.main;

            // 게임월드높이와 상하 레터박스 월드높이의 합을 camH라고 할 때
            // camH : _Height = 1 : gameHeightLayoutRate
            float camH = (1f / gameHeightLayoutRate) * _Height;
            gameCam.orthographicSize = camH / 2.0f;

            float diffH = camH * (bottomBoxHeightLayoutRate - topBoxHeightLayoutRate);
            Vector3 camPos = gameCam.transform.position;
            camPos.y = -1.0f * diffH / 2.0f;
            gameCam.transform.position = camPos;

            // 카메라 초기화 되었음 표시
            _cameraInitialized = true;
        }
        #endregion // Loading

        // 플레이 시작
        public void StartPlay()
        {
            // 상태 변경
            SetState(StateType.Play);

            // 노래 시작
            _srcSong.mute = false;
            _srcSong.Play();
            if (_TestInfo != null && _TestInfo._StartFrame > 0)
            {
                _srcSong.time = _TestInfo._StartFrame / Define._fps;
            }

            // 진행 초기화
            _Frame = -1; // 첫 갱신 시 0프레임 되도록
            _FrameByOver = false;

            // 시작 직후 바로 0프레임째 업데이트 수행
            UpdatePlay();
        }

        // 엔진에서 호출하는 플레이 갱신
        public void UpdatePlay()
        {
            // 키입력 처리
            if (_HasKeyInputFocus)
            {
                if (Input.GetButtonDown(ButtonName._start) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Menu))
                {
                    StartPause();
                }
            }
            if (_isPaused)
            {
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

                if (songFrame > gameFrame + _songFrameOverToMuch)
                {
                    // 한꺼번에 다 따라잡아야 할 정도로 노래가 앞서감
                    // 기본 갱신 1회가 있으므로 노래 -1 까지 따라잡음
                    int targetFrame = (songFrame - 1);
                    while (_Frame < targetFrame)
                    {
                        UpdatePlayFrame(true);
                    }
                }
                else if (songFrame > gameFrame + _songFrameOverTolerance)
                {
                    // 조금씩 따라잡을 정도로 노래가 앞서감
                    //Debug.Log("[GameSystem] Song frame over occurred. Song:" + songFrame.ToString() + " Game:" + gameFrame.ToString());
                    int targetFrame = Mathf.Min(songFrame - 1, _Frame + _additionalBySongFrameOver);
                    while (_Frame < targetFrame)
                    {
                        //Debug.Log("over occured!");
                        UpdatePlayFrame(true);
                    }
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

            // 결과 보여줘야 하는가?
            if ((_isGameOvered && _Frame >= (_gameOveredFrame + _gameOverResultDelay))  // 게임오버되었고 딜레이 지남
                || !_srcSong.isPlaying  // 노래가 종료됨
                )
            {
                DoResult();
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
            _FrameByOver = byFrameOver;

            // 세부 갱신
            _logic.UpdatePlayContext();

            // 이동 물체들 갱신
            // 보통 플레이어기가 샷을 생성하고 적기가 탄을 생성하므로 이 순서로 갱신
            // Hit 체크도 업데이트 순서대로 수행
            UpdateMoverList(_Players, _FrameByOver);
            UpdateMoverList(_Shots, _FrameByOver);
            UpdateMoverList(_Enemys, _FrameByOver);
            UpdateMoverList(_Bullets, _FrameByOver);
            UpdateMoverList(_Effects, _FrameByOver);

            // UI 갱신
            if (!_FrameByOver)
            {
                if (_totalFrame > 0)
                {
                    _uiProgressBar.SetRate((float)_Frame / (float)_totalFrame);
                }
            }
        }

        /// <summary>
        /// 게임오버 발생 지정
        /// </summary>
        private void SetGameOvered()
        {
            _srcSong.mute = true;
            _isGameOvered = true;
            _gameOveredFrame = _Frame;
        }

        /// <summary>
        /// 게임오버 발생여부 초기화
        /// </summary>
        private void InitGameOvered()
        {
            _isGameOvered = false;
            _gameOveredFrame = 0;
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

        private void UpdateMoverList<T>(List<T> movers, bool frameByOver) where T : Mover
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
            if (!frameByOver)
            {
                int order = 0;
                for (int i = 0; i < movers.Count; ++i)
                {
                    // 뒤에 추가된 무버일수록 위에 그림
                    movers[i].Draw(order);
                    order += movers[i]._shape._SpriteOrderCount;
                }
            }
        }

        /// <summary>
        /// 지정한 무버 목록의 무버들을 풀로 되돌린다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="movers"></param>
        private void RemoveMoverList<T>(List<T> movers) where T : Mover
        {
            if (movers != null)
            {
                for (int i = movers.Count - 1; i >= 0; --i)
                {
                    T mover = movers[i];
                    mover.OnDestroy();
                    _moverPoolManager.Delete(mover);
                }
            }
            movers.Clear();
        }

        /// <summary>
        /// 활성화된 모든 무버를 풀로 되돌린다.
        /// </summary>
        private void RemoveAllMover()
        {
            RemoveMoverList(_Players);
            RemoveMoverList(_Shots);
            RemoveMoverList(_Enemys);
            RemoveMoverList(_Bullets);
            RemoveMoverList(_Effects);
        }
        #endregion //Mover

        #region Player
        public T CreatePlayer<T>() where T : Player, new()
        {
            T player = _moverPoolManager.Create<T>();
            _Players.Add(player);
            return player;
        }

        /// <summary>
        /// 플레이어기가 죽었을 때
        /// </summary>
        public void OnPlayerDied()
        {
            // 게임오버 지정
            SetGameOvered();
        }

        /// <summary>
        /// 플레이어기가 시스템에 의해 무적상태가 되었는가?
        /// </summary>
        /// <returns></returns>
        public bool IsPlayerInvincible()
        {
            if (_TestInfo != null)
            {
                // 테스트 정보에서 지정
                if (_TestInfo._IsInvincible    // 항상 무적
                    || (_TestInfo._StartFrame > 0 && _Frame <= _TestInfo._StartFrame))  // 테스트용 시작프레임 이전
                {
                    return true;
                }
            }
            return false;
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
        #endregion // Bullet

        #region Effect
        public T CreateEffect<T>() where T : Effect, new()
        {
            T effect = _moverPoolManager.Create<T>();
            _Effects.Add(effect);
            return effect;
        }
        #endregion

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
        /// 0 <= n < 1 인 무작위 실수 구함
        /// </summary>
        /// <returns>0 <= n < 1 인 실수</returns>
        public float GetRandom01()
        {
            return (float)_random.NextDouble();
        }

        /// <summary>
        /// 범위 내 무작위 실수 구함
        /// <para>범위 올바르지 않으면 min 리턴</para>
        /// </summary>
        public float GetRandomRange(float min, float max)
        {
            if (min >= max)
            {
                return min;
            }
            else
            {
                return min + GetRandom01() * (max - min);
            }
        }
        #endregion // Random

        #region Pause
        /// <summary>
        /// 일시정지 시작
        /// </summary>
        public void StartPause()
        {
            if (_stateType == StateType.Play && !_isPaused)
            {
                _srcSong.Pause();
                _srcEffect.Pause();
                _timeScaleBeforePause = Time.timeScale;
                Time.timeScale = 0.0f;
                _isPaused = true;
                _UIPause.Open();
            }
        }

        /// <summary>
        /// 일시정지 중지
        /// </summary>
        public void StopPause()
        {
            if (_isPaused)
            {
                Time.timeScale = _timeScaleBeforePause;
                _srcSong.UnPause();
                _srcEffect.UnPause();
                _isPaused = false;
            }
        }
        #endregion Pause

        /// <summary>
        /// 게임 재시작
        /// </summary>
        public void Retry()
        {
            if (_stateType != StateType.Invalid && _stateType != StateType.Load)
            {
                StartLoading();   
            }
        }

        /// <summary>
        /// 결과 정산 및 보여주기
        /// </summary>
        private void DoResult()
        {
            // 상태 변경
            SetState(StateType.Result);

            // 유지할 필요 없는 구성요소 무효화
            _srcSong.Stop();
            _srcEffect.Stop();
            RemoveAllMover();

            // 하이스코어 갱신
            bool newHighScore = false;
            int highScore = Define.GetSongHighScore(_beatInfo);
            if (_score > highScore)
            {
                newHighScore = true;
                highScore = _score;
                Define.SetSongHighScore(_beatInfo, _score);
            }
            // 클리어 여부 갱신
            bool cleared = !_isGameOvered;
            if (cleared)
            {
                if (!Define.IsSongCleared(_beatInfo))
                {
                    Define.SetSongCleared(_beatInfo, true);
                }

                // 업적 달성 여부는 로컬로 불러오지 않으므로 매번 시도
                if (GlobalSystem._Instance._IsAuthenticated)
                {
                    Define.ReportAchievementProgress(_beatInfo._clearAchievementKey, 100.0);
                }
            }

            // 리더보드 갱신 시도
            if (GlobalSystem._Instance._IsAuthenticated)
            {
                Define.ReportScoreToSongLeaderboard(_beatInfo, _score);
            }

            // 결과 UI
            _UIResult.SetData(_beatInfo, cleared, _score, highScore, newHighScore);
            _UIResult.Open();
        }

        /// <summary>
        /// 사운드 이펙트 재생
        /// </summary>
        public void PlaySoundEffect(string path)
        {
            AudioClip clip = Resources.Load("Sounds/Effects/" + path) as AudioClip;
            if (clip != null)
            {
                _srcEffect.PlayOneShot(clip);
            }
        }

        /// <summary>
        /// 로직 접근용
        /// </summary>
        public T GetLogic<T>() where T : BaseGameLogic
        {
            return _logic as T;
        }

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