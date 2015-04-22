using UnityEngine;
using System.Collections;

namespace Game
{
    /// <summary>
    /// 음악별 다른 동작 정의
    /// </summary>
    public abstract class BaseGameLogic
    {
        // 특화 정보 로딩
        public abstract IEnumerator LoadContext();

        // 특화 정보 갱신
        public abstract void UpdatePlayContext();

        // 기본 플레이어기 로딩
        protected IEnumerator LoadBasicPlayer()
        {
            // 플레이어기 로딩 /////////////////
            GameSystem._Instance._UILoading.SetProgress("Loading Player");
            yield return null;
            GameSystem._Instance.PoolStackShape("Common/Player_Black", 1);
            GameSystem._Instance.PoolStackMover<PlayerAlive>(1);
            GameSystem._Instance.PoolStackShape("Common/Player_Crash", 1);
            GameSystem._Instance.PoolStackMover<PlayerCrash>(1);

            // 샷 로딩 ///////////////////////
            GameSystem._Instance._UILoading.SetProgress("Loading Shots");
            yield return null;
            GameSystem._Instance.PoolStackShape("Common/Shot_Black", 36);
            GameSystem._Instance.PoolStackMover<Shot>(36);
        }
    }
}