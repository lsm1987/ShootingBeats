using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    // 외양 리소스
    public class Shape : MonoBehaviour
    {
        #region Static Define
        // 전역 정의
        public const string _prefabRoot = "Shapes";

        public static string GetPrefabFullPath(string subPath)
        {
            return _prefabRoot + "/" + subPath;
        }
        #endregion // Static Define

        // 표시 크기. 반경
        public float _size;
        // 충돌 판정 크기. 반경
        public float _hit;

        // GameObject는 자주 접근하지 않으므로 캐싱하지 않음
        [HideInInspector]
        public Transform _trans;
        public string _poolKey { get; private set; } // 이 인스턴스가 되돌아갈 풀의 구분자

        /// <summary>
        /// 최초 오브젝트 생성시
        /// </summary>
        private void Awake()
        {
            _trans = transform;
        }

        /// <summary>
        /// 풀 내에서 처음 생성되었을 때
        /// </summary>
        public void OnFirstCreatedInPool(string poolKey_)
        {
            _poolKey = poolKey_;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 풀에서 생성 직전에 호출
        /// </summary>
        public void OnBeforeCreatedFromPool()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 삭제되어 풀에 들어간 직후 호출
        /// </summary>
        public void OnAfterDestroyedToPool()
        {
            gameObject.SetActive(false);
        }

        #region Debug
        private void OnDrawGizmos()
        {
            DrawGizmoCircle(Color.cyan, transform.position, _size);
            DrawGizmoCircle(Color.green, transform.position, _hit);
        }

        private void DrawGizmoCircle(Color color, Vector2 c, float r)
        {
            Vector2 top = new Vector2(c.x, c.y + r);
            Vector2 left = new Vector2(c.x - r, c.y);
            Vector2 bottom = new Vector2(c.x, c.y - r);
            Vector2 right = new Vector2(c.x + r, c.y);
            Gizmos.color = color;
            Gizmos.DrawLine(top, left);
            Gizmos.DrawLine(left, bottom);
            Gizmos.DrawLine(bottom, right);
            Gizmos.DrawLine(right, top);
        }
        #endregion // Debug
    }
}