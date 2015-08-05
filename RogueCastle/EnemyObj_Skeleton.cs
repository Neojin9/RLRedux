using System;
using DS2DEngine;
using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class EnemyObj_Skeleton : EnemyObj {
        private float AttackDelay = 0.1f;
        private float JumpDelay = 0.25f;
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalCooldownLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();
        private LogicBlock m_generalMiniBossLB = new LogicBlock();

        public EnemyObj_Skeleton(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemySkeletonIdle_Character", target, physicsManager, levelToAttachTo, difficulty) {
            Type = 15;
        }

        protected override void InitializeEV() {
            base.Name = "Skeleton";
            MaxHealth = 27;
            base.Damage = 20;
            base.XPValue = 100;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 2;
            MoneyDropChance = 0.4f;
            base.Speed = 80f;
            this.TurnSpeed = 10f;
            ProjectileSpeed = 1040f;
            base.JumpHeight = 925f;
            CooldownTime = 0.75f;
            base.AnimationDelay = 0.1f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = false;
            base.CanBeKnockedBack = true;
            base.IsWeighted = true;
            this.Scale = EnemyEV.Skeleton_Basic_Scale;
            base.ProjectileScale = EnemyEV.Skeleton_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Skeleton_Basic_Tint;
            MeleeRadius = 225;
            ProjectileRadius = 500;
            EngageRadius = 700;
            ProjectileDamage = base.Damage;
            base.KnockBack = EnemyEV.Skeleton_Basic_KnockBack;
            switch (base.Difficulty) {
                case GameTypes.EnemyDifficulty.BASIC:
                    break;
                case GameTypes.EnemyDifficulty.ADVANCED:
                    base.Name = "Mr Bones";
                    MaxHealth = 36;
                    base.Damage = 26;
                    base.XPValue = 150;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    base.Speed = 80f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 1040f;
                    base.JumpHeight = 925f;
                    CooldownTime = 0.45f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Skeleton_Advanced_Scale;
                    base.ProjectileScale = EnemyEV.Skeleton_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Skeleton_Advanced_Tint;
                    MeleeRadius = 225;
                    EngageRadius = 700;
                    ProjectileRadius = 500;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Skeleton_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.EXPERT:
                    base.Name = "McRib";
                    MaxHealth = 68;
                    base.Damage = 28;
                    base.XPValue = 200;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 3;
                    MoneyDropChance = 1f;
                    base.Speed = 140f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 1040f;
                    base.JumpHeight = 925f;
                    CooldownTime = 0.4f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Skeleton_Expert_Scale;
                    base.ProjectileScale = EnemyEV.Skeleton_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Skeleton_Expert_Tint;
                    MeleeRadius = 225;
                    ProjectileRadius = 500;
                    EngageRadius = 700;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Skeleton_Expert_KnockBack;
                    return;
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    base.Name = "Berith & Halphas";
                    MaxHealth = 255;
                    base.Damage = 32;
                    base.XPValue = 1000;
                    MinMoneyDropAmount = 11;
                    MaxMoneyDropAmount = 18;
                    MoneyDropChance = 1f;
                    base.Speed = 60f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 1040f;
                    base.JumpHeight = 925f;
                    CooldownTime = 0.15f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = false;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Skeleton_Miniboss_Scale;
                    base.ProjectileScale = EnemyEV.Skeleton_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Skeleton_Miniboss_Tint;
                    MeleeRadius = 225;
                    ProjectileRadius = 500;
                    EngageRadius = 700;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Skeleton_Miniboss_KnockBack;
                    return;
                default:
                    return;
            }
        }

        protected override void InitializeLogic() {
            ProjectileData projectileData = new ProjectileData(this) {
                SpriteName = "BoneProjectile_Sprite",
                SourceAnchor = new Vector2(20f, -20f),
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = true,
                RotationSpeed = 10f,
                Damage = base.Damage,
                AngleOffset = 0f,
                Angle = new Vector2(-72f, -72f),
                CollidesWithTerrain = false,
                Scale = base.ProjectileScale
            };
            LogicSet logicSet = new LogicSet(this);
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemySkeletonWalk_Character", true, true), Types.Sequence.Serial);
            logicSet.AddAction(new MoveLogicAction(m_target, true, -1f), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(0.2f, 0.75f, false), Types.Sequence.Serial);
            LogicSet logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemySkeletonWalk_Character", true, true), Types.Sequence.Serial);
            logicSet2.AddAction(new MoveLogicAction(m_target, false, -1f), Types.Sequence.Serial);
            logicSet2.AddAction(new DelayLogicAction(0.2f, 0.75f, false), Types.Sequence.Serial);
            LogicSet logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemySkeletonIdle_Character", true, true), Types.Sequence.Serial);
            logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet3.AddAction(new DelayLogicAction(0.2f, 0.75f, false), Types.Sequence.Serial);
            LogicSet logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new StopAnimationLogicAction(), Types.Sequence.Serial);
            logicSet4.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet4.AddAction(new DelayLogicAction(0.5f, 1f, false), Types.Sequence.Serial);
            LogicSet logicSet5 = new LogicSet(this);
            logicSet5.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet5.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false), Types.Sequence.Serial);
            logicSet5.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet5.AddAction(new DelayLogicAction(AttackDelay, false), Types.Sequence.Serial);
            logicSet5.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "SkeletonHit1"
            }), Types.Sequence.Serial);
            logicSet5.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet5.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
            logicSet5.AddAction(new DelayLogicAction(0.2f, 0.4f, false), Types.Sequence.Serial);
            logicSet5.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet5.Tag = 2;
            LogicSet logicSet6 = new LogicSet(this);
            logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet6.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false), Types.Sequence.Serial);
            logicSet6.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet6.AddAction(new DelayLogicAction(AttackDelay, false), Types.Sequence.Serial);
            logicSet6.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "SkeletonHit1"
            }), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-85f, -85f);
            logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet6.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
            logicSet6.AddAction(new DelayLogicAction(0.2f, 0.4f, false), Types.Sequence.Serial);
            logicSet6.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet6.Tag = 2;
            LogicSet logicSet7 = new LogicSet(this);
            logicSet7.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet7.AddAction(new ChangeSpriteLogicAction("EnemySkeletonJump_Character", false, false), Types.Sequence.Serial);
            logicSet7.AddAction(new PlayAnimationLogicAction(1, 3, false), Types.Sequence.Serial);
            logicSet7.AddAction(new DelayLogicAction(JumpDelay, false), Types.Sequence.Serial);
            logicSet7.AddAction(new JumpLogicAction(0f), Types.Sequence.Serial);
            logicSet7.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet7.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false), Types.Sequence.Serial);
            logicSet7.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet7.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "SkeletonHit1"
            }), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-72f, -72f);
            logicSet7.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet7.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
            logicSet7.AddAction(new DelayLogicAction(0.2f, 0.4f, false), Types.Sequence.Serial);
            logicSet7.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet7.Tag = 2;
            LogicSet logicSet8 = new LogicSet(this);
            logicSet8.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet8.AddAction(new ChangeSpriteLogicAction("EnemySkeletonJump_Character", false, false), Types.Sequence.Serial);
            logicSet8.AddAction(new PlayAnimationLogicAction(1, 3, false), Types.Sequence.Serial);
            logicSet8.AddAction(new DelayLogicAction(JumpDelay, false), Types.Sequence.Serial);
            logicSet8.AddAction(new JumpLogicAction(0f), Types.Sequence.Serial);
            logicSet8.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet8.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false), Types.Sequence.Serial);
            logicSet8.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet8.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "SkeletonHit1"
            }), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-85f, -85f);
            logicSet8.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet8.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
            logicSet8.AddAction(new DelayLogicAction(0.2f, 0.4f, false), Types.Sequence.Serial);
            logicSet8.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet8.Tag = 2;
            LogicSet logicSet9 = new LogicSet(this);
            logicSet9.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet9.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet9.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false), Types.Sequence.Serial);
            logicSet9.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet9.AddAction(new DelayLogicAction(AttackDelay, false), Types.Sequence.Serial);
            logicSet9.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "SkeletonHit1"
            }), Types.Sequence.Serial);
            ThrowThreeProjectiles(logicSet9);
            logicSet9.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Parallel);
            logicSet9.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
            ThrowThreeProjectiles(logicSet9);
            logicSet9.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
            ThrowThreeProjectiles(logicSet9);
            logicSet9.AddAction(new DelayLogicAction(0.2f, 0.4f, false), Types.Sequence.Serial);
            logicSet9.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet9.Tag = 2;
            LogicSet logicSet10 = new LogicSet(this);
            logicSet10.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet10.AddAction(new ChangeSpriteLogicAction("EnemySkeletonJump_Character", false, false), Types.Sequence.Serial);
            logicSet10.AddAction(new PlayAnimationLogicAction(1, 3, false), Types.Sequence.Serial);
            logicSet10.AddAction(new DelayLogicAction(JumpDelay, false), Types.Sequence.Serial);
            logicSet10.AddAction(new JumpLogicAction(0f), Types.Sequence.Serial);
            logicSet10.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet10.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false), Types.Sequence.Serial);
            logicSet10.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet10.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "SkeletonHit1"
            }), Types.Sequence.Serial);
            ThrowThreeProjectiles(logicSet10);
            logicSet10.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Parallel);
            logicSet10.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
            ThrowThreeProjectiles(logicSet10);
            logicSet10.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
            ThrowThreeProjectiles(logicSet10);
            logicSet10.AddAction(new DelayLogicAction(0.2f, 0.4f, false), Types.Sequence.Serial);
            logicSet10.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet10.Tag = 2;
            LogicSet logicSet11 = new LogicSet(this);
            logicSet11.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet11.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet11.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false), Types.Sequence.Serial);
            logicSet11.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet11.AddAction(new DelayLogicAction(AttackDelay, false), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-89f, -35f);
            projectileData.RotationSpeed = 8f;
            projectileData.SourceAnchor = new Vector2(5f, -20f);
            logicSet11.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "SkeletonHit1"
            }), Types.Sequence.Serial);
            logicSet11.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet11.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet11.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
            logicSet11.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet11.AddAction(new ChangeSpriteLogicAction("EnemySkeletonIdle_Character", true, true), Types.Sequence.Serial);
            logicSet11.AddAction(new DelayLogicAction(0.4f, 0.9f, false), Types.Sequence.Serial);
            LogicSet logicSet12 = new LogicSet(this);
            logicSet12.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet12.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet12.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false), Types.Sequence.Serial);
            logicSet12.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet12.AddAction(new DelayLogicAction(AttackDelay, false), Types.Sequence.Serial);
            logicSet12.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "SkeletonHit1"
            }), Types.Sequence.Serial);
            logicSet12.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet12.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet12.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet12.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet12.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet12.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
            logicSet12.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet12.AddAction(new DelayLogicAction(0.15f, 0.35f, false), Types.Sequence.Serial);
            logicSet12.AddAction(new ChangeSpriteLogicAction("EnemySkeletonIdle_Character", true, true), Types.Sequence.Serial);
            m_generalBasicLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2,
                logicSet3,
                logicSet5,
                logicSet6
            });
            m_generalAdvancedLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2,
                logicSet3,
                logicSet5,
                logicSet6,
                logicSet7,
                logicSet8
            });
            m_generalExpertLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2,
                logicSet3,
                logicSet9,
                logicSet10
            });
            m_generalMiniBossLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2,
                logicSet4,
                logicSet11,
                logicSet12
            });
            m_generalCooldownLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2,
                logicSet3
            });
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalMiniBossLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);
            base.SetCooldownLogicBlock(m_generalCooldownLB, new[] {
                30,
                30,
                40
            });
            projectileData.Dispose();
            base.InitializeLogic();
        }

        private void ThrowThreeProjectiles(LogicSet ls) {
            ProjectileData projectileData = new ProjectileData(this) {
                SpriteName = "BoneProjectile_Sprite",
                SourceAnchor = new Vector2(20f, -20f),
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = true,
                RotationSpeed = 10f,
                Damage = base.Damage,
                AngleOffset = 0f,
                Angle = new Vector2(-72f, -72f),
                CollidesWithTerrain = false,
                Scale = base.ProjectileScale
            };
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Speed = new Vector2(ProjectileSpeed - 350f, ProjectileSpeed - 350f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Speed = new Vector2(ProjectileSpeed + 350f, ProjectileSpeed + 350f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Dispose();
        }

        protected override void RunBasicLogic() {
            switch (base.State) {
                case 0:
                case 1: {
                    bool arg_91_1 = true;
                    LogicBlock arg_91_2 = m_generalBasicLB;
                    int[] array = new int[5];
                    array[0] = 40;
                    array[1] = 40;
                    array[2] = 20;
                    base.RunLogicBlock(arg_91_1, arg_91_2, array);
                    return;
                }
                case 2:
                    base.RunLogicBlock(true, m_generalBasicLB, new[] {
                        10,
                        10,
                        0,
                        40,
                        40
                    });
                    return;
                case 3:
                    base.RunLogicBlock(true, m_generalBasicLB, new[] {
                        10,
                        10,
                        0,
                        30,
                        50
                    });
                    return;
                default:
                    return;
            }
        }

        protected override void RunAdvancedLogic() {
            switch (base.State) {
                case 0:
                case 1: {
                    bool arg_72_1 = true;
                    LogicBlock arg_72_2 = m_generalAdvancedLB;
                    int[] array = new int[7];
                    array[0] = 40;
                    array[1] = 40;
                    array[2] = 20;
                    base.RunLogicBlock(arg_72_1, arg_72_2, array);
                    return;
                }
                case 2:
                case 3:
                    base.RunLogicBlock(true, m_generalAdvancedLB, new[] {
                        10,
                        10,
                        0,
                        15,
                        15,
                        25,
                        25
                    });
                    return;
                default:
                    RunBasicLogic();
                    return;
            }
        }

        protected override void RunExpertLogic() {
            switch (base.State) {
                case 0:
                case 1:
                    base.RunLogicBlock(true, m_generalExpertLB, new[] {
                        35,
                        35,
                        0,
                        0,
                        15
                    });
                    return;
                case 2:
                case 3:
                    base.RunLogicBlock(true, m_generalExpertLB, new[] {
                        15,
                        15,
                        0,
                        35,
                        35
                    });
                    return;
                default:
                    RunBasicLogic();
                    return;
            }
        }

        protected override void RunMinibossLogic() {
            switch (base.State) {
                case 0:
                case 1:
                case 2:
                case 3:
                    if (m_levelScreen.CurrentRoom.ActiveEnemies > 1) {
                        bool arg_4A_1 = true;
                        LogicBlock arg_4A_2 = m_generalMiniBossLB;
                        int[] array = new int[5];
                        array[2] = 10;
                        array[3] = 90;
                        base.RunLogicBlock(arg_4A_1, arg_4A_2, array);
                        return;
                    }
                    Console.WriteLine("RAGING");
                    base.RunLogicBlock(true, m_generalMiniBossLB, new[] {
                        0,
                        0,
                        10,
                        0,
                        90
                    });
                    return;
            }
        }

        public override void Update(GameTime gameTime) {
            if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS && m_levelScreen.CurrentRoom.ActiveEnemies == 1)
                TintablePart.TextureColor = new Color(185, 0, 15);
            base.Update(gameTime);
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer) {
            SoundManager.Play3DSound(this, Game.ScreenManager.Player, "SkeletonAttack1");
            base.HitEnemy(damage, position, isPlayer);
        }
    }
}
