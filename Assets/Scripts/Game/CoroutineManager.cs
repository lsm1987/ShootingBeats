/*
 * 자체 코루틴
 * http://wiki.unity3d.com/index.php/CoroutineScheduler 참고
 */

using System.Collections;

namespace Game
{
    // 코루틴 하나의 정보
    public class CoroutineNode
    {
        public CoroutineNode _listPrev = null;  // 관리자 내 코루틴 목록용
        public CoroutineNode _listNext = null;
        public IEnumerator _fiber;      // 수행할 작업
        public bool finished = false;   // 종료되었는가? 다른 코루틴 생성으로 yield 시, 생성된 코루틴의 종료여부 판단용
        public int waitForFrame = -1;   // 몇 프레임까지 대기할 것인가? -1: 미지정
        public CoroutineNode waitForCoroutine;  // 다른 코루틴 생성으로 대기중일 때

        public CoroutineNode(IEnumerator fiber_)
        {
            this._fiber = fiber_;
        }
    }

    #region YieldCommand
    // 코루틴이 yield 발생 시 어떤 동작을 할지 정의
    public abstract class YieldCommand
    {
    }

    // 지정한 프레임동안 대기
    public class WaitForFrames : YieldCommand
    {
        public int _frames;

        public WaitForFrames(int frames_)
        {
            _frames = frames_;
        }
    }

    // 지정한 절대 프레임까지 대기
    public class WaitForAbsFrames : YieldCommand
    {
        public int _frames;

        public WaitForAbsFrames(int frames_)
        {
            _frames = frames_;
        }
    }
    #endregion //YieldCommand

    // 코루틴들 관리
    public class CoroutineManager
    {
        private CoroutineNode _listFirst = null;    // 코루틴 목록 중 첫 요소
        private int _currentFrame { get { return GameSystem._Instance._Frame; } }  // 이번 업데이트가 호출된 프레임 번호

        /// <summary>
        /// 작업을 코루틴으로 등록한다.
        /// <para>등록 후 바로 한 번 업데이트 실행</para>
        /// <para>실행중이던 코루틴은 yield 문을 만나면 일시정지되며, yield 문의 리턴값은 코루틴어 언제 재개될지를 지정한다.</para>
        /// </summary>
        /// <param name="fiber">작업</param>
        /// <returns>등록된 코루틴</returns>
        public CoroutineNode StartCoroutine(IEnumerator fiber)
        {
            /*
             * 작업 함수가 yield 문을 갖고있지 않을 때는 (예: 바로 return null)
             * 여기서 즉시 실행된 후 목록에 추가되지 않는다.
             */
            if (fiber == null)
            {
                return null;
            }

            // UpdateAllCoroutine() 호출 전에 등록된다면 한 프레임에 2번 업데이트 될 수도 있음
            CoroutineNode coroutine = new CoroutineNode(fiber);
            AddCoroutine(coroutine);
            UpdateCoroutine(coroutine);
            return coroutine;
        }

        /// <summary>
        /// 작업을 코루틴으로 등록만 하고 다음 업데이트 호출시 실행
        /// </summary>
        public CoroutineNode RegisterCoroutine(IEnumerator fiber)
        {
            if (fiber == null)
            {
                return null;
            }

            CoroutineNode coroutine = new CoroutineNode(fiber);
            AddCoroutine(coroutine);
            return coroutine;
        }

        /// <summary>
        /// 모든 코루틴 강제로 끊기
        /// <para>이미 돌아가고 있던 코루틴의 정상종료가 불가능하므로 쓸 일 적음</para>
        /// </summary>
        public void StopAllCoroutines()
        {
            _listFirst = null;
        }

        /// <summary>
        /// 남아있는 코루틴이 있는가?
        /// </summary>
        /// <returns>남아있는지 여부</returns>
        public bool HasCoroutines()
        {
            return _listFirst != null;
        }

        /// <summary>
        /// 살아있는 모든 코루틴을 다음 yield 만날 떄 까지 업데이트한다.
        /// </summary>
        public void UpdateAllCoroutines()
        {
            CoroutineNode coroutine = this._listFirst;
            while (coroutine != null)
            {
                // UpdateCoroutine 도중에 현재 코루틴이 종료되어 다음 노드 정보가 사라질 수 있으므로 미리 백업
                CoroutineNode listNext = coroutine._listNext;

                if (coroutine.waitForFrame >= 0)
                {
                    if (_currentFrame >= coroutine.waitForFrame)
                    {
                        coroutine.waitForFrame = -1;
                        UpdateCoroutine(coroutine);
                    }
                }
                else if (coroutine.waitForCoroutine != null)
                {
                    if (coroutine.waitForCoroutine.finished)
                    {
                        coroutine.waitForCoroutine = null;
                        UpdateCoroutine(coroutine);
                    }
                }
                else
                {
                    UpdateCoroutine(coroutine);
                }
                coroutine = listNext;
            }
        }

        /// <summary>
        /// 코루틴이 yield 문을 만날 때 까지 진행시킨다.
        /// <para>코루틴이 끝났다면 종료 표시 후 목록에서 제거한다.</para>
        /// </summary>
        /// <param name="coroutine">실행할 코루틴</param>
        private void UpdateCoroutine(CoroutineNode coroutine)
        {
            IEnumerator fiber = coroutine._fiber;
            if (coroutine._fiber.MoveNext())
            {
                object yieldCommand = fiber.Current;

                if (yieldCommand == null)
                {
                    // yield return null 은 1프레임 대기와 동일
                    coroutine.waitForFrame = _currentFrame + 1;
                }
                else if (yieldCommand is WaitForFrames)
                {
                    WaitForFrames cmd = yieldCommand as WaitForFrames;
                    coroutine.waitForFrame = _currentFrame + cmd._frames;
                }
                else if (yieldCommand is WaitForAbsFrames)
                {
                    WaitForAbsFrames cmd = yieldCommand as WaitForAbsFrames;
                    coroutine.waitForFrame = cmd._frames;
                }
                else if (yieldCommand is CoroutineNode)
                {
                    // 새 코루틴 생성
                    coroutine.waitForCoroutine = yieldCommand as CoroutineNode;
                }
                else
                {
                    throw new System.ArgumentException("[CoroutineManager] Unexpected coroutine yield type: " + yieldCommand.GetType());
                }
            }
            else
            {
                // 코루틴 종료
                coroutine.finished = true;
                RemoveCoroutine(coroutine);
            }
        }

        /// <summary>
        /// 코루틴 목록에 추가
        /// <para>코루틴이 새 코루틴 yield 시, 새 코루틴을 먼저 업데이트하고 종료체크 할 수 있도록 목록 맨 앞에 추가</para>
        /// </summary>
        /// <param name="coroutine">추가할 코루틴</param>
        private void AddCoroutine(CoroutineNode coroutine)
        {
            if (_listFirst != null)
            {
                coroutine._listNext = _listFirst;
                _listFirst._listPrev = coroutine;
            }
            _listFirst = coroutine;
        }

        /// <summary>
        /// 코루틴을 목록에서 제거한다.
        /// <para>코루틴 인스턴스가 즉시 삭제되는 것은 아님</para>
        /// </summary>
        /// <param name="coroutine">제거할 코루틴</param>
        private void RemoveCoroutine(CoroutineNode coroutine)
        {
            if (this._listFirst == coroutine)
            {
                // 목록 처음이면 나의 다음 코루틴을 처음으로 지정
                this._listFirst = coroutine._listNext;
            }
            else
            {
                // 목록의 처음이 아님
                if (coroutine._listNext != null)
                {
                    // 다음 코루틴이 있다면 앞뒤 코루틴을 연결
                    coroutine._listPrev._listNext = coroutine._listNext;
                    coroutine._listNext._listPrev = coroutine._listPrev;
                }
                else if (coroutine._listPrev != null)
                {
                    // 마지막 코루틴임. 앞 코루틴을 마지막으로 만듦
                    coroutine._listPrev._listNext = null;
                }
            }

            // 다른 코루틴 참조 제거
            coroutine._listPrev = null;
            coroutine._listNext = null;
        }
    } // CoroutineManager
}
