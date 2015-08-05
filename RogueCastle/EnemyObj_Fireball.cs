using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Tweener;
using Tweener.Ease;


namespace RogueCastle {
    public class EnemyObj_Fireball : EnemyObj {
        private float DashDelay;
        private float DashDuration;
        private float DashSpeed;
        private Color FlameColour = Color.OrangeRed;
        private float m_MinibossProjectileLifspan = 11f;
        private float m_MinibossProjectileLifspanNeo = 20f;
        private int m_bossCoins = 30;
        private int m_bossDiamonds = 5;
        private int m_bossMoneyBags = 17;
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();
        private LogicBlock m_generalMiniBossLB = new LogicBlock();
        private LogicBlock m_generalNeoLB = new LogicBlock();
        private bool m_isNeo;
        private float m_minibossFireTime = 0.6f;
        private float m_minibossFireTimeCounter = 0.6f;
        private bool m_shake;
        private float m_shakeDuration = 0.03f;
        private float m_shakeTimer;
        private bool m_shookLeft;

        public EnemyObj_Fireball(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyGhostIdle_Character", target, physicsManager, levelToAttachTo, difficulty) {
            Type = 8;
            TintablePart = this._objectList[0];
        }

        public bool BossVersionKilled {
            get { return m_bossVersionKilled; }
        }

        public bool IsNeo {
            get { return m_isNeo; }
            set {
                m_isNeo = value;
                if (value) {
                    HealthGainPerLevel = 0;
                    DamageGainPerLevel = 0;
                    MoneyDropChance = 0f;
                    ItemDropChance = 0f;
                    m_saveToEnemiesKilledList = false;
                }
            }
        }

        protected override void InitializeEV() {
            DashDelay = 0.75f;
            DashDuration = 0.5f;
            DashSpeed = 900f;
            base.Name = "Charite";
            MaxHealth = 35;
            base.Damage = 23;
            base.XPValue = 100;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 2;
            MoneyDropChance = 0.4f;
            base.Speed = 400f;
            this.TurnSpeed = 0.0275f;
            ProjectileSpeed = 525f;
            base.JumpHeight = 950f;
            CooldownTime = 4f;
            base.AnimationDelay = 0.05f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = false;
            base.CanBeKnockedBack = true;
            base.IsWeighted = false;
            this.Scale = EnemyEV.Fireball_Basic_Scale;
            base.ProjectileScale = EnemyEV.Fireball_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Fireball_Basic_Tint;
            MeleeRadius = 500;
            ProjectileRadius = 700;
            EngageRadius = 1350;
            ProjectileDamage = base.Damage;
            base.KnockBack = EnemyEV.Fireball_Basic_KnockBack;
            switch (base.Difficulty) {
                case GameTypes.EnemyDifficulty.ADVANCED:
                    base.Name = "Pyrite";
                    MaxHealth = 45;
                    base.Damage = 25;
                    base.XPValue = 175;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    base.Speed = 420f;
                    this.TurnSpeed = 0.03f;
                    ProjectileSpeed = 525f;
                    base.JumpHeight = 950f;
                    CooldownTime = 4f;
                    base.AnimationDelay = 0.05f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = false;
                    this.Scale = EnemyEV.Fireball_Advanced_Scale;
                    base.ProjectileScale = EnemyEV.Fireball_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Fireball_Advanced_Tint;
                    MeleeRadius = 500;
                    EngageRadius = 1350;
                    ProjectileRadius = 700;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Fireball_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.EXPERT:
                    base.Name = "Infernite";
                    MaxHealth = 63;
                    base.Damage = 27;
                    base.XPValue = 350;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 3;
                    MoneyDropChance = 1f;
                    base.Speed = 440f;
                    this.TurnSpeed = 0.03f;
                    ProjectileSpeed = 525f;
                    base.JumpHeight = 950f;
                    CooldownTime = 4f;
                    base.AnimationDelay = 0.05f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = false;
                    this.Scale = EnemyEV.Fireball_Expert_Scale;
                    base.ProjectileScale = EnemyEV.Fireball_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Fireball_Expert_Tint;
                    MeleeRadius = 500;
                    ProjectileRadius = 700;
                    EngageRadius = 1350;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Fireball_Expert_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    base.Name = "Ponce de Leon";
                    MaxHealth = 505;
                    base.Damage = 29;
                    base.XPValue = 800;
                    MinMoneyDropAmount = 15;
                    MaxMoneyDropAmount = 20;
                    MoneyDropChance = 1f;
                    base.Speed = 380f;
                    this.TurnSpeed = 0.03f;
                    ProjectileSpeed = 0f;
                    base.JumpHeight = 950f;
                    CooldownTime = 4f;
                    base.AnimationDelay = 0.05f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = false;
                    base.IsWeighted = false;
                    this.Scale = EnemyEV.Fireball_Miniboss_Scale;
                    base.ProjectileScale = EnemyEV.Fireball_Miniboss_ProjectileScale;
                    MeleeRadius = 500;
                    ProjectileRadius = 775;
                    EngageRadius = 1350;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Fireball_Miniboss_KnockBack;
                    if (LevelEV.WEAKEN_BOSSES)
                        MaxHealth = 1;
                    break;
            }
            if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
                m_resetSpriteName = "EnemyGhostBossIdle_Character";
        }

        protected override void InitializeLogic() {
            LogicSet logicSet = new LogicSet(this);
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyGhostChase_Character", true, true), Types.Sequence.Serial);
            logicSet.AddAction(new ChaseLogicAction(m_target, true, 2f, -1f), Types.Sequence.Serial);
            LogicSet logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyGhostChase_Character", true, true), Types.Sequence.Serial);
            logicSet2.AddAction(new ChaseLogicAction(m_target, false, 1f, -1f), Types.Sequence.Serial);
            LogicSet logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyGhostIdle_Character", true, true), Types.Sequence.Serial);
            logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet3.AddAction(new DelayLogicAction(1f, 2f, false), Types.Sequence.Serial);
            LogicSet logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new MoveLogicAction(m_target, true, -1f), Types.Sequence.Serial);
            logicSet4.Tag = 2;
            LogicSet logicSet5 = new LogicSet(this);
            logicSet5.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyGhostDashPrep_Character", true, true), Types.Sequence.Serial);
            logicSet5.AddAction(new DelayLogicAction(DashDelay, false), Types.Sequence.Serial);
            logicSet5.AddAction(new RunFunctionLogicAction(this, "TurnToPlayer", null), Types.Sequence.Serial);
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyGhostDash_Character", true, true), Types.Sequence.Serial);
            logicSet5.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet5.AddAction(new MoveLogicAction(m_target, true, DashSpeed), Types.Sequence.Serial);
            logicSet5.AddAction(new DelayLogicAction(DashDuration, false), Types.Sequence.Serial);
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyGhostIdle_Character", true, true), Types.Sequence.Serial);
            logicSet5.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet5.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet5.AddAction(new DelayLogicAction(0.75f, false), Types.Sequence.Serial);
            logicSet5.Tag = 2;
            LogicSet logicSet6 = new LogicSet(this);
            logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyGhostDashPrep_Character", true, true), Types.Sequence.Serial);
            logicSet6.AddAction(new DelayLogicAction(DashDelay, false), Types.Sequence.Serial);
            logicSet6.AddAction(new RunFunctionLogicAction(this, "TurnToPlayer", null), Types.Sequence.Serial);
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyGhostDash_Character", true, true), Types.Sequence.Serial);
            ThrowProjectiles(logicSet6, true);
            logicSet6.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet6.AddAction(new MoveLogicAction(m_target, true, DashSpeed), Types.Sequence.Serial);
            logicSet6.AddAction(new DelayLogicAction(DashDuration, false), Types.Sequence.Serial);
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyGhostIdle_Character", true, true), Types.Sequence.Serial);
            logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet6.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet6.AddAction(new DelayLogicAction(0.75f, false), Types.Sequence.Serial);
            logicSet6.Tag = 2;
            LogicSet logicSet7 = new LogicSet(this);
            logicSet7.AddAction(new ChangeSpriteLogicAction("EnemyGhostBossIdle_Character", true, true), Types.Sequence.Serial);
            logicSet7.AddAction(new ChaseLogicAction(m_target, true, 2f, -1f), Types.Sequence.Serial);
            LogicSet logicSet8 = new LogicSet(this);
            logicSet8.AddAction(new ChangeSpriteLogicAction("EnemyGhostBossIdle_Character", true, true), Types.Sequence.Serial);
            logicSet8.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet8.AddAction(new DelayLogicAction(1f, 2f, false), Types.Sequence.Serial);
            LogicSet logicSet9 = new LogicSet(this);
            logicSet9.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet9.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet9.AddAction(new ChangeSpriteLogicAction("EnemyGhostBossDashPrep_Character", true, true), Types.Sequence.Serial);
            logicSet9.AddAction(new DelayLogicAction(DashDelay, false), Types.Sequence.Serial);
            logicSet9.AddAction(new RunFunctionLogicAction(this, "TurnToPlayer", null), Types.Sequence.Serial);
            logicSet9.AddAction(new ChangeSpriteLogicAction("EnemyGhostBossIdle_Character", true, true), Types.Sequence.Serial);
            logicSet9.AddAction(new RunFunctionLogicAction(this, "ChangeFlameDirection", new object[0]), Types.Sequence.Serial);
            logicSet9.AddAction(new MoveLogicAction(m_target, true, DashSpeed), Types.Sequence.Serial);
            logicSet9.AddAction(new DelayLogicAction(DashDuration, false), Types.Sequence.Serial);
            logicSet9.AddAction(new ChangePropertyLogicAction(this._objectList[0], "Rotation", 0), Types.Sequence.Serial);
            logicSet9.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet9.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet9.AddAction(new DelayLogicAction(0.75f, false), Types.Sequence.Serial);
            logicSet5.Tag = 2;
            m_generalBasicLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2,
                logicSet3,
                logicSet4,
                logicSet5
            });
            m_generalAdvancedLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2,
                logicSet3,
                logicSet4,
                logicSet5
            });
            m_generalExpertLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2,
                logicSet3,
                logicSet4,
                logicSet6
            });
            m_generalMiniBossLB.AddLogicSet(new LogicSet[] {
                logicSet7,
                logicSet2,
                logicSet8,
                logicSet4,
                logicSet9
            });
            m_generalNeoLB.AddLogicSet(new LogicSet[] {
                logicSet7,
                logicSet2,
                logicSet8,
                logicSet4,
                logicSet9
            });
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalMiniBossLB);
            logicBlocksToDispose.Add(m_generalNeoLB);
            LogicBlock arg_600_1 = m_generalBasicLB;
            int[] array = new int[5];
            array[0] = 100;
            base.SetCooldownLogicBlock(arg_600_1, array);
            base.InitializeLogic();
        }

        public void ChangeFlameDirection() {
            if (m_target.X < base.X) {
                this._objectList[0].Rotation = 90f;
                return;
            }
            this._objectList[0].Rotation = -90f;
        }

        private void ThrowProjectiles(LogicSet ls, bool useBossProjectile = false) {
            ProjectileData projectileData = new ProjectileData(this) {
                SpriteName = "GhostProjectile_Sprite",
                SourceAnchor = Vector2.Zero,
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = base.Damage,
                AngleOffset = 0f,
                Angle = new Vector2(0f, 0f),
                CollidesWithTerrain = false,
                Scale = base.ProjectileScale
            };
            if (useBossProjectile)
                projectileData.SpriteName = "GhostProjectile_Sprite";
            ls.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "FairyAttack1"
            }), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(60f, 60f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(30f, 30f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(120f, 120f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(150f, 150f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-60f, -60f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-30f, -30f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-120f, -120f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-150f, -150f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Dispose();
        }

        private void ThrowStandingProjectile(bool useBossProjectile = false) {
            ProjectileData projectileData = new ProjectileData(this) {
                SpriteName = "GhostProjectile_Sprite",
                SourceAnchor = Vector2.Zero,
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = base.Damage,
                AngleOffset = 0f,
                Angle = new Vector2(0f, 0f),
                CollidesWithTerrain = false,
                Scale = base.ProjectileScale,
                Lifespan = m_MinibossProjectileLifspan
            };
            if (IsNeo)
                projectileData.Lifespan = m_MinibossProjectileLifspanNeo;
            if (useBossProjectile)
                projectileData.SpriteName = "GhostBossProjectile_Sprite";
            ProjectileObj projectileObj = m_levelScreen.ProjectileManager.FireProjectile(projectileData);
            projectileObj.Rotation = 0f;
            if (IsNeo)
                projectileObj.TextureColor = Color.MediumSpringGreen;
            projectileData.Dispose();
        }

        protected override void RunBasicLogic() {
            switch (base.State) {
                case 0: {
                    bool arg_4E_1 = true;
                    LogicBlock arg_4E_2 = m_generalBasicLB;
                    int[] array = new int[5];
                    array[2] = 100;
                    base.RunLogicBlock(arg_4E_1, arg_4E_2, array);
                    return;
                }
                case 1:
                case 2:
                case 3: {
                    bool arg_33_1 = true;
                    LogicBlock arg_33_2 = m_generalBasicLB;
                    int[] array2 = new int[5];
                    array2[0] = 100;
                    base.RunLogicBlock(arg_33_1, arg_33_2, array2);
                    return;
                }
                default:
                    return;
            }
        }

        protected override void RunAdvancedLogic() {
            switch (base.State) {
                case 0: {
                    bool arg_6E_1 = true;
                    LogicBlock arg_6E_2 = m_generalAdvancedLB;
                    int[] array = new int[5];
                    array[2] = 100;
                    base.RunLogicBlock(arg_6E_1, arg_6E_2, array);
                    return;
                }
                case 1:
                case 2: {
                    bool arg_53_1 = true;
                    LogicBlock arg_53_2 = m_generalAdvancedLB;
                    int[] array2 = new int[5];
                    array2[0] = 100;
                    base.RunLogicBlock(arg_53_1, arg_53_2, array2);
                    return;
                }
                case 3:
                    base.RunLogicBlock(true, m_generalAdvancedLB, new[] {
                        40,
                        0,
                        0,
                        0,
                        60
                    });
                    return;
                default:
                    return;
            }
        }

        protected override void RunExpertLogic() {
            switch (base.State) {
                case 0: {
                    bool arg_6E_1 = true;
                    LogicBlock arg_6E_2 = m_generalExpertLB;
                    int[] array = new int[5];
                    array[2] = 100;
                    base.RunLogicBlock(arg_6E_1, arg_6E_2, array);
                    return;
                }
                case 1:
                case 2: {
                    bool arg_53_1 = true;
                    LogicBlock arg_53_2 = m_generalExpertLB;
                    int[] array2 = new int[5];
                    array2[0] = 100;
                    base.RunLogicBlock(arg_53_1, arg_53_2, array2);
                    return;
                }
                case 3:
                    base.RunLogicBlock(true, m_generalExpertLB, new[] {
                        40,
                        0,
                        0,
                        0,
                        60
                    });
                    return;
                default:
                    return;
            }
        }

        protected override void RunMinibossLogic() {
            if (!IsNeo) {
                switch (base.State) {
                    case 0: {
                        bool arg_76_1 = true;
                        LogicBlock arg_76_2 = m_generalMiniBossLB;
                        int[] array = new int[5];
                        array[0] = 100;
                        base.RunLogicBlock(arg_76_1, arg_76_2, array);
                        return;
                    }
                    case 1:
                    case 2: {
                        bool arg_5B_1 = true;
                        LogicBlock arg_5B_2 = m_generalMiniBossLB;
                        int[] array2 = new int[5];
                        array2[0] = 100;
                        base.RunLogicBlock(arg_5B_1, arg_5B_2, array2);
                        return;
                    }
                    case 3:
                        base.RunLogicBlock(true, m_generalMiniBossLB, new[] {
                            52,
                            0,
                            0,
                            0,
                            48
                        });
                        return;
                    default:
                        return;
                }
            }
            else {
                switch (base.State) {
                    case 0: {
                        bool arg_F6_1 = true;
                        LogicBlock arg_F6_2 = m_generalNeoLB;
                        int[] array3 = new int[5];
                        array3[0] = 100;
                        base.RunLogicBlock(arg_F6_1, arg_F6_2, array3);
                        return;
                    }
                    case 1:
                    case 2: {
                        bool arg_D8_1 = true;
                        LogicBlock arg_D8_2 = m_generalNeoLB;
                        int[] array4 = new int[5];
                        array4[0] = 100;
                        base.RunLogicBlock(arg_D8_1, arg_D8_2, array4);
                        return;
                    }
                    case 3:
                        base.RunLogicBlock(true, m_generalNeoLB, new[] {
                            45,
                            0,
                            0,
                            0,
                            55
                        });
                        return;
                    default:
                        return;
                }
            }
        }

        public override void Update(GameTime gameTime) {
            if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS && !base.IsPaused && m_minibossFireTimeCounter > 0f && !m_bossVersionKilled) {
                m_minibossFireTimeCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (m_minibossFireTimeCounter <= 0f) {
                    ThrowStandingProjectile(true);
                    m_minibossFireTimeCounter = m_minibossFireTime;
                }
            }
            if (m_shake && m_shakeTimer > 0f) {
                m_shakeTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (m_shakeTimer <= 0f) {
                    m_shakeTimer = m_shakeDuration;
                    if (m_shookLeft) {
                        m_shookLeft = false;
                        base.X += 5f;
                    }
                    else {
                        base.X -= 5f;
                        m_shookLeft = true;
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType) {
            if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS && !m_bossVersionKilled) {
                PlayerObj playerObj = otherBox.AbsParent as PlayerObj;
                if (playerObj != null && otherBox.Type == 1 && !playerObj.IsInvincible && playerObj.State == 8)
                    playerObj.HitPlayer(this);
            }
            if (collisionResponseType != 1)
                base.CollisionResponse(thisBox, otherBox, collisionResponseType);
        }

        public void TurnToPlayer() {
            float turnSpeed = this.TurnSpeed;
            this.TurnSpeed = 2f;
            CDGMath.TurnToFace(this, m_target.Position);
            this.TurnSpeed = turnSpeed;
        }

        public override void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer) {
            if (!m_bossVersionKilled)
                base.HitEnemy(damage, collisionPt, isPlayer);
        }

        public override void Kill(bool giveXP = true) {
            if (base.Difficulty != GameTypes.EnemyDifficulty.MINIBOSS) {
                base.Kill(giveXP);
                return;
            }
            if (m_target.CurrentHealth > 0) {
                Game.PlayerStats.FireballBossBeaten = true;
                SoundManager.StopMusic(0f);
                SoundManager.PlaySound("PressStart");
                m_bossVersionKilled = true;
                m_target.LockControls();
                m_levelScreen.PauseScreen();
                m_levelScreen.ProjectileManager.DestroyAllProjectiles(false);
                m_levelScreen.RunWhiteSlashEffect();
                Tween.RunFunction(1f, this, "Part2", new object[0]);
                SoundManager.PlaySound("Boss_Flash");
                SoundManager.PlaySound("Boss_Fireball_Freeze");
                GameUtil.UnlockAchievement("FEAR_OF_FIRE");
            }
        }

        public void Part2() {
            m_levelScreen.UnpauseScreen();
            m_target.UnlockControls();
            if (m_currentActiveLB != null)
                m_currentActiveLB.StopLogicBlock();
            base.PauseEnemy(true);
            this.ChangeSprite("EnemyGhostBossIdle_Character");
            base.PlayAnimation(true);
            m_target.CurrentSpeed = 0f;
            m_target.ForceInvincible = true;
            Tween.To(m_levelScreen.Camera, 0.5f, new Easing(Quad.EaseInOut), new[] {
                "X",
                base.X.ToString(),
                "Y",
                base.Y.ToString()
            });
            m_shake = true;
            m_shakeTimer = m_shakeDuration;
            for (int i = 0; i < 40; i++) {
                Vector2 vector = new Vector2((float)CDGMath.RandomInt(Bounds.Left, Bounds.Right), (float)CDGMath.RandomInt(Bounds.Top, Bounds.Bottom));
                Tween.RunFunction((float)i * 0.1f, typeof(SoundManager), "Play3DSound", new object[] {
                    this,
                    m_target,
                    new[] {
                        "Boss_Explo_01",
                        "Boss_Explo_02",
                        "Boss_Explo_03"
                    }
                });
                Tween.RunFunction((float)i * 0.1f, m_levelScreen.ImpactEffectPool, "DisplayExplosionEffect", new object[] {
                    vector
                });
            }
            Tween.AddEndHandlerToLastTween(this, "Part3", new object[0]);
            if (!IsNeo) {
                List<int> list = new List<int>();
                for (int j = 0; j < m_bossCoins; j++)
                    list.Add(0);
                for (int k = 0; k < m_bossMoneyBags; k++)
                    list.Add(1);
                for (int l = 0; l < m_bossDiamonds; l++)
                    list.Add(2);
                CDGMath.Shuffle<int>(list);
                float num = 2.5f / list.Count;
                for (int m = 0; m < list.Count; m++) {
                    Vector2 position = base.Position;
                    if (list[m] == 0) {
                        Tween.RunFunction((float)m * num, m_levelScreen.ItemDropManager, "DropItem", new object[] {
                            position,
                            1,
                            10
                        });
                    }
                    else if (list[m] == 1) {
                        Tween.RunFunction((float)m * num, m_levelScreen.ItemDropManager, "DropItem", new object[] {
                            position,
                            10,
                            100
                        });
                    }
                    else {
                        Tween.RunFunction((float)m * num, m_levelScreen.ItemDropManager, "DropItem", new object[] {
                            position,
                            11,
                            500
                        });
                    }
                }
            }
        }

        public void Part3() {
            SoundManager.PlaySound("Boss_Fireball_Death");
            m_levelScreen.ImpactEffectPool.DestroyFireballBoss(base.Position);
            base.Kill(true);
        }
    }
}
