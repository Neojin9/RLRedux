using System;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace RogueCastle {
    public class EnemyObj_HomingTurret : EnemyObj {
        private float FireDelay = 5f;
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();
        private LogicBlock m_generalMiniBossLB = new LogicBlock();

        public EnemyObj_HomingTurret(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyHomingTurret_Character", target, physicsManager, levelToAttachTo, difficulty) {
            base.StopAnimation();
            base.ForceDraw = true;
            Type = 28;
            base.PlayAnimationOnRestart = false;
        }

        protected override void InitializeEV() {
            base.LockFlip = false;
            FireDelay = 2f;
            base.Name = "GuardBox";
            MaxHealth = 18;
            base.Damage = 20;
            base.XPValue = 75;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 1;
            MoneyDropChance = 0.4f;
            base.Speed = 0f;
            this.TurnSpeed = 10f;
            ProjectileSpeed = 775f;
            base.JumpHeight = 1035f;
            CooldownTime = 2f;
            base.AnimationDelay = 0.1f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = false;
            base.CanBeKnockedBack = true;
            base.IsWeighted = true;
            this.Scale = EnemyEV.HomingTurret_Basic_Scale;
            base.ProjectileScale = EnemyEV.HomingTurret_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.HomingTurret_Basic_Tint;
            MeleeRadius = 10;
            ProjectileRadius = 20;
            EngageRadius = 975;
            ProjectileDamage = base.Damage;
            base.KnockBack = EnemyEV.HomingTurret_Basic_KnockBack;
            InitialLogicDelay = 1f;
            switch (base.Difficulty) {
                case GameTypes.EnemyDifficulty.BASIC:
                    break;
                case GameTypes.EnemyDifficulty.ADVANCED:
                    FireDelay = 1.5f;
                    base.Name = "GuardBox XL";
                    MaxHealth = 25;
                    base.Damage = 26;
                    base.XPValue = 125;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    base.Speed = 0f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 1100f;
                    base.JumpHeight = 1035f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.HomingTurret_Advanced_Scale;
                    base.ProjectileScale = EnemyEV.HomingTurret_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.HomingTurret_Advanced_Tint;
                    MeleeRadius = 10;
                    EngageRadius = 975;
                    ProjectileRadius = 20;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.HomingTurret_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.EXPERT:
                    FireDelay = 2.25f;
                    base.Name = "GuardBox 2000";
                    MaxHealth = 42;
                    base.Damage = 30;
                    base.XPValue = 225;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 3;
                    MoneyDropChance = 1f;
                    base.Speed = 0f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 925f;
                    base.JumpHeight = 1035f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.HomingTurret_Expert_Scale;
                    base.ProjectileScale = EnemyEV.HomingTurret_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.HomingTurret_Expert_Tint;
                    MeleeRadius = 10;
                    ProjectileRadius = 20;
                    EngageRadius = 975;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.HomingTurret_Expert_KnockBack;
                    return;
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    base.Name = "GuardBox Gigasaur";
                    MaxHealth = 500;
                    base.Damage = 40;
                    base.XPValue = 750;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 4;
                    MoneyDropChance = 1f;
                    base.Speed = 0f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 900f;
                    base.JumpHeight = 1035f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.HomingTurret_Miniboss_Scale;
                    base.ProjectileScale = EnemyEV.HomingTurret_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.HomingTurret_Miniboss_Tint;
                    MeleeRadius = 10;
                    ProjectileRadius = 20;
                    EngageRadius = 975;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.HomingTurret_Miniboss_KnockBack;
                    return;
                default:
                    return;
            }
        }

        protected override void InitializeLogic() {
            float arg_06_0 = base.Rotation;
            float num = base.ParseTagToFloat("delay");
            float num2 = base.ParseTagToFloat("speed");
            if (num == 0f) {
                Console.WriteLine("ERROR: Turret set with delay of 0. Shoots too fast.");
                num = FireDelay;
            }
            if (num2 == 0f)
                num2 = ProjectileSpeed;
            ProjectileData projectileData = new ProjectileData(this) {
                SpriteName = "HomingProjectile_Sprite",
                SourceAnchor = new Vector2(35f, 0f),
                Speed = new Vector2(num2, num2),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = base.Damage,
                AngleOffset = 0f,
                CollidesWithTerrain = true,
                Scale = base.ProjectileScale,
                FollowArc = false,
                ChaseTarget = false,
                TurnSpeed = 0f,
                StartingRotation = 0f,
                Lifespan = 10f
            };
            LogicSet logicSet = new LogicSet(this);
            logicSet.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
            LogicSet logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new PlayAnimationLogicAction(false), Types.Sequence.Parallel);
            logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet2.AddAction(new RunFunctionLogicAction(this, "FireProjectileEffect", new object[0]), Types.Sequence.Serial);
            logicSet2.AddAction(new Play3DSoundLogicAction(this, m_target, new[] {
                "Turret_Attack01",
                "Turret_Attack02",
                "Turret_Attack03"
            }), Types.Sequence.Serial);
            logicSet2.AddAction(new DelayLogicAction(num, false), Types.Sequence.Serial);
            LogicSet logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new PlayAnimationLogicAction(false), Types.Sequence.Parallel);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet3.AddAction(new Play3DSoundLogicAction(this, m_target, new[] {
                "Turret_Attack01",
                "Turret_Attack02",
                "Turret_Attack03"
            }), Types.Sequence.Serial);
            logicSet3.AddAction(new DelayLogicAction(0.1f, false), Types.Sequence.Serial);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet3.AddAction(new Play3DSoundLogicAction(this, m_target, new[] {
                "Turret_Attack01",
                "Turret_Attack02",
                "Turret_Attack03"
            }), Types.Sequence.Serial);
            logicSet3.AddAction(new DelayLogicAction(0.1f, false), Types.Sequence.Serial);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet3.AddAction(new Play3DSoundLogicAction(this, m_target, new[] {
                "Turret_Attack01",
                "Turret_Attack02",
                "Turret_Attack03"
            }), Types.Sequence.Serial);
            logicSet3.AddAction(new RunFunctionLogicAction(this, "FireProjectileEffect", new object[0]), Types.Sequence.Serial);
            logicSet3.AddAction(new DelayLogicAction(num, false), Types.Sequence.Serial);
            LogicSet logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new PlayAnimationLogicAction(false), Types.Sequence.Parallel);
            projectileData.ChaseTarget = true;
            projectileData.Target = m_target;
            projectileData.TurnSpeed = 0.02f;
            logicSet4.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet4.AddAction(new Play3DSoundLogicAction(this, m_target, new[] {
                "Turret_Attack01",
                "Turret_Attack02",
                "Turret_Attack03"
            }), Types.Sequence.Serial);
            logicSet4.AddAction(new RunFunctionLogicAction(this, "FireProjectileEffect", new object[0]), Types.Sequence.Serial);
            logicSet4.AddAction(new DelayLogicAction(num, false), Types.Sequence.Serial);
            m_generalBasicLB.AddLogicSet(new LogicSet[] {
                logicSet2,
                logicSet
            });
            m_generalAdvancedLB.AddLogicSet(new LogicSet[] {
                logicSet3,
                logicSet
            });
            m_generalExpertLB.AddLogicSet(new LogicSet[] {
                logicSet4,
                logicSet
            });
            m_generalMiniBossLB.AddLogicSet(new LogicSet[] {
                logicSet2,
                logicSet
            });
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalMiniBossLB);
            projectileData.Dispose();
            base.InitializeLogic();
        }

        public void FireProjectileEffect() {
            Vector2 position = base.Position;
            if (Flip == SpriteEffects.None)
                position.X += 30f;
            else
                position.X -= 30f;
            m_levelScreen.ImpactEffectPool.TurretFireEffect(position, new Vector2(0.5f, 0.5f));
            m_levelScreen.ImpactEffectPool.TurretFireEffect(position, new Vector2(0.5f, 0.5f));
            m_levelScreen.ImpactEffectPool.TurretFireEffect(position, new Vector2(0.5f, 0.5f));
        }

        protected override void RunBasicLogic() {
            switch (base.State) {
                case 1:
                case 2:
                case 3: {
                    bool arg_34_1 = false;
                    LogicBlock arg_34_2 = m_generalBasicLB;
                    int[] array = new int[2];
                    array[0] = 100;
                    base.RunLogicBlock(arg_34_1, arg_34_2, array);
                    return;
                }
            }
            bool arg_4F_1 = false;
            LogicBlock arg_4F_2 = m_generalBasicLB;
            int[] array2 = new int[2];
            array2[0] = 100;
            base.RunLogicBlock(arg_4F_1, arg_4F_2, array2);
        }

        protected override void RunAdvancedLogic() {
            switch (base.State) {
                case 1:
                case 2:
                case 3: {
                    bool arg_34_1 = false;
                    LogicBlock arg_34_2 = m_generalAdvancedLB;
                    int[] array = new int[2];
                    array[0] = 100;
                    base.RunLogicBlock(arg_34_1, arg_34_2, array);
                    return;
                }
            }
            bool arg_4F_1 = false;
            LogicBlock arg_4F_2 = m_generalAdvancedLB;
            int[] array2 = new int[2];
            array2[0] = 100;
            base.RunLogicBlock(arg_4F_1, arg_4F_2, array2);
        }

        protected override void RunExpertLogic() {
            switch (base.State) {
                case 1:
                case 2:
                case 3: {
                    bool arg_34_1 = false;
                    LogicBlock arg_34_2 = m_generalExpertLB;
                    int[] array = new int[2];
                    array[0] = 100;
                    base.RunLogicBlock(arg_34_1, arg_34_2, array);
                    return;
                }
            }
            base.RunLogicBlock(false, m_generalExpertLB, new[] {
                0,
                100
            });
        }

        protected override void RunMinibossLogic() {
            switch (base.State) {
                case 1:
                case 2:
                case 3: {
                    bool arg_34_1 = false;
                    LogicBlock arg_34_2 = m_generalBasicLB;
                    int[] array = new int[2];
                    array[0] = 100;
                    base.RunLogicBlock(arg_34_1, arg_34_2, array);
                    return;
                }
            }
            bool arg_4F_1 = false;
            LogicBlock arg_4F_2 = m_generalBasicLB;
            int[] array2 = new int[2];
            array2[0] = 100;
            base.RunLogicBlock(arg_4F_1, arg_4F_2, array2);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }
    }
}
