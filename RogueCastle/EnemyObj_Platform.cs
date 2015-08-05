using DS2DEngine;
using Microsoft.Xna.Framework;
using Tweener;


namespace RogueCastle {
    public class EnemyObj_Platform : EnemyObj {
        private float RetractDelay;
        private bool m_blinkedWarning;
        private bool m_isExtended;
        private float m_retractCounter;

        public EnemyObj_Platform(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyPlatform_Character", target, physicsManager, levelToAttachTo, difficulty) {
            base.CollisionTypeTag = 1;
            Type = 27;
            base.CollidesBottom = false;
            base.CollidesLeft = false;
            base.CollidesRight = false;
            base.StopAnimation();
            base.PlayAnimationOnRestart = false;
            base.NonKillable = true;
            base.DisableCollisionBoxRotations = false;
        }

        protected override void InitializeEV() {
            this.Scale = new Vector2(2f, 2f);
            base.AnimationDelay = 0.0333333351f;
            base.Speed = 0f;
            MaxHealth = 999;
            EngageRadius = 30;
            ProjectileRadius = 20;
            MeleeRadius = 10;
            CooldownTime = 2f;
            base.KnockBack = new Vector2(1f, 2f);
            base.Damage = 25;
            base.JumpHeight = 20.5f;
            AlwaysFaceTarget = false;
            CanFallOffLedges = false;
            base.XPValue = 2;
            base.CanBeKnockedBack = false;
            base.LockFlip = true;
            base.IsWeighted = false;
            RetractDelay = 3f;
            base.Name = "Platform";
            switch (base.Difficulty) {
                case GameTypes.EnemyDifficulty.BASIC:
                case GameTypes.EnemyDifficulty.ADVANCED:
                case GameTypes.EnemyDifficulty.EXPERT:
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    if (Game.PlayerStats.Traits.X == 34f || Game.PlayerStats.Traits.Y == 34f) {
                        m_isExtended = true;
                        base.PlayAnimation("EndRetract", "EndRetract", false);
                    }
                    return;
            }
        }

        public override void Update(GameTime gameTime) {
            bool flag = false;
            if (Game.PlayerStats.Traits.X == 34f || Game.PlayerStats.Traits.Y == 34f)
                flag = true;
            if (!flag) {
                if (m_retractCounter > 0f) {
                    m_retractCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (m_retractCounter <= 1.5f && !m_blinkedWarning) {
                        m_blinkedWarning = true;
                        float num = 0f;
                        for (int i = 0; i < 10; i++) {
                            Tween.RunFunction(num, this, "Blink", new object[] {
                                Color.Red,
                                0.05f
                            });
                            num += 0.1f;
                        }
                    }
                    if (m_retractCounter <= 0f) {
                        m_isExtended = false;
                        base.PlayAnimation("StartExtract", "EndExtract", false);
                        SoundManager.Play3DSound(this, Game.ScreenManager.Player, new[] {
                            "Platform_Activate_03",
                            "Platform_Activate_04"
                        });
                    }
                }
            }
            else if (!m_isExtended) {
                m_isExtended = true;
                base.PlayAnimation("EndRetract", "EndRetract", false);
            }
            base.Update(gameTime);
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer = true) {
            if (m_target.IsAirAttacking && !m_isExtended && (base.CurrentFrame == 1 || base.CurrentFrame == base.TotalFrames)) {
                SoundManager.Play3DSound(this, Game.ScreenManager.Player, new[] {
                    "EnemyHit1",
                    "EnemyHit2",
                    "EnemyHit3",
                    "EnemyHit4",
                    "EnemyHit5",
                    "EnemyHit6"
                });
                base.Blink(Color.Red, 0.1f);
                m_levelScreen.ImpactEffectPool.DisplayEnemyImpactEffect(position);
                m_isExtended = true;
                m_blinkedWarning = false;
                base.PlayAnimation("StartRetract", "EndRetract", false);
                SoundManager.Play3DSound(this, Game.ScreenManager.Player, new[] {
                    "Platform_Activate_01",
                    "Platform_Activate_02"
                });
                m_retractCounter = RetractDelay;
                if (m_target.IsAirAttacking) {
                    m_target.IsAirAttacking = false;
                    m_target.AccelerationY = -m_target.AirAttackKnockBack;
                    m_target.NumAirBounces++;
                }
            }
        }

        public override void ResetState() {
            if (Game.PlayerStats.Traits.X == 34f || Game.PlayerStats.Traits.Y == 34f)
                return;
            base.PlayAnimation(1, 1, false);
            m_isExtended = false;
            m_blinkedWarning = false;
            m_retractCounter = 0f;
            base.ResetState();
        }

        public override void Reset() {
            if (Game.PlayerStats.Traits.X == 34f || Game.PlayerStats.Traits.Y == 34f)
                return;
            base.PlayAnimation(1, 1, false);
            m_isExtended = false;
            m_blinkedWarning = false;
            m_retractCounter = 0f;
            base.Reset();
            base.StopAnimation();
        }
    }
}
