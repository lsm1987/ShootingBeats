using UnityEngine;

namespace Game
{
    // 샷. 플레이어기가 발사하는 탄
    public class Shot : Bullet
    {
        public Shot()
            : base()
        {
        }
        
        public override void Move()
        {
            base.Move();

            // 살아있다면 추가동작
            if (_alive)
            {
                // 적기와 충돌 체크
                Enemy enemy = IsHit(GameSystem._Instance._Enemys);
                if (enemy != null)
                {
                    _alive = false; // 샷 소멸
                    enemy.OnHit();  // 적 피격 처리
                }
            }
        }
    }
}