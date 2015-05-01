using UnityEngine;

namespace Game
{
    // 외양에 이펙트로서의 기능 추가
    public class ShapeEffect : MonoBehaviour
    {
        private ParticleSystem[] _particles = null;

        private void Awake()
        {
            _particles = gameObject.GetComponentsInChildren<ParticleSystem>(true);
        }

        /// <summary>
        /// 이펙트 재생중인가?
        /// </summary>
        public bool IsPlaying()
        {
            if (_particles != null)
            {
                for (int i = 0; i < _particles.Length; ++i)
                {
                    if (_particles[i].IsAlive())
                    {
                        // 살아있는 파티클이 하나라도 남아있다면 재생중
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 이팩트 재생
        /// </summary>
        public void Play()
        {
            if (_particles != null)
            {
                for (int i = 0; i < _particles.Length; ++i)
                {
                    _particles[i].Play();
                }
            }
        }
    }
}