using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public class ShapePoolManager
    {
        private Dictionary<string, Stack<Shape>> _pools = new Dictionary<string, Stack<Shape>>();

        public Shape Create(string subPath)
        {
            Stack<Shape> pool = null;
            if (!_pools.TryGetValue(subPath, out pool))
            {
                // 새 풀 생성
                pool = new Stack<Shape>();
                _pools.Add(subPath, pool);
            }

            Shape instance = null;
            if (pool.Count > 0)
            {
                instance = pool.Pop();
            }
            else
            {
                string prefabPath = Shape.PrefabRoot + "/" + subPath;
                UnityEngine.Object prefab = Resources.Load(prefabPath);
                if (prefab == null)
                {
                    Debug.LogError("[ShapePoolManager] invalid path:" + prefabPath);
                    return null;
                }
                instance = (GameObject.Instantiate(prefab) as GameObject).GetComponent<Shape>();
                instance.OnFirstCreatedInPool(subPath);
            }

            return instance;
        }

        public void Delete(Shape instance)
        {
            if (instance == null) { return; }
            Stack<Shape> pool = null;
            if (!_pools.TryGetValue(instance._subPath, out pool) || pool == null) { return; }
            if (pool.Contains(instance)) { return; }
            pool.Push(instance);
        }

        public void Clear(string subPath)
        {
            Stack<Shape> pool = null;
            if (!_pools.TryGetValue(subPath, out pool)) { return; }
            if (pool != null) { pool.Clear(); }
            _pools.Remove(subPath);
        }

        public int GetCount(string subPath)
        {
            Stack<Shape> pool = null;
            if (!_pools.TryGetValue(subPath, out pool) || pool == null) { return 0; }
            return pool.Count;
        }
    }
}
