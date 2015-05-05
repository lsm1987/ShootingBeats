using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    // 외양 리소스 관리
    public class ShapePoolManager
    {
        // 리소스별 풀. <subPath, 풀>
        private Dictionary<string, Stack<Shape>> _pools = new Dictionary<string, Stack<Shape>>();

        #region Debug
        // 최대 생성 수 기록
        private Dictionary<string, int> _maxCreatedCount = null;
        #endregion Debug

        /// <summary>
        /// 지정한 풀을 찾거나, 없으면 생성하여 리턴한다.
        /// </summary>
        private Stack<Shape> GetOrCreatePool(string subPath)
        {
            Stack<Shape> pool = null;
            if (!_pools.TryGetValue(subPath, out pool))
            {
                // 새 풀 생성
                pool = new Stack<Shape>();
                _pools.Add(subPath, pool);
            }
            return pool;
        }

        /// <summary>
        /// 새 인스턴스 생성 및 초기화
        /// </summary>
        private Shape CreateInstance(string subPath, Stack<Shape> pool)
        {
            string prefabPath = Shape._prefabRoot + "/" + subPath;
            UnityEngine.Object prefab = Resources.Load(prefabPath);
            if (prefab != null)
            {
                // 최초 한번만 지정할 정보
                GameObject obj = (GameObject.Instantiate(prefab) as GameObject);
                obj.name = subPath;
                Shape instance = obj.GetComponent<Shape>();
                instance.OnFirstCreatedInPool(subPath);
                AddCreatedCount(subPath);
                return instance;
            }
            else
            {
                Debug.LogError("[ShapePoolManager] invalid path:" + subPath);
                return null;
            }
        }

        /// <summary>
        /// 인스턴스를 생성해 풀에 쌓아둔다.
        /// <para>이미 풀에 쌓여있는 수는 제외</para>
        /// </summary>
        public void PoolStack(string subPath, int count)
        {
            Stack<Shape> pool = GetOrCreatePool(subPath);
            count -= pool.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; ++i)
                {
                    Shape instance = CreateInstance(subPath, pool);
                    pool.Push(instance);
                }
            }
        }

        /// <summary>
        /// 지정한 subPath의 외양을 생성한다.
        /// </summary>
        public Shape Create(string subPath)
        {
            Stack<Shape> pool = GetOrCreatePool(subPath);

            Shape instance = null;
            if (pool.Count > 0)
            {
                // 인스턴스 재활용
                instance = pool.Pop();
            }
            else
            {
                instance = CreateInstance(subPath, pool);
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

            Stack<Shape> pool = null;
            if (_pools.TryGetValue(instance._poolKey, out pool))
            {
                // 되돌아갈 풀이 지정되어있다면 풀로 되돌림
                pool.Push(instance);
                instance.OnAfterDestroyedToPool();
            }
            else
            {
                // 풀이 존재하지 않으면 않으면 바로 삭제
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

        #region Debug Func
        public void RecordMaxCreatedCount()
        {
            if (_maxCreatedCount == null)
            {
                _maxCreatedCount = new Dictionary<string, int>();
            }
        }

        private void AddCreatedCount(string subPath)
        {
            if (_maxCreatedCount != null)
            {
                int createdCount = 0;
                if (_maxCreatedCount.TryGetValue(subPath, out createdCount))
                {
                    _maxCreatedCount[subPath] = (createdCount + 1);
                }
                else
                {
                    _maxCreatedCount.Add(subPath, 1);
                }
            }
        }

        public void LogCreatedCount()
        {
            if (_maxCreatedCount != null)
            {
                Debug.Log("Shape CreatedCount----------------");
                foreach (KeyValuePair<string, int> pair in _maxCreatedCount)
                {
                    Debug.Log(pair.Key + " " + pair.Value.ToString());
                }
            }
        }
        #endregion Debug Func
    } // class ShapePoolManager
}
