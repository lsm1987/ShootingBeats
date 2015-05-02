using UnityEngine;
using System.Collections.Generic;

using System;

namespace Game
{
    // 게임 내의 이동 물체
    public abstract class Mover
    {
        public Shape _shape;
        public Vector2 _pos;
        public float _X
        {
            get { return _pos.x; }
            set { _pos.x = value; }
        }
        public float _Y
        {
            get { return _pos.y; }
            set { _pos.y = value; }
        }
        public float _angle; // 현재 회전. 단위 0.0f~1.0f
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
        public virtual void Init(string shapeSubPath, float x, float y, float angle)
        {
            SetShape(shapeSubPath);
            _X = x;
            _Y = y;
            _angle = angle;
            _alive = true;

            // 배치 후 외양에 위치 반영. 배치 즉시 발생하는 파티클에서 필요할 수 있음
            _shape.SetPosition(_pos, _angle);
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
        public void Draw(int order)
        {
            _shape.SetPosition(_pos, _angle);
            _shape.SetSortingOrder(order);
        }

        // 충돌 판정. 충돌여부를 리턴
        public bool IsHit(Mover mover)
        {
            // 대상 또는 자신의 hit 영역이 없다면 Hit 발생하지 않음
            if (_shape._hit != 0.0f && mover._shape._hit != 0.0f)
            {
                float dx = mover._X - _X;
                float dy = mover._Y - _Y;
                float hit = mover._shape._hit + _shape._hit;
                return (dx * dx + dy * dy) < (hit * hit);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 이 무버가 지정한 무버 목록 중 하나에라도 충돌하는가?
        /// <para>충돌했다면 그 무버 리턴</para>
        /// </summary>
        public T IsHit<T>(List<T> movers) where T : Mover
        {
            foreach (T mover in movers)
            {
                if (IsHit(mover))
                {
                    return mover;
                }
            }
            return null;
        }

        // 게임 영역 안에 있는지 여부 리턴
        public bool IsInStage()
        {
            if ((_X + _shape._size) <= GameSystem._Instance._MinX
                || (_X - _shape._size) >= GameSystem._Instance._MaxX
                || (_Y + _shape._size) <= GameSystem._Instance._MinY
                || (_Y - _shape._size) >= GameSystem._Instance._MaxY)
            {
                return false; // 벗어남
            }
            else
            {
                return true;
            }
        }

        // 순회에 의한 삭제 전 호출
        public virtual void OnDestroy()
        {
            ClearShape();
        }

        public int _Frame
        {
            get
            {
                return GameSystem._Instance._Frame;
            }
        }
    }
}