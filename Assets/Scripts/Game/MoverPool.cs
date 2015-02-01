using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game
{
    // Mover 풀 모음
    public class MoverPoolManager
    {
        // 타입별 풀. <class type, 풀>
        private Dictionary<Type, Stack<Mover>> _pools = new Dictionary<Type, Stack<Mover>>();

        /// <summary>
        /// 특정 타입의 Mover를 생성한다.
        /// </summary>
        public T Create<T>() where T : Mover, new()
        {
            Stack<Mover> pool = null;
            if (!_pools.TryGetValue(typeof(T), out pool))
            {
                // 새 풀 생성
                pool = new Stack<Mover>();
                _pools.Add(typeof(T), pool);
            }

            T instance = null;
            if (pool.Count > 0)
            {
                // 인스턴스 재활용
                instance = pool.Pop() as T;
            }
            else
            {
                // 인스턴스 생성
                instance = new T();
                instance.OnFirstCreatedInPool(pool); // 이 풀 매니저에서 생성되었음을 기억
            }

            if (instance == null)
            {
                Debug.LogError("[MoverPoolManager] Instance is null. T:" + typeof(T).ToString());
            }

            // 여기까지 왔으면 인스턴스 리턴
            return instance;
        }

        /// <summary>
        /// 지정한 인스턴스를 풀에 되돌린다.
        /// </summary>
        public void Delete(Mover instance)
        {
            if (instance == null)
            {
                return;
            }

            if (instance._pool != null)
            {
                // 되돌아갈 풀이 지정되어있다면 풀로 되돌림
                instance._pool.Push(instance);
            }
        }

        /// <summary>
        /// 지정한 타입에 해당하는 풀을 삭제하고 인스턴스도 삭제한다.
        /// </summary>
        public void Clear(Type type)
        {
            Stack<Mover> pool = null;
            if (_pools.TryGetValue(type, out pool))
            {
                pool.Clear();
                _pools.Remove(type);
            }
        }

        public int GetCount(Type type)
        {
            Stack<Mover> pool = null;
            if (_pools.TryGetValue(type, out pool))
            {
                return pool.Count;
            }
            else
            {
                return 0;
            }
        }
    }
}
