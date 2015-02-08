using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    // GameSystem 의 FSM 관련 코드
    public partial class GameSystem
    {
        // 상태 타입
        public enum StateType
        {
            Invalid, Load, Play
        }

        // 상태 공통
        public abstract class BaseState
        {
            // 상태가 영향을 미칠 게임
            protected GameSystem _GameSystem { get; private set; }

            // 이 상태의 타입
            public abstract StateType _StateType { get; }

            // 생성자
            public BaseState(GameSystem gameSystem_)
            {
                _GameSystem = gameSystem_;
            }

            /// <summary>
            /// 상태 진입 후에 호출
            /// </summary>
            public virtual void OnEnter(StateType prevState)
            {
            }

            /// <summary>
            /// 상태 이탈 전에 호출
            /// </summary>
            public virtual void OnLeave(StateType nextState)
            {
            }

            /// <summary>
            /// 상태 갱신
            /// </summary>
            public virtual void OnUpdate()
            {
            }
        }

        // 상태 관리
        public class FSM
        {
            private Dictionary<StateType, BaseState> _states = new Dictionary<StateType, BaseState>();
            public BaseState _CurrentState { get; private set; }

            /// <summary>
            /// 생성자. 최초 무효 상태 추가
            /// </summary>
            public FSM(GameSystem gameSystem_)
            {
                InvalidState invalidState = new InvalidState(gameSystem_);
                AddState(invalidState);
                _CurrentState = invalidState;
            }

            /// <summary>
            /// 상태 추가
            /// </summary>
            public void AddState(BaseState state)
            {
                _states.Add(state._StateType, state);
            }

            /// <summary>
            /// 상태 전환
            /// </summary>
            public void SetState(StateType stateType)
            {
                BaseState nextState = null;
                _states.TryGetValue(stateType, out nextState);
                if (nextState == null)
                {
                    Debug.LogError("[FSM] Invalid StateType:" + stateType.ToString());
                    return;
                }

                // 현재 상태에서 빠져나감
                _CurrentState.OnLeave(stateType);
                BaseState prevState = _CurrentState;

                // 현재 상태 새로 지정
                _CurrentState = nextState;
                _CurrentState.OnEnter(prevState._StateType);
            }
        }

        // 무효 상태. FSM 초기화용
        protected class InvalidState : BaseState
        {
            public override StateType _StateType
            {
                get { return StateType.Invalid; }
            }

            public InvalidState(GameSystem gameSystem_)
                : base(gameSystem_)
            {
            }
        }

        // 로딩 상태
        protected class LoadState : BaseState
        {
            IEnumerator _loading = null;

            public override StateType _StateType
            {
                get { return StateType.Load; }
            }

            public LoadState(GameSystem gameSystem_)
                : base(gameSystem_)
            {
            }

            public override void OnEnter(StateType prevState)
            {
                base.OnEnter(prevState);

                // 진입 시 로딩 준비. 아직 실행되지는 않음
                _loading = _GameSystem.Loading();
            }

            public override void OnUpdate()
            {
                base.OnUpdate();

                if (_loading.MoveNext())
                {
                    // 아직 로딩 끝나지 않음
                }
                else
                {
                    // 로딩 끝남
                    _GameSystem._FSM.SetState(StateType.Play);
                }
            }
        }

        // 플레이 상태
        protected class PlayState : BaseState
        {
            public override StateType _StateType
            {
                get { return StateType.Play; }
            }

            public PlayState(GameSystem gameSystem_)
                : base(gameSystem_)
            {
            }

            public override void OnEnter(StateType prevState)
            {
                base.OnEnter(prevState);

                // 진입 시 플레이 시작
                _GameSystem.StartPlay();

                // 시작 직후 바로 0프레임째 업데이트 수행
                _GameSystem.Update();
            }

            public override void OnUpdate()
            {
                base.OnUpdate();

                // 플레이 갱신
                _GameSystem.UpdatePlay();
            }
        }
    }
}