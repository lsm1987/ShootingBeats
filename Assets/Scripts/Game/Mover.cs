using UnityEngine;
using System.Collections.Generic;

using System;

namespace Game
{
    // 게임 내의 이동 물체
    public abstract class Mover
    {
        public Shape _shape;
        public float _x, _y;
        public float _angle; // 현재 회전. 단위 0.0f~1.0f
        public float _scale;
        public bool _alive;
        public Type _poolKey { get; private set; } // 이 인스턴스가 되돌아갈 풀의 구분자

        public Mover()
        {
        }

        /// <summary>
        /// 풀 내에서 처음 생성되었을 때
        /// </summary>
        public void OnFirstCreatedInPool(Type poolKey_)
        {
            _poolKey = poolKey_;
        }

        /// <summary>
        /// 스테이지에 배치되었을 떄 초기화
        /// </summary>
        public void Init(string shapeSubPath, float x, float y, float angle)
        {
            SetShape(shapeSubPath);
            _x = x;
            _y = y;
            _angle = angle;
            _alive = true;
        }

        /// <summary>
        /// 기존에 적용중이던 외양이 있다면 삭제
        /// </summary>
        private void ClearShape()
        {
            if (_shape != null)
            {
                GameSystem._Instance.DeleteShape(_shape);
                _shape = null;
            }
        }

        private void SetShape(string shapeSubPath)
        {
            ClearShape();
            _shape = GameSystem._Instance.CreateShape(shapeSubPath);
        }
        
        // 이동
        public abstract void Move();

        // 그리기
        public void Draw()
        {
            _shape._trans.position = new Vector2(_x, _y);
            _shape._trans.rotation = Quaternion.Euler(0.0f, 0.0f, 360.0f * _angle);
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
            if ((_x + _shape._size) <= GameSystem._Instance._MinX
                || (_x - _shape._size) >= GameSystem._Instance._MaxX
                || (_y + _shape._size) <= GameSystem._Instance._MinY
                || (_y - _shape._size) >= GameSystem._Instance._MaxY)
            {
                return false; // 벗어남
            }
            else
            {
                return true;
            }
        }

        // 순회에 의한 삭제 전 호출
        public void OnDestroy()
        {
            ClearShape();
        }
    }
}