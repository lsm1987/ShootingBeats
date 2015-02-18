using UnityEngine;
using System.Collections;

namespace Game
{
    namespace RainbowSocialism
    {
        // "무지개빛 사회주의" 진행 정보
        public class GameSystem : Game.GameSystem
        {
            // 노래 경로
            protected override string _SongPath
            {
                get { return "Sounds/RainbowSocialism"; }
            }

            // 특화 정보 로딩
            protected override IEnumerator LoadContext()
            {
                // 적기 로딩 /////////////////////
                PoolStackShape("Common/Boss_Orange", 1);
                PoolStackMover<Boss>(1);
                yield return null;

                // 탄 로딩 ///////////////////
                // 외양 로딩
                PoolStackShape("Common/Bullet_Blue", 50);
                yield return null;
                PoolStackShape("Common/Bullet_Red", 50);
                yield return null;
                // 클래스 로딩
                PoolStackMover<Bullet>(100);
                yield return null;
            }

            // 특화 정보 갱신
            protected override void UpdatePlayContext()
            {
                if (_Frame == 0)
                {
                    Boss boss = CreateEnemy<Boss>();
                    boss.Init("Common/Boss_Orange", 0.0f, 1.0f, 0.0f);
                }
                if (_Frame == 64)
                {
                    AddTestNormalBullet();
                }
                else if (_Frame == 75)
                {
                    AddTestNormalBullet();
                }
                //
                else if (_Frame == 87)
                {
                    Bullet b = CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Blue", 0.0f, 1.0f, (0.625f + 0.25f / 6.0f * 1.0f)
                        , 0.0f, 0.03f, 0.0f);
                }
                else if (_Frame == 93)
                {
                    Bullet b = CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Blue", 0.0f, 1.0f, (0.625f + 0.25f / 6.0f * 2.0f)
                        , 0.0f, 0.03f, 0.0f);
                }
                else if (_Frame == 100)
                {
                    Bullet b = CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Blue", 0.0f, 1.0f, (0.625f + 0.25f / 6.0f * 3.0f)
                        , 0.0f, 0.03f, 0.0f);
                }
                else if (_Frame == 108)
                {
                    Bullet b = CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Blue", 0.0f, 1.0f, (0.625f + 0.25f / 6.0f * 4.0f)
                        , 0.0f, 0.03f, 0.0f);
                }
                else if (_Frame == 114)
                {
                    Bullet b = CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Blue", 0.0f, 1.0f, (0.625f + 0.25f / 6.0f * 5.0f)
                        , 0.0f, 0.03f, 0.0f);
                }
                else if (_Frame == 120)
                {
                    Bullet b = CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Blue", 0.0f, 1.0f, (0.625f + 0.25f / 6.0f * 6.0f)
                        , 0.0f, 0.03f, 0.0f);
                }
                //
                else if (_Frame == 64 + 84)
                {
                    AddTestNormalBullet();
                }
                else if (_Frame == 75 + 84)
                {
                    AddTestNormalBullet();
                }
                //
                else if (_Frame == 87 + 84)
                {
                    Bullet b = CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Blue", 0.0f, 1.0f, (0.875f - 0.25f / 6.0f * 1.0f)
                        , 0.0f, 0.03f, 0.0f);
                }
                else if (_Frame == 93 + 84)
                {
                    Bullet b = CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Blue", 0.0f, 1.0f, (0.875f - 0.25f / 6.0f * 2.0f)
                        , 0.0f, 0.03f, 0.0f);
                }
                else if (_Frame == 100 + 84)
                {
                    Bullet b = CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Blue", 0.0f, 1.0f, (0.875f - 0.25f / 6.0f * 3.0f)
                        , 0.0f, 0.03f, 0.0f);
                }
                else if (_Frame == 108 + 84)
                {
                    Bullet b = CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Blue", 0.0f, 1.0f, (0.875f - 0.25f / 6.0f * 4.0f)
                        , 0.0f, 0.03f, 0.0f);
                }
                else if (_Frame == 114 + 84)
                {
                    Bullet b = CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Blue", 0.0f, 1.0f, (0.875f - 0.25f / 6.0f * 5.0f)
                        , 0.0f, 0.03f, 0.0f);
                }
                else if (_Frame == 120 + 84)
                {
                    Bullet b = CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Blue", 0.0f, 1.0f, (0.875f - 0.25f / 6.0f * 6.0f)
                        , 0.0f, 0.03f, 0.0f);
                }
                //
                else if (_Frame == 249)
                {
                    Bullet b = CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Red", -0.2f, 1.0f, 0.75f
                        , 0.0f, 0.03f, 0.0f);
                    b = CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Red", 0.2f, 1.0f, 0.75f
                        , 0.0f, 0.03f, 0.0f);
                }
                else if (_Frame == 274)
                {
                    Bullet b = CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Red", -0.4f, 1.0f, 0.75f
                        , 0.0f, 0.03f, 0.0f);
                    b = CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Red", 0.4f, 1.0f, 0.75f
                        , 0.0f, 0.03f, 0.0f);
                }
                else if (_Frame == 312)
                {
                    Bullet b = CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Red", -0.6f, 1.0f, 0.75f
                        , 0.0f, 0.03f, 0.0f);
                    b = CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Red", 0.6f, 1.0f, 0.75f
                        , 0.0f, 0.03f, 0.0f);
                }
                else if (_Frame == 329)
                {
                    Bullet b = CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Red", -0.8f, 1.0f, 0.75f
                        , 0.0f, 0.03f, 0.0f);
                    b = CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Red", 0.8f, 1.0f, 0.75f
                        , 0.0f, 0.03f, 0.0f);
                }
                //
                else if (_Frame == 356)  // 1
                {
                    AddTestCircleBlueBullet();
                }
                else if (_Frame == 369)  // 3
                {
                    AddTestCircleRedBullet();
                }
                else if (_Frame == 380)  // 5
                {
                    AddTestCircleBlueBullet();
                }
                else if (_Frame == 391)  // 7
                {
                    AddTestCircleRedBullet();
                }
                else if (_Frame == 403)  // 9
                {
                    AddTestCircleBlueBullet();
                }
                else if (_Frame == 417)  // 11
                {
                    AddTestCircleRedBullet();
                }
                else if (_Frame == 429)  // 13
                {
                    AddTestCircleBlueBullet();
                }
                else if (_Frame == 441)  // 15
                {
                    AddTestCircleRedBullet();
                }
                else if (_Frame == 459)  // 17
                {
                    AddTestCircleBlueBullet();
                }
            }

            private void AddTestNormalBullet()
            {
                Bullet b = CreateBullet<Bullet>();
                b.Init("Common/Bullet_Blue", 0.0f, 1.0f, 0.75f
                    , 0.0f, 0.03f, 0.0f);
            }

            private void AddTestCircleBlueBullet()
            {
                const int count = 20;
                for (int i = 0; i < count; ++i)
                {
                    Bullet b = CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Blue", 0.0f, 0.5f, (1.0f / count * i)
                        , 0.0f, 0.01f, 0.0f);
                }
            }

            private void AddTestCircleRedBullet()
            {
                const int count = 20;
                for (int i = 0; i < count; ++i)
                {
                    Bullet b = CreateBullet<Bullet>();
                    b.Init("Common/Bullet_Red", 0.0f, 0.5f, (1.0f / count * i) + (1.0f / count / 2.0f)
                        , 0.0f, 0.01f, 0.0f);
                }
            }
        }
    }
}