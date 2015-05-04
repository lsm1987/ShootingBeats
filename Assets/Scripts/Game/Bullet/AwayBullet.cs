using UnityEngine;

namespace Game
{
    /// <summary>
    /// 지정한 프레임 이후에 플레어와 반대방향으로 이동하는 탄
    /// </summary>
    public class AwayBullet : Bullet
    {
        private int _awayFrame = 0;     // 반대방향으로 이동할 프레임
        private float _awaySpeed = 0.0f;// 반대방향으로 이동 시 속도 

        public void Init(string shapeSubPath, float x, float y, float angle
            , float angleRate, float speed, float speedRate
            , int waitDuration, float awaySpeed)
        {
            base.Init(shapeSubPath, x, y, angle, angleRate, speed, speedRate);
            _awayFrame = _Frame + waitDuration;
            _awaySpeed = awaySpeed;
        }

        public override void Move()
        {
            // 프레임 도달하면 속성 변경
            if (_Frame == _awayFrame)
            {
                _angle = GameSystem._Instance.GetLogic<BaseGameLogic>().GetPlayerAngle(this) + 0.5f;
                _speed = _awaySpeed;
            }
            base.Move();
        }
    }
}
