using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    // 외양 리소스 관리
    public class ShapePoolManager
    {
        // 리소스별 풀. <subPath, 풀>
        private Dictionary<string, Stack<Shape>> _pools = new Dictionary<string, Stack<Shape>>();

        /// <summary>
        /// 지정한 subPath의 외양을 생성한다.
        /// </summary>
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
                // 인스턴스 재활용
                instance = pool.Pop();
            }
            else
            {
                // 인스턴스 생성
                string prefabPath = Shape._prefabRoot + "/" + subPath;
                UnityEngine.Object prefab = Resources.Load(prefabPath);
                if (prefab != null)
                {
                    // 최초 한번만 지정할 정보
                    GameObject obj = (GameObject.Instantiate(prefab) as GameObject);
                    obj.name = subPath;
                    instance = obj.GetComponent<Shape>();
                    instance.OnFirstCreatedInPool(pool);
                }
            }

            // 생성 전 항상 지정할 정보
            if (instance != null)
            {
                instance.OnBeforeCreatedFromPool();
            }
            else
            {
                Debug.LogError("[ShapePoolManager] invalid path:" + subPath);
            }
            return instance;
        }

        /// <summary>
        /// 지정한 인스턴스를 풀에 되돌린다.
        /// </summary>
        public void Delete(Shape instance)
        {
            if (instance == null)
            {
                return;
            }

            if (instance._pool != null)
            {
                // 되돌아갈 풀이 지정되어있다면 풀로 되돌림
                instance._pool.Push(instance);
                instance.OnAfterDestroyedToPool();
            }
            else
            {
                // 풀이 지정되어있지 않으면 바로 삭제
                GameObject.Destroy(instance);
            }
        }

        /// <summary>
        /// 지정한 subPath에 해당하는 풀을 삭제하고 인스턴스도 삭제한다.
        /// </summary>
        public void Clear(string subPath)
        {
            Stack<Shape> pool = null;
            if (_pools.TryGetValue(subPath, out pool))
            {
                while(pool.Count > 0)
                {
                    Shape instance = pool.Pop();
                    GameObject.Destroy(instance);
                }
                _pools.Remove(subPath);
            }
        }

        public int GetCount(string subPath)
        {
            Stack<Shape> pool = null;
            if (_pools.TryGetValue(subPath, out pool))
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
