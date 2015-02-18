using UnityEngine;

namespace Game
{
    // 플레이어기
    public class Player : Mover
    {
        public override void Move()
        {
            float speed = (Input.GetButton("Slow")) ? GameSystem._Instance._PlayerSlowSpeed : GameSystem._Instance._PlayerSpeed;
            // 입력
            float vx = Input.GetAxis("Horizontal");
            float vy = Input.GetAxis("Vertical");
            //Debug.Log("vx:" + vx.ToString() + " vy:" + vy.ToString());
            
            // 이동경계
            float mx = GameSystem._Instance._MaxX - _shape._size;
            float my = GameSystem._Instance._MaxY - _shape._size;
            
            // 이동하려는 위치
            float x = _x + vx * speed;
            float y = _y + vy * speed;
            x = Mathf.Clamp(x, -mx, mx);
            y = Mathf.Clamp(y, -my, my);
            
            // 변위
            float dx = x - _x;
            float dy = y - _y;
            //Debug.Log("dx1:" + dx.ToString() + " dy1:" + dy.ToString());
            float d = Mathf.Sqrt(dx * dx + dy * dy);
            if (d > speed)
            {
                // x, y축으로 모두 이동 시 기본속도보다 빠르게 이동하지 않도록
                dx *= speed / d;
                dy *= speed / d;
            }
            
            // 실제 이동
            _x += dx;
            _y += dy;
        }
    }
}