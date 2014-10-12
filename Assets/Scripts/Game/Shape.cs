using UnityEngine;

namespace Game
{
    public class Shape : MonoBehaviour
    {
        #region Static Define
        // 전역 정의
        public const string PrefabRoot = "Shape";
        public const string PrefabCommon = "Common";
        public const string PrefabCommonDpBlueBulletC = PrefabCommon + "/DpBlueBulletC";
        public const string PrefabCommonDpRedBulletC = PrefabCommon + "/DpRedBulletC";

        public static string GetPrefabFullPath(string subPath)
        {
            return PrefabRoot + "/" + subPath;
        }
        #endregion // Static Define

        // 표시 크기. 반경
        public float _size;
        // 충돌 판정 크기. 반경
        public float _hit;

        public GameObject _go;
        public Transform _trans;
        public string _subPath; // pool 구분용

        private void Awake()
        {
            _go = gameObject;
            _trans = transform;
        }

        public void Init()
        {
            _go.SetActive(true);
        }

        public void OnFirstCreatedInPool(string subPath)
        {
            _subPath = subPath;
        }

        public void OnDestroy()
        {
            _go.SetActive(false);
        }

        #region Debug
        private void OnDrawGizmos()
        {
            DrawGizmoCircle(Color.white, transform.position, _size);
            DrawGizmoCircle(Color.yellow, transform.position, _hit);
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