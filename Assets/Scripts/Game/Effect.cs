using UnityEngine;

namespace Game
{
    // 이펙트
    public class Effect : Mover
    {
        private ShapeEffect _shapeEffect = null;
        
        public override void Init(string shapeSubPath, float x, float y, float angle)
        {
            base.Init(shapeSubPath, x, y, angle);
            _shapeEffect = _shape.GetComponent<ShapeEffect>();
            if (_shapeEffect != null)
            {
                _shapeEffect.Play();
            }
        }

        public override void Move()
        {
            if (_shapeEffect == null || !_shapeEffect.IsPlaying())
            {
                _alive = false;
            }
        }
    }
}