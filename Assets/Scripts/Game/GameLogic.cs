using UnityEngine;
using System.Collections;

namespace Game
{
    /// <summary>
    /// 노래별 다른 동작을 분리
    /// </summary>
    public abstract class GameLogic
    {
        // 특화 정보 로딩
        public abstract IEnumerator LoadContext();

        // 특화 정보 갱신
        public abstract void UpdatePlayContext();
    }
}