using UnityEngine;
using System.Collections;

namespace Game
{
    namespace RainbowSocialism
    {
        // 보스
        public class Boss : Enemy
        {
            private int _score = 10; // 피격시 획득 점수
            CoroutineManager _coroutineManager = new CoroutineManager();

            public Boss()
                : base()
            {
            }

            public override void Move()
            {
                if (GameSystem._Instance._Frame == 1)
                {
                    Debug.Log("before add co frame:" + GameSystem._Instance._Frame.ToString());
                    _coroutineManager.StartCoroutine(TestCoroutine1());
                    Debug.Log("after add co");
                }

                _coroutineManager.UpdateAllCoroutines();
            }

            // 피격시
            public override void OnHit()
            {
                GameSystem._Instance.SetScoreDelta(_score);
            }

            private IEnumerator TestCoroutine1()
            {
                Debug.Log("1-1 frame:" + GameSystem._Instance._Frame.ToString());
                yield return _coroutineManager.StartCoroutine(TestCoroutine2());
                Debug.Log("1-2 frame:" + GameSystem._Instance._Frame.ToString());
            }

            private IEnumerator TestCoroutine2()
            {
                Debug.Log("2-1 frame:" + GameSystem._Instance._Frame.ToString());
                yield return _coroutineManager.StartCoroutine(TestCoroutine3());
                Debug.Log("2-2 frame:" + GameSystem._Instance._Frame.ToString());
            }
            
            private IEnumerator TestCoroutine3()
            {
                Debug.Log("3-1 frame:" + GameSystem._Instance._Frame.ToString());
                yield return new WaitForFrames(5);
                Debug.Log("3-2 frame:" + GameSystem._Instance._Frame.ToString());
            }
        }
    }
}