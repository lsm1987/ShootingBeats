using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    // 게임 진행 관리
    // 진행 세부는 상속 클래스에서 지정
    public abstract partial class GameSystem : MonoBehaviour
    {
        private static GameSystem _instance;
        public static GameSystem _Instance { get { return _instance; } }

        private int _oriVSyncCount = 0; // 유니티 설정 복원용
        private const int _fps = 60; // 갱신주기
        public FSM _FSM { get; private set; }
        private ShapePoolManager _shapePoolManager = new ShapePoolManager();    // 외양 풀
        private MoverPoolManager _moverPoolManager = new MoverPoolManager();    // Mover 풀
        private AudioSource _srcSong;    // 노래 재생할 소스
        private List<Bullet> _bullets = new List<Bullet>(); // 살아있는 탄 목록
        protected int _Frame { get; private set; }

        // 자식 클래스에서 재정의 가능한 멤버변수 /////////////////////////////////
        // 게임 경계. 좌하단 min(-,-), 우상단 max(+,+). 짧은 쪽이 1
        // 3:4 -> 1:1.3
        public float _MaxX { get { return 1.0f; } }
        public float _MinX { get { return -1.0f; } }
        public float _MaxY { get { return 1.3f; } }
        public float _MinY { get { return -1.3f; } }

        protected abstract string _SongPath { get; }

        /// <summary>
        ///  씬 진입 시
        /// </summary>
        private void Awake()
        {
            _instance = this;
            _oriVSyncCount = QualitySettings.vSyncCount;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = _fps;

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
            QualitySettings.vSyncCount = _oriVSyncCount;
            Application.targetFrameRate = -1;
        }

        // 고정 프레임 간격으로 갱신
        private void Update()
        {
            _FSM._CurrentState.OnUpdate();
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
            Debug.Log("deviceR:" + deviceR.ToString() + " gameR:" + gameR.ToString());

            // 메인 카메라를 게임 카메라로 사용
            Camera gameCam = Camera.main;
            
            // 게임 카메라 뷰포트 영역 지정
            if (gameR >= deviceR)
            {
                // 게임 화면 가로를 기기 가로에 가득 채움
                // 세로 상단
                float h = (1 / gameR) / (1 / deviceR);
                gameCam.rect = new Rect(0.0f, 1.0f - h, 1.0f, h);
            }
            else
            {
                // 게임 화면 세로를 기기 세로에 가득 채움
                // 가로 중앙
                float w = gameR / deviceR;
                gameCam.rect = new Rect(0.5f - w / 2.0f, 0.0f, w, 1.0f);
            }
            
            // 게임 세로 범위가 게임 카메라가 보여주는 세로 범위가 되도록 맞춤
            gameCam.orthographicSize = _MaxY;
        }
        #endregion // Camera

        #region Loading
        // 로딩 전체 감싸기
        private IEnumerator Loading()
        {
            // 노래 로딩
            {
                IEnumerator loadSong = LoadSong();
                while (loadSong.MoveNext())
                {
                    yield return loadSong.Current;
                }
            }

            // 특화 정보 로딩
            {
                IEnumerator loadContext = LoadContext();
                while (loadContext.MoveNext())
                {
                    yield return loadContext.Current;
                }
            }
        }

        // 노래 로딩
        private IEnumerator LoadSong()
        {
            GameObject go = gameObject;
            _srcSong = go.AddComponent<AudioSource>();
            _srcSong.playOnAwake = false;
            _srcSong.clip = Resources.Load<AudioClip>(_SongPath);
            yield return null;
        }

        // 특화 정보 로딩
        protected abstract IEnumerator LoadContext();
        #endregion // Loading

        // 플레이 시작
        public void StartPlay()
        {
            // 노래 시작
            _srcSong.Play();

            // 진행 초기화
            _Frame = -1; // 첫 갱신 시 0프레임 되도록
        }

        // 고정 프레임 간격으로 갱신
        public void UpdatePlay()
        {
            // 프레임 갱신
            _Frame++;

            // 세부 갱신
            UpdatePlayContext();

            // 탄환 갱신
            UpdateBullet();
        }

        protected abstract void UpdatePlayContext();

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

        #region Bullet
        public void PoolStackBullet<T>(int count) where T : Bullet, new()
        {
            _moverPoolManager.PoolStack<T>(count);
        }

        /// <summary>
        /// 풀에서 탄을 생성하고, 탄 업데이트 목록의 가장 뒤에 추가한다.
        /// <para>생성된 탄을 리턴</para>
        /// </summary>
        public T CreateBullet<T>() where T : Bullet, new()
        {
            T bullet = _moverPoolManager.Create<T>();
            if (bullet != null)
            {
                _bullets.Add(bullet);
            }
            return bullet;
        }

        /// <summary>
        /// 탄 업데이트 목록 순회하며 갱신
        /// </summary>
        private void UpdateBullet()
        {
            // 갱신 도중에 새 탄이 생길 수 있으므로 인덱스 순회
            for (int i = 0; i < _bullets.Count; ++i)
            {
                _bullets[i].Move();
            }

            // 역순 순회하며 삭제
            // 삭제 도중에 새 탄이 생기면 안됨
            for (int i = _bullets.Count - 1; i >= 0; --i)
            {
                if (!_bullets[i]._alive)
                {
                    Bullet bullet = _bullets[i];
                    bullet.OnDestroy();
                    _bullets.RemoveAt(i);
                    _moverPoolManager.Delete(bullet);
                }
            }

            // 그리기
            foreach (Bullet bullet in _bullets)
            {
                bullet.Draw();
            }
        }
        #endregion // Bullet

        #region Debug
        private void OnDrawGizmos()
        {
            // 경계
            {
                Vector2 lt = new Vector2(_MinX, _MaxY);
                Vector2 lb = new Vector2(_MinX, _MinY);
                Vector2 rb = new Vector2(_MaxX, _MinY);
                Vector2 rt = new Vector2(_MaxX, _MaxY);
                Gizmos.color = Color.white;
                Gizmos.DrawLine(lt, lb);
                Gizmos.DrawLine(lb, rb);
                Gizmos.DrawLine(rb, rt);
                Gizmos.DrawLine(rt, lt);
            }
        }
        #endregion // Debug
    }
}