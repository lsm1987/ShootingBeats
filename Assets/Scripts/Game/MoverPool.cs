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
        /// 지정한 풀을 찾거나, 없으면 생성하여 리턴한다.
        /// </summary>
        private Stack<Mover> GetOrCreatePool<T>() where T : Mover, new()
        {
            Stack<Mover> pool = null;
            if (!_pools.TryGetValue(typeof(T), out pool))
            {
                // 새 풀 생성
                pool = new Stack<Mover>();
                _pools.Add(typeof(T), pool);
            }
            return pool;
        }

        /// <summary>
        /// 새 인스턴스 생성 및 초기화
        /// </summary>
        private T CreateInstance<T>(Stack<Mover> pool) where T : Mover, new()
        {
            T instance = new T();
            instance.OnFirstCreatedInPool(pool); // 이 풀에서 생성되었음을 기억
            return instance;
        }

        /// <summary>
        /// 인스턴스를 생성해 풀에 쌓아둔다.
        /// </summary>
        public void PoolStack<T>(int count) where T : Mover, new()
        {
            Stack<Mover> pool = GetOrCreatePool<T>();

            for (int i = 0; i < count; ++i)
            {
                T instance = CreateInstance<T>(pool);
                pool.Push(instance);
            }
        }

        /// <summary>
        /// 특정 타입의 Mover를 생성한다.
        /// </summary>
        public T Create<T>() where T : Mover, new()
        {
            Stack<Mover> pool = GetOrCreatePool<T>();

            T instance = null;
            if (pool.Count > 0)
            {
                // 인스턴스 재활용
                instance = pool.Pop() as T;
            }
            else
            {
                // 인스턴스 생성
                instance = CreateInstance<T>(pool);
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
