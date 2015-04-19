using UnityEngine;

namespace Game
{
    // 살아있는 플레이어기
    public class PlayerAlive : Player
    {
        private const int _shotInterval = 6;    // 몇 프레임 간격으로 샷 발사하는가?
        private int _shotCounter = 0; // 샷 발사 체크용 프레임 카운터

        public override void Init(string shapeSubPath, float x, float y, float angle)
        {
            base.Init(shapeSubPath, x, y, angle);
            _shotCounter = 0;
        }

        public override void Move()
        {
            TryShot();

            // 따라잡기가 아닌 프레임에서만 입력 적용
            if (!GameSystem._Instance._FrameByOver)
            {
                if (GameSystem._Instance._MoveInputArea.IsTouching())
                {
                    MoveByTouch();
                }
                else
                {
                    MoveByKey();
                }
            }

            // 탄, 적기와 충돌 체크
            if (IsHit(GameSystem._Instance._Bullets) != null || IsHit(GameSystem._Instance._Enemys) != null)
            {
                if (!GameSystem._Instance._isTestInvincible)
                {
                    _alive = false;

                    // 게임에 알림
                    GameSystem._Instance.OnPlayerDied();

                    // 충돌 표시용 플레이어기 생성
                    PlayerCrash playerCrash = GameSystem._Instance.CreatePlayer<PlayerCrash>();
                    playerCrash.Init("Common/Player_Crash", _x, _y, _angle);
                }
//                 else
//                 {
//                     Debug.Log("Player Crashed!");
//                 }
            }
        }

        /// <summary>
        /// 샷 발사 시도
        /// </summary>
        private void TryShot()
        {
            if (_shotCounter == 0)
            {
                CreateShot(true);
                CreateShot(false);
            }

            // 카운터 갱신
            _shotCounter = (_shotCounter + 1) % _shotInterval;
        }

        private void CreateShot(bool left)
        {
            Shot shot = GameSystem._Instance.CreateShot<Shot>();
            shot.Init("Common/Shot_Black", _x + 0.05f * ((left) ? -1.0f : 1.0f), _y + 0.05f, 0.25f
                , 0.0f, 0.03f, 0.0f);
        }

        private void MoveByTouch()
        {
            float moveRate = ((GlobalSystem._Instance != null)
                ? GlobalSystem._Instance._Config._MoveSensitivity
                : Config._moveSensitivityDefault)
                / 100.0f;
            Vector2 delta = GameSystem._Instance._MoveInputArea.GetDelta();

            // 이동경계
            float mx = GameSystem._Instance._MaxX - _shape._size;
            float my = GameSystem._Instance._MaxY - _shape._size;

            _x = Mathf.Clamp(_x + delta.x * moveRate, -mx, mx);
            _y = Mathf.Clamp(_y + delta.y * moveRate, -my, my);
        }

        private void MoveByKey()
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