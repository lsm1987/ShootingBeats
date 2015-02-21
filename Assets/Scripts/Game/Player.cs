﻿using UnityEngine;

namespace Game
{
    // 플레이어기
    // 세부 구현은 자식 클래스에서
    public abstract class Player : Mover
    {
        // 가장 최근에 생성된 플레이어기를 활성화된 플레이어기로
        public override void Init(string shapeSubPath, float x, float y, float angle)
        {
            base.Init(shapeSubPath, x, y, angle);
            GameSystem._Instance._Player = this;
        }
    }
}