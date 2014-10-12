using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    // 타입별 Mover pool 원형
    public class MoverPoolManager
    {
        private Dictionary<int, Stack<Mover>> _pools = new Dictionary<int, Stack<Mover>>();

        public T Create<T>(int moverType)
            where T : Mover, new()
        {
            Stack<Mover> pool = null;
            if (!_pools.TryGetValue(moverType, out pool))
            {
                // 새 풀 생성
                pool = new Stack<Mover>();
                _pools.Add(moverType, pool);
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
                if (instance.MoverType != moverType)
                {
                    Debug.LogError("[MoverPoolManager] Invalid moverType. moverType:" + moverType.ToString() + " T:" + typeof(T).ToString());
                    return null;
                }
                instance.OnFirstCreatedInPool(this); // 이 풀 매니저에서 생성되었음을 기억
            }

            if (instance == null)
            {
                Debug.LogError("[MoverPoolManager] Instance is null. moverType:" + moverType.ToString() + " T:" + typeof(T).ToString());
                return null;
            }

            // 여기까지 왔으면 인스턴스 리턴
            return instance;
        }

        public void Delete(Mover instance)
        {
            if (instance == null) { return; }
            Stack<Mover> pool = null;
            if (!_pools.TryGetValue(instance.MoverType, out pool) || pool == null) { return; }
            if (pool.Contains(instance)) { return; } // 이미 들어있음
            pool.Push(instance);
        }

        public void Clear(int moverType)
        {
            Stack<Mover> pool = null;
            if (!_pools.TryGetValue(moverType, out pool)) { return; }
            if (pool != null) { pool.Clear(); }
            _pools.Remove(moverType);
        }

        public int GetCount(int moverType)
        {
            Stack<Mover> pool = null;
            if (!_pools.TryGetValue(moverType, out pool) || pool == null) { return 0; }
            return pool.Count;
        }
    }
}
