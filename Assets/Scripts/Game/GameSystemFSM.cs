using UnityEngine;
using System.Collections;

namespace Game
{
    // 게임 시스템 진행 관리
    public enum GameSystemStateType
    {
        Invalid, Load, Play
    }

    // 상태 공통
    public abstract class GameSystemBaseState : FSMState<GameSystemStateType>
    {

    }

    // 상태 관리
    public class GameSystemFSM : FSM<GameSystemStateType, GameSystemBaseState>
    {
        // 무효한 상태
        protected override GameSystemStateType _InvalidStateType
        {
            get { return GameSystemStateType.Invalid; }
        }
    }

    // 로딩 상태
    public class GameSystemLoadState : GameSystemBaseState
    {
        public override GameSystemStateType _StateType
        {
            get { return GameSystemStateType.Load; }
        }

        public override void OnEnter(GameSystemStateType prevState)
        {
            base.OnEnter(prevState);

            // 진입 시 로딩 시작
            GameSystem.Instance.StartLoading();
        }
    }

    // 플레이 상태
    public class GameSystemPlayState : GameSystemBaseState
    {
        public override GameSystemStateType _StateType
        {
            get { return GameSystemStateType.Play; }
        }

        public override void OnEnter(GameSystemStateType prevState)
        {
            base.OnEnter(prevState);

            // 진입 시 플레이 시작
            GameSystem.Instance.StartPlay();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            // 플레이 갱신
            GameSystem.Instance.UpdatePlay();
        }
    }
}