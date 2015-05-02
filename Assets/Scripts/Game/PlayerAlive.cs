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
                if (!GameSystem._Instance.IsPlayerInvincible())
                {
                    _alive = false;

                    // 게임에 알림
                    GameSystem._Instance.OnPlayerDied();

                    // 충돌 표시용 플레이어기 생성
                    GameSystem._Instance.PlaySoundEffect("PlayerExplosion");
                    PlayerCrash playerCrash = GameSystem._Instance.CreatePlayer<PlayerCrash>();
                    playerCrash.Init("Common/Player_Crash", _X, _Y, _angle);
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
                CreateShot(true, true);
                CreateShot(true, false);
                CreateShot(false, false);
                CreateShot(false, true);
            }

            // 카운터 갱신
            _shotCounter = (_shotCounter + 1) % _shotInterval;
        }

        private void CreateShot(bool left, bool diagonal)
        {
            Shot shot = GameSystem._Instance.CreateShot<Shot>();
            float x = _X + 0.05f * ((left) ? -1.0f : 1.0f);
            if (diagonal)
            {
                x += (left ? -0.025f : 0.025f);
            }
            float y = _Y + 0.05f;
            float angle = 0.25f;
            const float diagonalAngle = 0.01f;
            if (diagonal)
            {
                angle += (left ? diagonalAngle : -diagonalAngle);
            }
            // 대각선 방향 탄은 속도를 좀 더 빠르게 해야 수직탄과 같은 y위치를 가짐(v' = v / cos)
            // 하지만 diagonalAngle = 0.01, velocity = 0.03 이면 v' = 0.03006 으로 큰 차이 없으므로 보정하지 않음
            float velocity = 0.05f;
            shot.Init("Common/Shot_Black", x, y, angle
                , 0.0f, velocity, 0.0f);
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

            _X = Mathf.Clamp(_X + delta.x * moveRate, -mx, mx);
            _Y = Mathf.Clamp(_Y + delta.y * moveRate, -my, my);
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
            float x = _X + vx * speed;
            float y = _Y + vy * speed;
            x = Mathf.Clamp(x, -mx, mx);
            y = Mathf.Clamp(y, -my, my);

            // 변위
            float dx = x - _X;
            float dy = y - _Y;
            //Debug.Log("dx1:" + dx.ToString() + " dy1:" + dy.ToString());
            float d = Mathf.Sqrt(dx * dx + dy * dy);
            if (d > speed)
            {
                // x, y축으로 모두 이동 시 기본속도보다 빠르게 이동하지 않도록
                dx *= speed / d;
                dy *= speed / d;
            }

            // 실제 이동
            _X += dx;
            _Y += dy;
        }
    }
}