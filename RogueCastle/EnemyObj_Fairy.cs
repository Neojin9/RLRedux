using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Tweener;
using Tweener.Ease;


namespace RogueCastle {
    public class EnemyObj_Fairy : EnemyObj {
        private float AttackDelay = 0.5f;
        public int NumHits = 1;
        public RoomObj SpawnRoom;
        private int m_bossCoins = 30;
        private int m_bossDiamonds = 3;
        private int m_bossMoneyBags = 12;
        private Cue m_deathLoop;
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalCooldownLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();
        private LogicBlock m_generalMiniBossLB = new LogicBlock();
        private LogicBlock m_generalNeoLB = new LogicBlock();
        private bool m_isNeo;
        private int m_numSummons = 22;
        private bool m_playDeathLoop;
        private bool m_shake;
        private float m_shakeDuration = 0.03f;
        private float m_shakeTimer;
        private bool m_shookLeft;
        private float m_summonCounter = 6f;
        private float m_summonTimer = 6f;
        private float m_summonTimerNeo = 3f;

        public EnemyObj_Fairy(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyFairyGhostIdle_Character", target, physicsManager, levelToAttachTo, difficulty) {
            base.PlayAnimation(true);
            MainFairy = true;
            TintablePart = this._objectList[0];
            Type = 7;
        }

        public bool MainFairy { get; set; }
        public Vector2 SavedStartingPos { get; set; }

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
            base.Name = "Fury";
            MaxHealth = 27;
            base.Damage = 18;
            base.XPValue = 125;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 2;
            MoneyDropChance = 0.4f;
            base.Speed = 250f;
            this.TurnSpeed = 0.0325f;
            ProjectileSpeed = 475f;
            base.JumpHeight = 300f;
            CooldownTime = 1.75f;
            base.AnimationDelay = 0.1f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = true;
            base.CanBeKnockedBack = true;
            base.IsWeighted = false;
            this.Scale = EnemyEV.Fairy_Basic_Scale;
            base.ProjectileScale = EnemyEV.Fairy_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Fairy_Basic_Tint;
            MeleeRadius = 225;
            ProjectileRadius = 700;
            EngageRadius = 925;
            ProjectileDamage = base.Damage;
            base.KnockBack = EnemyEV.Fairy_Basic_KnockBack;
            switch (base.Difficulty) {
                case GameTypes.EnemyDifficulty.ADVANCED:
                    base.Name = "Rage";
                    MaxHealth = 37;
                    base.Damage = 22;
                    base.XPValue = 200;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    base.Speed = 265f;
                    this.TurnSpeed = 0.0325f;
                    ProjectileSpeed = 475f;
                    base.JumpHeight = 300f;
                    CooldownTime = 1.75f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = false;
                    this.Scale = EnemyEV.Fairy_Advanced_Scale;
                    base.ProjectileScale = EnemyEV.Fairy_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Fairy_Advanced_Tint;
                    MeleeRadius = 225;
                    EngageRadius = 925;
                    ProjectileRadius = 700;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Fairy_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.EXPERT:
                    base.Name = "Wrath";
                    MaxHealth = 72;
                    base.Damage = 24;
                    base.XPValue = 350;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 4;
                    MoneyDropChance = 1f;
                    base.Speed = 280f;
                    this.TurnSpeed = 0.0325f;
                    ProjectileSpeed = 475f;
                    base.JumpHeight = 300f;
                    CooldownTime = 2.5f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = false;
                    this.Scale = EnemyEV.Fairy_Expert_Scale;
                    base.ProjectileScale = EnemyEV.Fairy_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Fairy_Expert_Tint;
                    MeleeRadius = 225;
                    ProjectileRadius = 700;
                    EngageRadius = 925;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Fairy_Expert_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    base.Name = "Alexander";
                    MaxHealth = 635;
                    base.Damage = 30;
                    base.XPValue = 1000;
                    MinMoneyDropAmount = 15;
                    MaxMoneyDropAmount = 20;
                    MoneyDropChance = 1f;
                    base.Speed = 220f;
                    this.TurnSpeed = 0.0325f;
                    ProjectileSpeed = 545f;
                    base.JumpHeight = 300f;
                    CooldownTime = 3f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    base.CanBeKnockedBack = false;
                    base.IsWeighted = false;
                    this.Scale = new Vector2(2.5f, 2.5f);
                    base.ProjectileScale = new Vector2(2f, 2f);
                    MeleeRadius = 225;
                    ProjectileRadius = 775;
                    EngageRadius = 925;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Fairy_Miniboss_KnockBack;
                    NumHits = 1;
                    if (LevelEV.WEAKEN_BOSSES)
                        MaxHealth = 1;
                    break;
            }
            if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
                m_resetSpriteName = "EnemyFairyGhostBossIdle_Character";
        }

        protected override void InitializeLogic() {
            LogicSet logicSet = new LogicSet(this);
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostMove_Character", true, true), Types.Sequence.Serial);
            logicSet.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "FairyMove1",
                "FairyMove2",
                "FairyMove3"
            }), Types.Sequence.Serial);
            logicSet.AddAction(new ChaseLogicAction(m_target, true, 1f, -1f), Types.Sequence.Serial);
            LogicSet logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character", true, true), Types.Sequence.Serial);
            logicSet2.AddAction(new ChaseLogicAction(m_target, false, 0.5f, -1f), Types.Sequence.Serial);
            LogicSet logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet3.AddAction(new DelayLogicAction(0.5f, 0.75f, false), Types.Sequence.Serial);
            LogicSet logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character", true, true), Types.Sequence.Serial);
            logicSet4.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet4.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostShoot_Character", false, false), Types.Sequence.Serial);
            logicSet4.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet4.AddAction(new DelayLogicAction(AttackDelay, false), Types.Sequence.Serial);
            logicSet4.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", new object[] {
                false
            }), Types.Sequence.Serial);
            logicSet4.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character", true, true), Types.Sequence.Serial);
            logicSet4.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
            logicSet4.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet4.Tag = 2;
            LogicSet logicSet5 = new LogicSet(this);
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character", true, true), Types.Sequence.Serial);
            logicSet5.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet5.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostShoot_Character", false, false), Types.Sequence.Serial);
            logicSet5.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet5.AddAction(new DelayLogicAction(AttackDelay, false), Types.Sequence.Serial);
            logicSet5.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", new object[] {
                false
            }), Types.Sequence.Serial);
            logicSet5.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
            logicSet5.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", new object[] {
                false
            }), Types.Sequence.Serial);
            logicSet5.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
            logicSet5.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", new object[] {
                false
            }), Types.Sequence.Serial);
            logicSet5.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character", true, true), Types.Sequence.Serial);
            logicSet5.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
            logicSet5.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet5.Tag = 2;
            LogicSet logicSet6 = new LogicSet(this);
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character", true, true), Types.Sequence.Serial);
            logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet6.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostShoot_Character", false, false), Types.Sequence.Serial);
            logicSet6.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet6.AddAction(new DelayLogicAction(AttackDelay, false), Types.Sequence.Serial);
            ThrowEightProjectiles(logicSet6);
            logicSet6.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
            ThrowEightProjectiles(logicSet6);
            logicSet6.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
            ThrowEightProjectiles(logicSet6);
            logicSet6.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
            ThrowEightProjectiles(logicSet6);
            logicSet6.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character", true, true), Types.Sequence.Serial);
            logicSet6.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
            logicSet6.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet6.Tag = 2;
            LogicSet logicSet7 = new LogicSet(this);
            logicSet7.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossMove_Character", true, true), Types.Sequence.Serial);
            logicSet7.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "FairyMove1",
                "FairyMove2",
                "FairyMove3"
            }), Types.Sequence.Serial);
            logicSet7.AddAction(new ChaseLogicAction(m_target, true, 1f, -1f), Types.Sequence.Serial);
            LogicSet logicSet8 = new LogicSet(this);
            logicSet8.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossIdle_Character", true, true), Types.Sequence.Serial);
            logicSet8.AddAction(new ChaseLogicAction(m_target, false, 0.5f, -1f), Types.Sequence.Serial);
            LogicSet logicSet9 = new LogicSet(this);
            logicSet9.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet9.AddAction(new DelayLogicAction(0.5f, 0.75f, false), Types.Sequence.Serial);
            LogicSet logicSet10 = new LogicSet(this);
            logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossIdle_Character", true, true), Types.Sequence.Serial);
            logicSet10.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet10.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossShoot_Character", false, false), Types.Sequence.Serial);
            logicSet10.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossIdle_Character", true, true), Types.Sequence.Serial);
            logicSet10.AddAction(new DelayLogicAction(AttackDelay, false), Types.Sequence.Serial);
            logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossShoot_Character", false, false), Types.Sequence.Serial);
            logicSet10.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
            logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossMove_Character", true, true), Types.Sequence.Serial);
            logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", new object[] {
                true
            }), Types.Sequence.Serial);
            logicSet10.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
            logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", new object[] {
                true
            }), Types.Sequence.Serial);
            logicSet10.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
            logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", new object[] {
                true
            }), Types.Sequence.Serial);
            logicSet10.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
            logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", new object[] {
                true
            }), Types.Sequence.Serial);
            logicSet10.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
            logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", new object[] {
                true
            }), Types.Sequence.Serial);
            logicSet10.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
            logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", new object[] {
                true
            }), Types.Sequence.Serial);
            logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossIdle_Character", true, true), Types.Sequence.Serial);
            logicSet10.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
            logicSet10.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet10.Tag = 2;
            m_generalBasicLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2,
                logicSet3,
                logicSet4
            });
            m_generalAdvancedLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2,
                logicSet3,
                logicSet5
            });
            m_generalExpertLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2,
                logicSet3,
                logicSet6
            });
            m_generalMiniBossLB.AddLogicSet(new LogicSet[] {
                logicSet7,
                logicSet8,
                logicSet9,
                logicSet10
            });
            m_generalNeoLB.AddLogicSet(new LogicSet[] {
                logicSet7,
                logicSet8,
                logicSet9,
                logicSet10
            });
            if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS) {
                m_generalCooldownLB.AddLogicSet(new LogicSet[] {
                    logicSet7,
                    logicSet8,
                    logicSet9
                });
            }
            else {
                m_generalCooldownLB.AddLogicSet(new LogicSet[] {
                    logicSet,
                    logicSet2,
                    logicSet3
                });
            }
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalMiniBossLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);
            logicBlocksToDispose.Add(m_generalNeoLB);
            LogicBlock arg_975_1 = m_generalCooldownLB;
            int[] array = new int[3];
            array[0] = 70;
            array[1] = 30;
            base.SetCooldownLogicBlock(arg_975_1, array);
            base.InitializeLogic();
        }

        public void ThrowFourProjectiles(bool useBossProjectile) {
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
                projectileData.SpriteName = "GhostProjectileBoss_Sprite";
            if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS) {
                SoundManager.Play3DSound(this, Game.ScreenManager.Player, new[] {
                    "Boss_Flameskull_Roar_01",
                    "Boss_Flameskull_Roar_02",
                    "Boss_Flameskull_Roar_03"
                });
            }
            else
                SoundManager.Play3DSound(this, Game.ScreenManager.Player, "FairyAttack1");
            m_levelScreen.ProjectileManager.FireProjectile(projectileData);
            projectileData.Angle = new Vector2(90f, 90f);
            m_levelScreen.ProjectileManager.FireProjectile(projectileData);
            projectileData.Angle = new Vector2(180f, 180f);
            m_levelScreen.ProjectileManager.FireProjectile(projectileData);
            projectileData.Angle = new Vector2(-90f, -90f);
            m_levelScreen.ProjectileManager.FireProjectile(projectileData);
            projectileData.Dispose();
        }

        private void ThrowEightProjectiles(LogicSet ls) {
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
            ls.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "FairyAttack1"
            }), Types.Sequence.Serial);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(90f, 90f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(180f, 180f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-90f, -90f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            ls.AddAction(new DelayLogicAction(0.125f, false), Types.Sequence.Serial);
            ls.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "FairyAttack1"
            }), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(135f, 135f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(45f, 45f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-45f, -45f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-135f, -135f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Dispose();
        }

        protected override void RunBasicLogic() {
            switch (base.State) {
                case 0: {
                    bool arg_73_1 = true;
                    LogicBlock arg_73_2 = m_generalBasicLB;
                    int[] array = new int[4];
                    array[2] = 100;
                    base.RunLogicBlock(arg_73_1, arg_73_2, array);
                    return;
                }
                case 1: {
                    bool arg_58_1 = true;
                    LogicBlock arg_58_2 = m_generalBasicLB;
                    int[] array2 = new int[4];
                    array2[0] = 100;
                    base.RunLogicBlock(arg_58_1, arg_58_2, array2);
                    return;
                }
                case 2:
                case 3:
                    base.RunLogicBlock(true, m_generalBasicLB, new[] {
                        50,
                        10,
                        0,
                        40
                    });
                    return;
                default:
                    return;
            }
        }

        protected override void RunAdvancedLogic() {
            switch (base.State) {
                case 0: {
                    bool arg_73_1 = true;
                    LogicBlock arg_73_2 = m_generalAdvancedLB;
                    int[] array = new int[4];
                    array[2] = 100;
                    base.RunLogicBlock(arg_73_1, arg_73_2, array);
                    return;
                }
                case 1: {
                    bool arg_58_1 = true;
                    LogicBlock arg_58_2 = m_generalAdvancedLB;
                    int[] array2 = new int[4];
                    array2[0] = 100;
                    base.RunLogicBlock(arg_58_1, arg_58_2, array2);
                    return;
                }
                case 2:
                case 3:
                    base.RunLogicBlock(true, m_generalAdvancedLB, new[] {
                        50,
                        10,
                        0,
                        40
                    });
                    return;
                default:
                    return;
            }
        }

        protected override void RunExpertLogic() {
            switch (base.State) {
                case 0: {
                    bool arg_73_1 = true;
                    LogicBlock arg_73_2 = m_generalExpertLB;
                    int[] array = new int[4];
                    array[2] = 100;
                    base.RunLogicBlock(arg_73_1, arg_73_2, array);
                    return;
                }
                case 1: {
                    bool arg_58_1 = true;
                    LogicBlock arg_58_2 = m_generalExpertLB;
                    int[] array2 = new int[4];
                    array2[0] = 100;
                    base.RunLogicBlock(arg_58_1, arg_58_2, array2);
                    return;
                }
                case 2:
                case 3:
                    base.RunLogicBlock(true, m_generalExpertLB, new[] {
                        50,
                        10,
                        0,
                        40
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
                        bool arg_80_1 = true;
                        LogicBlock arg_80_2 = m_generalMiniBossLB;
                        int[] array = new int[4];
                        array[0] = 80;
                        array[1] = 20;
                        base.RunLogicBlock(arg_80_1, arg_80_2, array);
                        return;
                    }
                    case 1: {
                        bool arg_60_1 = true;
                        LogicBlock arg_60_2 = m_generalMiniBossLB;
                        int[] array2 = new int[4];
                        array2[0] = 100;
                        base.RunLogicBlock(arg_60_1, arg_60_2, array2);
                        return;
                    }
                    case 2:
                    case 3:
                        base.RunLogicBlock(true, m_generalMiniBossLB, new[] {
                            50,
                            10,
                            0,
                            40
                        });
                        return;
                    default:
                        return;
                }
            }
            else {
                switch (base.State) {
                    case 0: {
                        bool arg_10C_1 = true;
                        LogicBlock arg_10C_2 = m_generalNeoLB;
                        int[] array3 = new int[4];
                        array3[0] = 80;
                        array3[1] = 20;
                        base.RunLogicBlock(arg_10C_1, arg_10C_2, array3);
                        return;
                    }
                    case 1: {
                        bool arg_E8_1 = true;
                        LogicBlock arg_E8_2 = m_generalNeoLB;
                        int[] array4 = new int[4];
                        array4[0] = 100;
                        base.RunLogicBlock(arg_E8_1, arg_E8_2, array4);
                        return;
                    }
                    case 2:
                    case 3:
                        base.RunLogicBlock(true, m_generalNeoLB, new[] {
                            50,
                            10,
                            0,
                            40
                        });
                        return;
                    default:
                        return;
                }
            }
        }

        public override void Update(GameTime gameTime) {
            float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS && !base.IsPaused) {
                if (m_summonCounter > 0f) {
                    m_summonCounter -= num;
                    if (m_summonCounter <= 0f) {
                        if (IsNeo)
                            m_summonTimer = m_summonTimerNeo;
                        m_summonCounter = m_summonTimer;
                        NumHits--;
                        if (!base.IsKilled && NumHits <= 0 && m_levelScreen.CurrentRoom.ActiveEnemies <= m_numSummons + 1) {
                            if (Game.PlayerStats.TimesCastleBeaten <= 0 || IsNeo) {
                                CreateFairy(GameTypes.EnemyDifficulty.BASIC);
                                CreateFairy(GameTypes.EnemyDifficulty.BASIC);
                            }
                            else {
                                CreateFairy(GameTypes.EnemyDifficulty.ADVANCED);
                                CreateFairy(GameTypes.EnemyDifficulty.ADVANCED);
                            }
                            NumHits = 1;
                        }
                    }
                }
                RoomObj currentRoom = m_levelScreen.CurrentRoom;
                Rectangle bounds = Bounds;
                Rectangle bounds2 = currentRoom.Bounds;
                int num2 = bounds.Right - bounds2.Right;
                int num3 = bounds.Left - bounds2.Left;
                int num4 = bounds.Bottom - bounds2.Bottom;
                if (num2 > 0)
                    base.X -= (float)num2;
                if (num3 < 0)
                    base.X -= (float)num3;
                if (num4 > 0)
                    base.Y -= (float)num4;
            }
            if (m_shake && m_shakeTimer > 0f) {
                m_shakeTimer -= num;
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
            if (m_playDeathLoop && m_deathLoop != null && !m_deathLoop.IsPlaying)
                m_deathLoop = SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flameskull_Death_Loop");
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

        private void CreateFairy(GameTypes.EnemyDifficulty difficulty) {
            EnemyObj_Fairy enemyObj_Fairy = new EnemyObj_Fairy(null, null, null, difficulty);
            enemyObj_Fairy.Position = base.Position;
            enemyObj_Fairy.DropsItem = false;
            if (m_target.X < enemyObj_Fairy.X)
                enemyObj_Fairy.Orientation = MathHelper.ToRadians(0f);
            else
                enemyObj_Fairy.Orientation = MathHelper.ToRadians(180f);
            enemyObj_Fairy.Level = base.Level - 7 - 1;
            m_levelScreen.AddEnemyToCurrentRoom(enemyObj_Fairy);
            enemyObj_Fairy.PlayAnimation(true);
            enemyObj_Fairy.MainFairy = false;
            enemyObj_Fairy.SavedStartingPos = enemyObj_Fairy.Position;
            enemyObj_Fairy.SaveToFile = false;
            if (LevelEV.SHOW_ENEMY_RADII)
                enemyObj_Fairy.InitializeDebugRadii();
            enemyObj_Fairy.SpawnRoom = m_levelScreen.CurrentRoom;
            enemyObj_Fairy.GivesLichHealth = false;
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer) {
            if (!m_bossVersionKilled) {
                base.HitEnemy(damage, position, isPlayer);
                SoundManager.Play3DSound(this, Game.ScreenManager.Player, "SkeletonAttack1");
            }
        }

        public override void Kill(bool giveXP = true) {
            if (base.Difficulty != GameTypes.EnemyDifficulty.MINIBOSS) {
                base.Kill(giveXP);
                return;
            }
            if (m_target.CurrentHealth > 0) {
                Game.PlayerStats.FairyBossBeaten = true;
                SoundManager.StopMusic(0f);
                SoundManager.Play3DSound(this, Game.ScreenManager.Player, "PressStart");
                m_bossVersionKilled = true;
                m_target.LockControls();
                m_levelScreen.PauseScreen();
                m_levelScreen.ProjectileManager.DestroyAllProjectiles(false);
                m_levelScreen.RunWhiteSlashEffect();
                Tween.RunFunction(1f, this, "Part2", new object[0]);
                SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flash");
                SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flameskull_Freeze");
                GameUtil.UnlockAchievement("FEAR_OF_GHOSTS");
            }
        }

        public void Part2() {
            m_playDeathLoop = true;
            foreach (EnemyObj current in m_levelScreen.CurrentRoom.TempEnemyList) {
                if (!current.IsKilled)
                    current.Kill(true);
            }
            m_levelScreen.UnpauseScreen();
            m_target.UnlockControls();
            if (m_currentActiveLB != null)
                m_currentActiveLB.StopLogicBlock();
            base.PauseEnemy(true);
            this.ChangeSprite("EnemyFairyGhostBossShoot_Character");
            base.PlayAnimation(true);
            m_target.CurrentSpeed = 0f;
            m_target.ForceInvincible = true;
            if (IsNeo)
                m_target.InvincibleToSpikes = true;
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
            m_playDeathLoop = false;
            SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flameskull_Death");
            if (m_deathLoop != null && m_deathLoop.IsPlaying)
                m_deathLoop.Stop(AudioStopOptions.Immediate);
            m_levelScreen.RunWhiteSlash2();
            base.Kill(true);
        }

        public override void Reset() {
            if (!MainFairy) {
                m_levelScreen.RemoveEnemyFromRoom(this, SpawnRoom, SavedStartingPos);
                Dispose();
                return;
            }
            NumHits = 1;
            base.Reset();
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                SpawnRoom = null;
                if (m_deathLoop != null && !m_deathLoop.IsDisposed)
                    m_deathLoop.Dispose();
                m_deathLoop = null;
                base.Dispose();
            }
        }
    }
}
