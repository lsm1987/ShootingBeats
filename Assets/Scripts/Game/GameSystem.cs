using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    // 게임 진행 관리
    public class GameSystem : MonoBehaviour
    {
        private static GameSystem _instance;
        public static GameSystem Instance { get { return _instance; } }

        private int _oriVSyncCount = 0; // 유니티 설정 복원용
        private const int _fps = 60; // 갱신주기

        // 게임 경계. 좌하단 min(-,-), 우상단 max(+,+). 짧은 쪽이 1
        // 3:4 -> 1:1.3
        public float _maxX { get { return 1.0f; } }
        public float _minX { get { return -1.0f; } }
        public float _maxY { get { return 1.3f; } }
        public float _minY { get { return -1.3f; } }

        private GameSystemFSM _fsm = new GameSystemFSM();
        private ShapePoolManager _shapePoolManager = new ShapePoolManager();    // 외양 풀
        private MoverPoolManager _moverPoolManager = new MoverPoolManager();    // Mover 풀

        private AudioSource _srcSong;    // 노래 재생할 소스
        private List<Bullet> _bullets = new List<Bullet>(); // 살아있는 탄 목록
        private int testFrame = 0;

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
            Camera cam = Camera.main;
            cam.orthographicSize = _maxY;

            // FSM 초기화
            _fsm.AddState(new GameSystemLoadState());
            _fsm.AddState(new GameSystemPlayState());

            // 로딩 시작
            _fsm.SetState(GameSystemStateType.Load);
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
            if (_fsm._CurrentState != null)
            {
                _fsm._CurrentState.OnUpdate();
            }
        }

        #region Loading
        public void StartLoading()
        {
            StartCoroutine(Loading());
        }

        // 로딩
        private IEnumerator Loading()
        {
            // 노래 로딩
            GameObject go = gameObject;
            _srcSong = go.AddComponent<AudioSource>();
            _srcSong.playOnAwake = false;
            _srcSong.clip = Resources.Load<AudioClip>("Sounds/RainbowSocialism");
            yield return new WaitForEndOfFrame();

            // 외양 로딩
            PoolStackShape("Common/DpBlueBulletC", 50);
            yield return new WaitForEndOfFrame();
            PoolStackShape("Common/DpRedBulletC", 50);
            yield return new WaitForEndOfFrame();

            // 탄 로딩
            PoolStackBullet<Bullet>(100);
            yield return new WaitForEndOfFrame();
            yield return null;
            yield return null;

            // 로딩 종료
            _fsm.SetState(GameSystemStateType.Play);
        }
        #endregion // Loading

        // 플레이 시작
        public void StartPlay()
        {
            // 노래 시작
            _srcSong.Play();

            // 진행 초기화
            testFrame = -1; // 첫 갱신 시 0프레임 되도록
        }

        // 고정 프레임 간격으로 갱신
        public void UpdatePlay()
        {
            testFrame++;

            if (testFrame == 64)
            {
                AddTestNormalBullet();
            }
            else if (testFrame == 75)
            {
                AddTestNormalBullet();
            }
            //
            else if (testFrame == 87)
            {
                Bullet b = CreateBullet<Bullet>();
                b.Init("Common/DpBlueBulletC", 0.0f, 1.0f, (0.625f + 0.25f / 6.0f * 1.0f)
                    , 0.0f, 0.03f, 0.0f);
            }
            else if (testFrame == 93)
            {
                Bullet b = CreateBullet<Bullet>();
                b.Init("Common/DpBlueBulletC", 0.0f, 1.0f, (0.625f + 0.25f / 6.0f * 2.0f)
                    , 0.0f, 0.03f, 0.0f);
            }
            else if (testFrame == 100)
            {
                Bullet b = CreateBullet<Bullet>();
                b.Init("Common/DpBlueBulletC", 0.0f, 1.0f, (0.625f + 0.25f / 6.0f * 3.0f)
                    , 0.0f, 0.03f, 0.0f);
            }
            else if (testFrame == 108)
            {
                Bullet b = CreateBullet<Bullet>();
                b.Init("Common/DpBlueBulletC", 0.0f, 1.0f, (0.625f + 0.25f / 6.0f * 4.0f)
                    , 0.0f, 0.03f, 0.0f);
            }
            else if (testFrame == 114)
            {
                Bullet b = CreateBullet<Bullet>();
                b.Init("Common/DpBlueBulletC", 0.0f, 1.0f, (0.625f + 0.25f / 6.0f * 5.0f)
                    , 0.0f, 0.03f, 0.0f);
            }
            else if (testFrame == 120)
            {
                Bullet b = CreateBullet<Bullet>();
                b.Init("Common/DpBlueBulletC", 0.0f, 1.0f, (0.625f + 0.25f / 6.0f * 6.0f)
                    , 0.0f, 0.03f, 0.0f);
            }
            //
            else if (testFrame == 64 + 84)
            {
                AddTestNormalBullet();
            }
            else if (testFrame == 75 + 84)
            {
                AddTestNormalBullet();
            }
            //
            else if (testFrame == 87 + 84)
            {
                Bullet b = CreateBullet<Bullet>();
                b.Init("Common/DpBlueBulletC", 0.0f, 1.0f, (0.875f - 0.25f / 6.0f * 1.0f)
                    , 0.0f, 0.03f, 0.0f);
            }
            else if (testFrame == 93 + 84)
            {
                Bullet b = CreateBullet<Bullet>();
                b.Init("Common/DpBlueBulletC", 0.0f, 1.0f, (0.875f - 0.25f / 6.0f * 2.0f)
                    , 0.0f, 0.03f, 0.0f);
            }
            else if (testFrame == 100 + 84)
            {
                Bullet b = CreateBullet<Bullet>();
                b.Init("Common/DpBlueBulletC", 0.0f, 1.0f, (0.875f - 0.25f / 6.0f * 3.0f)
                    , 0.0f, 0.03f, 0.0f);
            }
            else if (testFrame == 108 + 84)
            {
                Bullet b = CreateBullet<Bullet>();
                b.Init("Common/DpBlueBulletC", 0.0f, 1.0f, (0.875f - 0.25f / 6.0f * 4.0f)
                    , 0.0f, 0.03f, 0.0f);
            }
            else if (testFrame == 114 + 84)
            {
                Bullet b = CreateBullet<Bullet>();
                b.Init("Common/DpBlueBulletC", 0.0f, 1.0f, (0.875f - 0.25f / 6.0f * 5.0f)
                    , 0.0f, 0.03f, 0.0f);
            }
            else if (testFrame == 120 + 84)
            {
                Bullet b = CreateBullet<Bullet>();
                b.Init("Common/DpBlueBulletC", 0.0f, 1.0f, (0.875f - 0.25f / 6.0f * 6.0f)
                    , 0.0f, 0.03f, 0.0f);
            }
            //
            else if (testFrame == 249)
            {
                Bullet b = CreateBullet<Bullet>();
                b.Init("Common/DpRedBulletC", -0.2f, 1.0f, 0.75f
                    , 0.0f, 0.03f, 0.0f);
                b = CreateBullet<Bullet>();
                b.Init("Common/DpRedBulletC", 0.2f, 1.0f, 0.75f
                    , 0.0f, 0.03f, 0.0f);
            }
            else if (testFrame == 274)
            {
                Bullet b = CreateBullet<Bullet>();
                b.Init("Common/DpRedBulletC", -0.4f, 1.0f, 0.75f
                    , 0.0f, 0.03f, 0.0f);
                b = CreateBullet<Bullet>();
                b.Init("Common/DpRedBulletC", 0.4f, 1.0f, 0.75f
                    , 0.0f, 0.03f, 0.0f);
            }
            else if (testFrame == 312)
            {
                Bullet b = CreateBullet<Bullet>();
                b.Init("Common/DpRedBulletC", -0.6f, 1.0f, 0.75f
                    , 0.0f, 0.03f, 0.0f);
                b = CreateBullet<Bullet>();
                b.Init("Common/DpRedBulletC", 0.6f, 1.0f, 0.75f
                    , 0.0f, 0.03f, 0.0f);
            }
            else if (testFrame == 329)
            {
                Bullet b = CreateBullet<Bullet>();
                b.Init("Common/DpRedBulletC", -0.8f, 1.0f, 0.75f
                    , 0.0f, 0.03f, 0.0f);
                b = CreateBullet<Bullet>();
                b.Init("Common/DpRedBulletC", 0.8f, 1.0f, 0.75f
                    , 0.0f, 0.03f, 0.0f);
            }
            //
            else if (testFrame == 356)  // 1
            {
                AddTestCircleBlueBullet();
            }
            else if (testFrame == 369)  // 3
            {
                AddTestCircleRedBullet();
            }
            else if (testFrame == 380)  // 5
            {
                AddTestCircleBlueBullet();
            }
            else if (testFrame == 391)  // 7
            {
                AddTestCircleRedBullet();
            }
            else if (testFrame == 403)  // 9
            {
                AddTestCircleBlueBullet();
            }
            else if (testFrame == 417)  // 11
            {
                AddTestCircleRedBullet();
            }
            else if (testFrame == 429)  // 13
            {
                AddTestCircleBlueBullet();
            }
            else if (testFrame == 441)  // 15
            {
                AddTestCircleRedBullet();
            }
            else if (testFrame == 459)  // 17
            {
                AddTestCircleBlueBullet();
            }

            UpdateBullet();
        }

        private void AddTestNormalBullet()
        {
            Bullet b = CreateBullet<Bullet>();
            b.Init("Common/DpBlueBulletC", 0.0f, 1.0f, 0.75f
                , 0.0f, 0.03f, 0.0f);
        }

        private void AddTestCircleBlueBullet()
        {
            const int count = 20;
            for (int i = 0; i < count; ++i)
            {
                Bullet b = CreateBullet<Bullet>();
                b.Init("Common/DpBlueBulletC", 0.0f, 0.5f, (1.0f / count * i)
                    , 0.0f, 0.01f, 0.0f);
            }
        }

        private void AddTestCircleRedBullet()
        {
            const int count = 20;
            for (int i = 0; i < count; ++i)
            {
                Bullet b = CreateBullet<Bullet>();
                b.Init("Common/DpRedBulletC", 0.0f, 0.5f, (1.0f / count * i) + (1.0f / count / 2.0f)
                    , 0.0f, 0.01f, 0.0f);
            }
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
                Vector2 lt = new Vector2(_minX, _maxY);
                Vector2 lb = new Vector2(_minX, _minY);
                Vector2 rb = new Vector2(_maxX, _minY);
                Vector2 rt = new Vector2(_maxX, _maxY);
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