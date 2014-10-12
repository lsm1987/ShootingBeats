using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    // 게임 내의 이동 물체
    public abstract class Mover
    {
        public Shape _shape;
        public float _x, _y;
        public float _angle; // 현재 회전. 단위 도.
        public float _scale;
        public bool _alive;
        private MoverPoolManager _poolManager = null; // 어떤 풀 매니저에서 생성되었는가?

        public Mover()
        {
        }

        // 상속받은 non abstract 클래스들은 모두 개별지정 필요
        public abstract int MoverType { get; }

        public void Init(string shapeSubPath, float x, float y, float angle)
        {
            SetShape(shapeSubPath);
            _x = x;
            _y = y;
            _angle = angle;
            _alive = true;
        }
        
        // 이동
        public abstract void Move();

        // 그리기
        public void Draw()
        {
            _shape._trans.position = new Vector2(_x, _y);
            _shape._trans.rotation = Quaternion.Euler(0.0f, 0.0f, _angle);
        }

        // 충돌 판정. 충돌여부를 리턴
        public bool IsHit(Mover mover)
        {
            float dx = mover._x - _x;
            float dy = mover._y - _y;
            float hit = mover._shape._hit + _shape._hit;
            return (dx * dx + dy * dy) < (hit * hit);
        }

        // 게임 영역 안에 있는지 여부 리턴
        public bool IsInStage()
        {
            if ((_x + _shape._size) <= GameSystem.Instance._minX
                || (_x - _shape._size) >= GameSystem.Instance._maxX
                || (_y + _shape._size) <= GameSystem.Instance._minY
                || (_y - _shape._size) >= GameSystem.Instance._maxY)
            {
                return false; // 벗어남
            }
            else
            {
                return true;
            }
        }

        public void SetShape(string shapeSubPath)
        {
            _shape = GameSystem.Instance.CreateShape(shapeSubPath);
            _shape.Init();
        }

        // 풀에서 최초 생성시 1회 호출
        public void OnFirstCreatedInPool(MoverPoolManager poolManager)
        {
            _poolManager = poolManager;
        }

        // 순회에 의한 삭제 전 호출
        public void OnDestroy()
        {
            _shape.OnDestroy();
            GameSystem.Instance.DeleteShape(_shape);
            _shape = null;
            _poolManager.Delete(this);
        }
    }
}