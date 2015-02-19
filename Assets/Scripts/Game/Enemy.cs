using UnityEngine;

namespace Game
{
    // 적기
    // 세부 구현은 자식 클래스에서
    public abstract class Enemy : Mover
    {
        // 피격시
        public abstract void OnHit();
    }
}