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
        
        // 이 외양을 구성하는 스프라이트의 정보
        private class SpriteInfo
        {
            private SpriteRenderer _sprite = null;  // 스프라이트 참조
            public int _OrderOffset { get; private set; }   // 소팅 오더 오프셋. 프리팹 상태의 소팅 오더 기본값을 오프셋으로 사용. 0 이상이어야 함. 그림이 겹치지 않으면 중복 가능

            public SpriteInfo(SpriteRenderer sprite_)
            {
                _sprite = sprite_;
                _OrderOffset = _sprite.sortingOrder;
            }

            public void SetOrder(int baseOrder)
            {
                _sprite.sortingOrder = baseOrder + _OrderOffset; 
            }
        }

        // 표시 크기. 반경
        public float _size;
        // 충돌 판정 크기. 반경
        public float _hit;

        // GameObject는 자주 접근하지 않으므로 캐싱하지 않음
        [HideInInspector]
        public Transform _trans;
        private SpriteInfo[] _spriteInfos = null;   // 이 오브젝트와 자식들이 갖고 있는 모든 렌더러들의 정보. null 가능
        public int _SpriteOrderCount { get; private set; } // 이 외양이 사용하는 오더 수
        public string _poolKey { get; private set; } // 이 인스턴스가 되돌아갈 풀의 구분자

        /// <summary>
        /// 최초 오브젝트 생성시
        /// </summary>
        private void Awake()
        {
            _trans = transform;

            // 스프라이트 관련 정보 초기화
            SpriteRenderer[] sprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
            if (sprites != null && sprites.Length > 0)
            {
                int maxOrder = 0;
                _spriteInfos = new SpriteInfo[sprites.Length];
                for (int i = 0; i < sprites.Length; ++i)
                {
                    // 스프라이트 정보 채우기
                    _spriteInfos[i] = new SpriteInfo(sprites[i]);

                    // 최대 오더값 갱신
                    if (maxOrder < _spriteInfos[i]._OrderOffset)
                    {
                        maxOrder = _spriteInfos[i]._OrderOffset;
                    }
                }

                // 최대 오더 + 1이 이 외양이 사용하는 오더 수가 된다.
                _SpriteOrderCount = maxOrder + 1;
            }
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

        /// <summary>
        /// 위치정보 적용
        /// </summary>
        public void SetPosition(Vector2 pos, float angle)
        {
            _trans.position = pos;
            _trans.rotation = Quaternion.Euler(0.0f, 0.0f, 360.0f * angle);
        }

        public void SetSortingOrder(int order)
        {
            if (_spriteInfos != null)
            {
                for (int i = 0; i < _spriteInfos.Length; ++i)
                {
                    _spriteInfos[i].SetOrder(order);
                }
            }
        }

        #region Debug
        private void OnDrawGizmos()
        {
            DrawGizmoCircle(Color.cyan, transform.position, _size);
            DrawGizmoCircle(Color.green, transform.position, _hit);
        }

        private void DrawGizmoCircle(Color color, Vector2 c, float r)
        {
            if (r > 0.0f)
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
        }
        #endregion // Debug
    }
}