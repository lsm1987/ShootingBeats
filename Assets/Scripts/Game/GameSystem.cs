using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game
{
    public class GameSystem : MonoBehaviour
    {
        private static GameSystem _instance;
        public static GameSystem Instance { get { return _instance; } }

        // 게임 경계. 좌하단 min, 우상단 max
        public float _maxX = 1.0f;
        public float _minX = -1.0f;
        public float _maxY = 1.0f;
        public float _minY = -1.0f;

        private ShapePoolManager _shapePoolManager = new ShapePoolManager();
        private MoverPoolManager _baseBulletPoolManager = new MoverPoolManager();

        private List<Bullet> _bullets = new List<Bullet>(); // 살아있는 탄 목록
        private int testFrame = 0;
        float testShotAngle = 0;
        float testShotAngleRate = 0.02f;

        private void Awake()
        {
            _instance = this;
        }

        private void OnDestroy()
        {
            _instance = null;
        }

        // 고정 프레임 간격으로 갱신
        private void FixedUpdate()
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
                Bullet b = CreateBaseBullet<Bullet>(BaseBulletType.Base);
                b.Init(Shape.PrefabCommonPsNeedleC, 0.0f, 0.0f, testShotAngle
                    , 0.0f, 0.01f, 0.0f);

                testShotAngle += testShotAngleRate;
                testShotAngle -= Mathf.Floor(testShotAngle);

                testFrame = 0;
            }

            UpdateBullet();
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
        // 풀에서 탄을 생성하고, 업데이트 목록의 가장 뒤에 추가한다.
        // 생성된 탄을 리턴
        public T CreateBaseBullet<T>(BaseBulletType moverType)
            where T : Bullet, new()
        {
            T bullet = _baseBulletPoolManager.Create<T>((int)moverType);
            if (bullet != null)
            {
                _bullets.Add(bullet);
            }
            return bullet;
        }

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
                    _bullets[i].OnDestroy();
                    _bullets.RemoveAt(i);
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