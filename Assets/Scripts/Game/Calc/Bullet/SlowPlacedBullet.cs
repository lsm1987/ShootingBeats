using UnityEngine;

namespace Game
{
    namespace Calc
    {
        public class SlowPlacedBullet : PlacedBullet
        {
            private float _minSpeed = float.MinValue;
            private float _minSpeed2 = float.MinValue;

            public void Init(string shapeSubPath, float x, float y
                , float angle1, float angleRate1, float speed1, float speedRate1
                , int moveDuration, int stopDuration
                , float angle2, float angleRate2, float speed2, float speedRate2, float minSpeed2)
            {
                base.Init(shapeSubPath, x, y
                    , angle1, angleRate1, speed1, speedRate1
                    , moveDuration, stopDuration
                    , angle2, angleRate2, speed2, speedRate2);

                _minSpeed = float.MinValue;
                _minSpeed2 = minSpeed2;
            }

            protected override void OnMove2Started()
            {
                _minSpeed = _minSpeed2;
            }

            protected override void OnSpeedUpdated()
            {
                _speed = Mathf.Max(_speed, _minSpeed);
            }
        }
    }
}