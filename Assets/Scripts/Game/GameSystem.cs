using UnityEngine;
using System;
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

        private ShapePoolManager _shapePoolManager = new ShapePoolManager();    // 외양 풀
        private MoverPoolManager _moverPoolManager = new MoverPoolManager();    // Mover 풀

        private AudioSource srcSong;    // 노래 재생할 소스
        private List<Bullet> _bullets = new List<Bullet>(); // 살아있는 탄 목록
        private int testFrame = 0;
        float testShotAngle = 0;
        float testShotAngleRate = 0.02f;

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

            // 로딩
            Loading();
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
            /*
            if (testFrame == 60)
            {
                Bullet b = CreateBaseBullet<Bullet>(BaseBulletType.Base);
                b.Init(Shape.PrefabCommonDpBlueBulletC, 0.0f, 0.0f, 270.0f
                    , 0.0f, 0.02f, 0.0f);
                testFrame = 0;
            }
            */

            testFrame++;
            if (testFrame == 5)
            {
                Bullet b = CreateBullet<Bullet>();
                b.Init("Common/PsNeedleC", 0.0f, 0.0f, testShotAngle
                    , 0.0f, 0.01f, 0.0f);

                testShotAngle += testShotAngleRate;
                testShotAngle -= Mathf.Floor(testShotAngle);

                testFrame = 0;
            }

            UpdateBullet();
        }

        // 로딩
        private void Loading()
        {
            GameObject go = gameObject;
            srcSong = go.AddComponent<AudioSource>();
            srcSong.playOnAwake = false;
            srcSong.clip = Resources.Load<AudioClip>("Sounds/RainbowSocialism");
        }

        // 고정 프레임 간격으로 갱신
        private void UpdateFrame()
        {

        }

        #region Shape
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