using UnityEngine;
using System.Collections;

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
        protected abstract class BaseState : Common.FSMState<StateType>
        {
            // 상태가 영향을 미칠 게임
            protected GameSystem _gameSystem { get; private set; }

            public BaseState(GameSystem gameSystem_)
            {
                _gameSystem = gameSystem_;
            }
        }

        // 상태 관리
        protected class FSM : Common.FSM<StateType, BaseState>
        {
            // 무효한 상태
            protected override StateType _InvalidStateType
            {
                get { return StateType.Invalid; }
            }
        }

        // 로딩 상태
        protected class LoadState : BaseState
        {
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

                // 진입 시 로딩 시작
                _gameSystem.StartLoading();
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
                _gameSystem.StartPlay();
            }

            public override void OnUpdate()
            {
                base.OnUpdate();

                // 플레이 갱신
                _gameSystem.UpdatePlay();
            }
        }
    }
}