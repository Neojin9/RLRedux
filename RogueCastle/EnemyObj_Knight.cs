using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class EnemyObj_Knight : EnemyObj {
        private float AttackProjectileDelay = 0.35f;
        private float AttackProjectileExpertDelay = 0.425f;
        private float AttackProjectileMinibossDelay = 0.5f;
        private float AttackThrustDelay = 0.65f;
        private float AttackThrustDelayExpert = 0.65f;
        private float AttackThrustDelayMiniBoss = 0.65f;
        private float AttackThrustDuration = 0.4f;
        private float AttackThrustDurationExpert = 0.25f;
        private float AttackThrustDurationMiniBoss = 0.25f;
        private float AttackThrustSpeed = 1850f;
        private float AttackThrustSpeedExpert = 1750f;
        private float AttackThrustSpeedMiniBoss = 2300f;
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalCooldownLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();
        private LogicBlock m_generalMiniBossLB = new LogicBlock();
        private FrameSoundObj m_walkSound;
        private FrameSoundObj m_walkSound2;

        public EnemyObj_Knight(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemySpearKnightIdle_Character", target, physicsManager, levelToAttachTo, difficulty) {
            TintablePart = this._objectList[1];
            Type = 12;
            m_walkSound = new FrameSoundObj(this, m_target, 1, new[] {
                "KnightWalk1",
                "KnightWalk2"
            });
            m_walkSound2 = new FrameSoundObj(this, m_target, 6, new[] {
                "KnightWalk1",
                "KnightWalk2"
            });
        }

        protected override void InitializeEV() {
            base.Name = "Corrupt Knight";
            MaxHealth = 40;
            base.Damage = 27;
            base.XPValue = 125;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 2;
            MoneyDropChance = 0.4f;
            base.Speed = 75f;
            this.TurnSpeed = 10f;
            ProjectileSpeed = 860f;
            base.JumpHeight = 950f;
            CooldownTime = 0.75f;
            base.AnimationDelay = 0.1f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = false;
            base.CanBeKnockedBack = true;
            base.IsWeighted = true;
            this.Scale = EnemyEV.Knight_Basic_Scale;
            base.ProjectileScale = EnemyEV.Knight_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Knight_Basic_Tint;
            MeleeRadius = 325;
            ProjectileRadius = 690;
            EngageRadius = 850;
            ProjectileDamage = base.Damage;
            base.KnockBack = EnemyEV.Knight_Basic_KnockBack;
            switch (base.Difficulty) {
                case GameTypes.EnemyDifficulty.BASIC:
                    break;
                case GameTypes.EnemyDifficulty.ADVANCED:
                    base.Name = "Corrupt Vanguard";
                    MaxHealth = 58;
                    base.Damage = 32;
                    base.XPValue = 185;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    base.Speed = 75f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 860f;
                    base.JumpHeight = 950f;
                    CooldownTime = 0.5f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Knight_Advanced_Scale;
                    base.ProjectileScale = EnemyEV.Knight_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Knight_Advanced_Tint;
                    MeleeRadius = 325;
                    EngageRadius = 850;
                    ProjectileRadius = 690;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Knight_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.EXPERT:
                    base.Name = "Corrupt Lord";
                    MaxHealth = 79;
                    base.Damage = 36;
                    base.XPValue = 250;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 4;
                    MoneyDropChance = 1f;
                    base.Speed = 125f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 780f;
                    base.JumpHeight = 950f;
                    CooldownTime = 0.5f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Knight_Expert_Scale;
                    base.ProjectileScale = EnemyEV.Knight_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Knight_Expert_Tint;
                    MeleeRadius = 325;
                    ProjectileRadius = 690;
                    EngageRadius = 850;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Knight_Expert_KnockBack;
                    return;
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    base.Name = "Botis";
                    MaxHealth = 265;
                    base.Damage = 40;
                    base.XPValue = 1250;
                    MinMoneyDropAmount = 11;
                    MaxMoneyDropAmount = 18;
                    MoneyDropChance = 1f;
                    base.Speed = 200f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 780f;
                    base.JumpHeight = 1350f;
                    CooldownTime = 0.65f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Knight_Miniboss_Scale;
                    base.ProjectileScale = EnemyEV.Knight_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Knight_Miniboss_Tint;
                    MeleeRadius = 325;
                    ProjectileRadius = 690;
                    EngageRadius = 850;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Knight_Miniboss_KnockBack;
                    return;
                default:
                    return;
            }
        }

        protected override void InitializeLogic() {
            LogicSet logicSet = new LogicSet(this);
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightWalk_Character", true, true), Types.Sequence.Serial);
            logicSet.AddAction(new MoveLogicAction(m_target, true, -1f), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(0.2f, 1f, false), Types.Sequence.Serial);
            LogicSet logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightWalk_Character", true, true), Types.Sequence.Serial);
            logicSet2.AddAction(new MoveLogicAction(m_target, false, -1f), Types.Sequence.Serial);
            logicSet2.AddAction(new DelayLogicAction(0.2f, 1f, false), Types.Sequence.Serial);
            LogicSet logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightIdle_Character", true, true), Types.Sequence.Serial);
            logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet3.AddAction(new DelayLogicAction(0.2f, 1f, false), Types.Sequence.Serial);
            LogicSet logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet4.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack_Character", true, true), Types.Sequence.Serial);
            logicSet4.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet4.AddAction(new DelayLogicAction(AttackThrustDelay, false), Types.Sequence.Serial);
            logicSet4.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "SpearKnightAttack1"
            }), Types.Sequence.Serial);
            logicSet4.AddAction(new MoveDirectionLogicAction(AttackThrustSpeed), Types.Sequence.Serial);
            logicSet4.AddAction(new RunFunctionLogicAction(m_levelScreen.ImpactEffectPool, "DisplayThrustDustEffect", new object[] {
                this,
                20,
                0.3f
            }), Types.Sequence.Serial);
            logicSet4.AddAction(new PlayAnimationLogicAction("AttackStart", "End", false), Types.Sequence.Parallel);
            logicSet4.AddAction(new DelayLogicAction(AttackThrustDuration, false), Types.Sequence.Serial);
            logicSet4.AddAction(new MoveLogicAction(null, true, 0f), Types.Sequence.Serial);
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightIdle_Character", true, true), Types.Sequence.Serial);
            logicSet4.AddAction(new DelayLogicAction(0.3f, false), Types.Sequence.Serial);
            logicSet4.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet4.Tag = 2;
            LogicSet logicSet5 = new LogicSet(this);
            logicSet5.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet5.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack_Character", true, true), Types.Sequence.Serial);
            logicSet5.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet5.AddAction(new DelayLogicAction(AttackThrustDelayExpert, false), Types.Sequence.Serial);
            logicSet5.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "SpearKnightAttack1"
            }), Types.Sequence.Serial);
            logicSet5.AddAction(new MoveDirectionLogicAction(AttackThrustSpeedExpert), Types.Sequence.Serial);
            logicSet5.AddAction(new RunFunctionLogicAction(m_levelScreen.ImpactEffectPool, "DisplayThrustDustEffect", new object[] {
                this,
                20,
                0.3f
            }), Types.Sequence.Serial);
            logicSet5.AddAction(new PlayAnimationLogicAction("AttackStart", "End", false), Types.Sequence.Parallel);
            logicSet5.AddAction(new DelayLogicAction(AttackThrustDurationExpert, false), Types.Sequence.Serial);
            logicSet5.AddAction(new MoveLogicAction(null, true, 0f), Types.Sequence.Serial);
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightIdle_Character", true, true), Types.Sequence.Serial);
            logicSet5.AddAction(new DelayLogicAction(0.3f, false), Types.Sequence.Serial);
            logicSet5.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet5.Tag = 2;
            LogicSet logicSet6 = new LogicSet(this);
            logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet6.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack_Character", true, true), Types.Sequence.Serial);
            logicSet6.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet6.AddAction(new DelayLogicAction(AttackThrustDelayMiniBoss, false), Types.Sequence.Serial);
            logicSet6.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "SpearKnightAttack1"
            }), Types.Sequence.Serial);
            logicSet6.AddAction(new MoveDirectionLogicAction(AttackThrustSpeedMiniBoss), Types.Sequence.Serial);
            logicSet6.AddAction(new RunFunctionLogicAction(m_levelScreen.ImpactEffectPool, "DisplayThrustDustEffect", new object[] {
                this,
                20,
                0.3f
            }), Types.Sequence.Serial);
            logicSet6.AddAction(new PlayAnimationLogicAction("AttackStart", "End", false), Types.Sequence.Parallel);
            logicSet6.AddAction(new DelayLogicAction(AttackThrustDurationMiniBoss, false), Types.Sequence.Serial);
            logicSet6.AddAction(new MoveLogicAction(null, true, 0f), Types.Sequence.Serial);
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightIdle_Character", true, true), Types.Sequence.Serial);
            logicSet6.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet6.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack_Character", true, true), Types.Sequence.Serial);
            logicSet6.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet6.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
            logicSet6.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "SpearKnightAttack1"
            }), Types.Sequence.Serial);
            logicSet6.AddAction(new MoveDirectionLogicAction(AttackThrustSpeedMiniBoss), Types.Sequence.Serial);
            logicSet6.AddAction(new RunFunctionLogicAction(m_levelScreen.ImpactEffectPool, "DisplayThrustDustEffect", new object[] {
                this,
                20,
                0.3f
            }), Types.Sequence.Serial);
            logicSet6.AddAction(new PlayAnimationLogicAction("AttackStart", "End", false), Types.Sequence.Parallel);
            logicSet6.AddAction(new DelayLogicAction(AttackThrustDurationMiniBoss, false), Types.Sequence.Serial);
            logicSet6.AddAction(new MoveLogicAction(null, true, 0f), Types.Sequence.Serial);
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightIdle_Character", true, true), Types.Sequence.Serial);
            logicSet6.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet6.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack_Character", true, true), Types.Sequence.Serial);
            logicSet6.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet6.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
            logicSet6.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "SpearKnightAttack1"
            }), Types.Sequence.Serial);
            logicSet6.AddAction(new MoveDirectionLogicAction(AttackThrustSpeedMiniBoss), Types.Sequence.Serial);
            logicSet6.AddAction(new RunFunctionLogicAction(m_levelScreen.ImpactEffectPool, "DisplayThrustDustEffect", new object[] {
                this,
                20,
                0.3f
            }), Types.Sequence.Serial);
            logicSet6.AddAction(new PlayAnimationLogicAction("AttackStart", "End", false), Types.Sequence.Parallel);
            logicSet6.AddAction(new DelayLogicAction(AttackThrustDurationMiniBoss, false), Types.Sequence.Serial);
            logicSet6.AddAction(new MoveLogicAction(null, true, 0f), Types.Sequence.Serial);
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightIdle_Character", true, true), Types.Sequence.Serial);
            logicSet6.AddAction(new DelayLogicAction(0.3f, false), Types.Sequence.Serial);
            logicSet6.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet6.Tag = 2;
            ProjectileData projectileData = new ProjectileData(this) {
                SpriteName = "EnemySpearKnightWave_Sprite",
                SourceAnchor = new Vector2(30f, 0f),
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = base.Damage,
                AngleOffset = 0f,
                Angle = new Vector2(0f, 0f),
                Scale = base.ProjectileScale
            };
            LogicSet logicSet7 = new LogicSet(this);
            logicSet7.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet7.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet7.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack2_Character", true, true), Types.Sequence.Serial);
            logicSet7.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet7.AddAction(new DelayLogicAction(AttackProjectileDelay, false), Types.Sequence.Serial);
            logicSet7.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "SpearKnightAttack1"
            }), Types.Sequence.Serial);
            logicSet7.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "SpearKnight_Projectile"
            }), Types.Sequence.Serial);
            logicSet7.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet7.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
            logicSet7.AddAction(new DelayLogicAction(0.3f, false), Types.Sequence.Serial);
            logicSet7.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet7.Tag = 2;
            LogicSet logicSet8 = new LogicSet(this);
            logicSet8.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet8.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet8.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack2_Character", true, true), Types.Sequence.Serial);
            logicSet8.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet8.AddAction(new DelayLogicAction(AttackProjectileExpertDelay, false), Types.Sequence.Serial);
            ThrowThreeProjectiles(logicSet8);
            logicSet8.AddAction(new DelayLogicAction(0.3f, false), Types.Sequence.Serial);
            logicSet8.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet8.Tag = 2;
            LogicSet logicSet9 = new LogicSet(this);
            logicSet9.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet9.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet9.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack2_Character", true, true), Types.Sequence.Serial);
            logicSet9.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet9.AddAction(new DelayLogicAction(AttackProjectileMinibossDelay, false), Types.Sequence.Serial);
            logicSet9.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "SpearKnight_Projectile"
            }), Types.Sequence.Serial);
            logicSet9.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.0333333351f), Types.Sequence.Serial);
            ThrowTwoProjectiles(logicSet9);
            logicSet9.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack2_Character", true, true), Types.Sequence.Serial);
            logicSet9.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet9.AddAction(new DelayLogicAction(0.05f, false), Types.Sequence.Serial);
            ThrowThreeProjectiles(logicSet9);
            logicSet9.AddAction(new DelayLogicAction(0.05f, false), Types.Sequence.Serial);
            logicSet9.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack2_Character", true, true), Types.Sequence.Serial);
            logicSet9.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet9.AddAction(new DelayLogicAction(0.05f, false), Types.Sequence.Serial);
            ThrowTwoProjectiles(logicSet9);
            logicSet9.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
            logicSet9.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet9.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.1f), Types.Sequence.Serial);
            logicSet9.Tag = 2;
            LogicSet logicSet10 = new LogicSet(this);
            logicSet10.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet10.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet10.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack2_Character", true, true), Types.Sequence.Serial);
            logicSet10.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet10.AddAction(new DelayLogicAction(AttackProjectileMinibossDelay, false), Types.Sequence.Serial);
            logicSet10.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "SpearKnight_Projectile"
            }), Types.Sequence.Serial);
            logicSet10.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.0333333351f), Types.Sequence.Serial);
            ThrowThreeProjectiles(logicSet10);
            logicSet10.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack2_Character", true, true), Types.Sequence.Serial);
            logicSet10.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet10.AddAction(new DelayLogicAction(0.05f, false), Types.Sequence.Serial);
            ThrowTwoProjectiles(logicSet10);
            logicSet10.AddAction(new DelayLogicAction(0.05f, false), Types.Sequence.Serial);
            logicSet10.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack2_Character", true, true), Types.Sequence.Serial);
            logicSet10.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet10.AddAction(new DelayLogicAction(0.05f, false), Types.Sequence.Serial);
            ThrowThreeProjectiles(logicSet10);
            logicSet10.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
            logicSet10.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet10.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.1f), Types.Sequence.Serial);
            logicSet10.Tag = 2;
            LogicSet logicSet11 = new LogicSet(this);
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightIdle_Character", true, true), Types.Sequence.Serial);
            logicSet11.AddAction(new MoveLogicAction(m_target, false, 300f), Types.Sequence.Serial);
            logicSet11.AddAction(new JumpLogicAction(0f), Types.Sequence.Serial);
            logicSet11.AddAction(new DelayLogicAction(0.3f, false), Types.Sequence.Serial);
            logicSet11.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
            logicSet11.AddAction(new JumpLogicAction(0f), Types.Sequence.Serial);
            ThrowRapidProjectiles(logicSet11);
            ThrowRapidProjectiles(logicSet11);
            ThrowRapidProjectiles(logicSet11);
            logicSet11.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
            logicSet11.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet11.AddAction(new DelayLogicAction(0.1f, false), Types.Sequence.Serial);
            logicSet11.AddAction(new JumpLogicAction(0f), Types.Sequence.Serial);
            ThrowRapidProjectiles(logicSet11);
            ThrowRapidProjectiles(logicSet11);
            ThrowRapidProjectiles(logicSet11);
            logicSet11.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
            logicSet11.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet11.Tag = 2;
            m_generalBasicLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2,
                logicSet3,
                logicSet4,
                logicSet7
            });
            m_generalAdvancedLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2,
                logicSet3,
                logicSet4,
                logicSet7
            });
            m_generalExpertLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2,
                logicSet3,
                logicSet5,
                logicSet8
            });
            m_generalMiniBossLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2,
                logicSet3,
                logicSet6,
                logicSet9,
                logicSet10
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
                55,
                25,
                20
            });
            projectileData.Dispose();
            base.InitializeLogic();
        }

        private void ThrowThreeProjectiles(LogicSet ls) {
            ProjectileData projectileData = new ProjectileData(this) {
                SpriteName = "EnemySpearKnightWave_Sprite",
                SourceAnchor = new Vector2(30f, 0f),
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = base.Damage,
                AngleOffset = 0f,
                Angle = new Vector2(0f, 0f),
                Scale = base.ProjectileScale
            };
            ls.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "SpearKnight_Projectile"
            }), Types.Sequence.Serial);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(45f, 45f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-45f, -45f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            ls.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
            projectileData.Dispose();
        }

        private void ThrowTwoProjectiles(LogicSet ls) {
            ProjectileData projectileData = new ProjectileData(this) {
                SpriteName = "EnemySpearKnightWave_Sprite",
                SourceAnchor = new Vector2(30f, 0f),
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = base.Damage,
                AngleOffset = 0f,
                Angle = new Vector2(22f, 22f)
            };
            ls.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "SpearKnightAttack1"
            }), Types.Sequence.Serial);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-22f, -22f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            ls.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
            projectileData.Dispose();
        }

        private void ThrowRapidProjectiles(LogicSet ls) {
            ProjectileData projectileData = new ProjectileData(this) {
                SpriteName = "EnemySpearKnightWave_Sprite",
                SourceAnchor = new Vector2(130f, -28f),
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = base.Damage,
                AngleOffset = 0f,
                Angle = Vector2.Zero
            };
            ls.AddAction(new DelayLogicAction(0.2f, 0.35f, false), Types.Sequence.Serial);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            ls.AddAction(new DelayLogicAction(0.2f, 0.35f, false), Types.Sequence.Serial);
            projectileData.SourceAnchor = new Vector2(130f, 28f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Dispose();
        }

        protected override void RunBasicLogic() {
            switch (base.State) {
                case 0: {
                    bool arg_AA_1 = true;
                    LogicBlock arg_AA_2 = m_generalBasicLB;
                    int[] array = new int[5];
                    array[2] = 100;
                    base.RunLogicBlock(arg_AA_1, arg_AA_2, array);
                    return;
                }
                case 1:
                    base.RunLogicBlock(true, m_generalBasicLB, new[] {
                        55,
                        30,
                        0,
                        0,
                        15
                    });
                    return;
                case 2:
                    base.RunLogicBlock(true, m_generalBasicLB, new[] {
                        20,
                        20,
                        10,
                        0,
                        50
                    });
                    return;
                case 3:
                    base.RunLogicBlock(true, m_generalBasicLB, new[] {
                        20,
                        15,
                        0,
                        0,
                        65
                    });
                    return;
                default:
                    return;
            }
        }

        protected override void RunAdvancedLogic() {
            switch (base.State) {
                case 0: {
                    bool arg_B4_1 = true;
                    LogicBlock arg_B4_2 = m_generalAdvancedLB;
                    int[] array = new int[5];
                    array[2] = 100;
                    base.RunLogicBlock(arg_B4_1, arg_B4_2, array);
                    return;
                }
                case 1:
                    base.RunLogicBlock(true, m_generalAdvancedLB, new[] {
                        55,
                        30,
                        0,
                        0,
                        15
                    });
                    return;
                case 2:
                    base.RunLogicBlock(true, m_generalAdvancedLB, new[] {
                        15,
                        15,
                        10,
                        15,
                        45
                    });
                    return;
                case 3:
                    base.RunLogicBlock(true, m_generalAdvancedLB, new[] {
                        15,
                        10,
                        0,
                        60,
                        15
                    });
                    return;
                default:
                    return;
            }
        }

        protected override void RunExpertLogic() {
            switch (base.State) {
                case 0: {
                    bool arg_B4_1 = true;
                    LogicBlock arg_B4_2 = m_generalExpertLB;
                    int[] array = new int[5];
                    array[2] = 100;
                    base.RunLogicBlock(arg_B4_1, arg_B4_2, array);
                    return;
                }
                case 1:
                    base.RunLogicBlock(true, m_generalExpertLB, new[] {
                        55,
                        30,
                        0,
                        0,
                        15
                    });
                    return;
                case 2:
                    base.RunLogicBlock(true, m_generalExpertLB, new[] {
                        15,
                        15,
                        10,
                        15,
                        45
                    });
                    return;
                case 3:
                    base.RunLogicBlock(true, m_generalExpertLB, new[] {
                        15,
                        10,
                        0,
                        60,
                        15
                    });
                    return;
                default:
                    return;
            }
        }

        protected override void RunMinibossLogic() {
            switch (base.State) {
                case 0:
                case 1:
                case 2:
                case 3:
                    base.RunLogicBlock(true, m_generalMiniBossLB, new[] {
                        14,
                        13,
                        11,
                        26,
                        18,
                        18
                    });
                    return;
                default:
                    return;
            }
        }

        public override void Update(GameTime gameTime) {
            if (base.SpriteName == "EnemySpearKnightWalk_Character") {
                m_walkSound.Update();
                m_walkSound2.Update();
            }
            base.Update(gameTime);
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer) {
            SoundManager.Play3DSound(this, Game.ScreenManager.Player, new[] {
                "Knight_Hit01",
                "Knight_Hit02",
                "Knight_Hit03"
            });
            base.HitEnemy(damage, position, isPlayer);
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_walkSound.Dispose();
                m_walkSound = null;
                m_walkSound2.Dispose();
                m_walkSound2 = null;
                base.Dispose();
            }
        }
    }
}
