﻿using UnityEngine;
using System.Collections;

namespace Game
{
    /// <summary>
    /// 음악별 다른 동작 정의
    /// </summary>
    public abstract class BaseGameLogic
    {
        // 특화 정보 로딩
        public abstract IEnumerator LoadContext();

        // 특화 정보 갱신
        public abstract void UpdatePlayContext();

        // 기본 플레이어기 로딩
        protected IEnumerator LoadBasicPlayer()
        {
            // 플레이어기 로딩 /////////////////
            GameSystem._Instance._UILoading.SetProgress("Loading Player");
            yield return null;
            GameSystem._Instance.PoolStackShape("Common/Player_Black", 1);
            GameSystem._Instance.PoolStackMover<PlayerAlive>(1);
            GameSystem._Instance.PoolStackShape("Common/Player_Crash", 1);
            GameSystem._Instance.PoolStackMover<PlayerCrash>(1);

            // 샷 로딩 ///////////////////////
            GameSystem._Instance._UILoading.SetProgress("Loading Shots");
            yield return null;
            GameSystem._Instance.PoolStackShape("Common/Shot_Black", 36);
            GameSystem._Instance.PoolStackMover<Shot>(36);
        }

        #region Util
        /// <summary>
        /// 지정한 좌표로부터 플레이어로 향하는 각도 구하기
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public float GetPlayerAngle(float x, float y)
        {
            // Atan2 의 결과가 라디안이므로 0~1로 변경
            Vector2 playerPos = GameSystem._Instance._Player._pos;
            return Mathf.Atan2(playerPos.y - y, playerPos.x - x) / Mathf.PI / 2.0f;
        }

        /// <summary>
        /// 지정한 무버로부터 플레이어로 향하는 각도 구하기
        /// </summary>
        /// <param name="startMover"></param>
        /// <returns></returns>
        public float GetPlayerAngle(Mover startMover)
        {
            return GetPlayerAngle(startMover._X, startMover._Y);
        }
        #endregion Util

        #region Pattern
        /// <summary>
        /// 무버를 지정한 위치까지 등속 이동
        /// </summary>
        public IEnumerator MoveConstantVelocity(Mover mover, Vector2 arrivePos, int duration)
        {
            Vector2 delta = (arrivePos - mover._pos) / (float)duration; // 한 프레임에 움직일 거리

            for (int i = 0; i < duration - 1; ++i)
            {
                mover._pos += delta;
                yield return null;
            }

            // 마지막 프레임
            mover._pos = arrivePos;
        }

        /// <summary>
        /// 무버를 지정한 위치까지 비례감속 이동
        /// </summary>
        public IEnumerator MoveDamp(Mover mover, Vector2 arrivePos, int duration, float damp)
        {
            for (int i = 0; i < duration - 1; ++i)
            {
                mover._pos = Vector2.Lerp(mover._pos, arrivePos, damp);
                yield return null;
            }

            // 마지막 프레임
            mover._pos = arrivePos;
        }

        /// <summary>
        /// N-Way 탄
        /// </summary>
        public void NWayBullet(Mover mover, string shape, float angle, float angleRange, float speed, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init(shape, mover._X, mover._Y, angle + angleRange * ((float)i / (count - 1) - 0.5f), 0.0f, speed, 0.0f);
            }
        }

        /// <summary>
        /// 무작위로 구멍 뚫린 N-Way
        /// </summary>
        public IEnumerator GapBullets(Mover mover, string shape, float angleRange, float speed, int count
            , int interval, int repeatCount)
        {
            for (int i = 0; i < repeatCount; ++i)
            {
                float angle = GameSystem._Instance.GetRandom01();
                NWayBullet(mover, shape, angle, angleRange, speed, count);
                yield return new WaitForFrames(interval);
            }
        }

        public IEnumerator CustomGapBullets(Mover mover, string shape, float angleRange, float speed, int count
            , int interval, float[] angles)
        {
            for (int i = 0; i < angles.Length; ++i)
            {
                NWayBullet(mover, shape, angles[i], angleRange, speed, count);
                yield return new WaitForFrames(interval);
            }
        }

        /// <summary>
        /// 원형탄
        /// </summary>
        public void CircleBullet(Mover mover, string shape, float angle, float speed, int count, bool halfAngleOffset)
        {
            float angleStart = angle + ((halfAngleOffset) ? (1.0f / count / 2.0f) : 0.0f);
            for (int i = 0; i < count; ++i)
            {
                Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init(shape, mover._X, mover._Y, angleStart + (1.0f / count * i), 0.0f, speed, 0.0f);
            }
        }

        public IEnumerator CircleBullets(Mover mover, string shape, float angle, float speed, int count, bool halfAngleOffset
            , int interval, int repeatCount)
        {
            for (int i = 0; i < repeatCount; ++i)
            {
                CircleBullet(mover, shape, angle, speed, count, halfAngleOffset);
                yield return new WaitForFrames(interval);
            }
        }

        /// <summary>
        /// 선회가속 원형탄
        /// </summary>
        public void BentCircleBullet(Mover mover, string shape, float angle, float speed, int count, float bulletAngleRate, float bulletSpeedRate, bool halfAngleOffset)
        {
            float angleStart = angle + ((halfAngleOffset) ? (1.0f / count / 2.0f) : 0.0f);
            for (int i = 0; i < count; ++i)
            {
                Bullet b = GameSystem._Instance.CreateBullet<Bullet>();
                b.Init(shape, mover._X, mover._Y, angleStart + (1.0f / count * i), bulletAngleRate, speed, bulletSpeedRate);
            }
        }

        public IEnumerator BentCircleBullets(Mover mover, string shape, float angle, float speed, int count, float bulletAngleRate, float bulletSpeedRate, bool halfAngleOffset
            , int interval, int repeatCount)
        {
            for (int i = 0; i < repeatCount; ++i)
            {
                BentCircleBullet(mover, shape, angle, speed, count, bulletAngleRate, bulletSpeedRate, halfAngleOffset);
                yield return new WaitForFrames(interval);
            }
        }
        #endregion Pattern
    }
}