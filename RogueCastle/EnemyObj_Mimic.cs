using DS2DEngine;
using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class EnemyObj_Mimic : EnemyObj {
        private FrameSoundObj m_closeSound;
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private bool m_isAttacking;

        public EnemyObj_Mimic(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyMimicIdle_Character", target, physicsManager, levelToAttachTo, difficulty) {
            Type = 33;
            base.OutlineWidth = 0;
            m_closeSound = new FrameSoundObj(this, m_target, 1, new[] {
                "Chest_Snap"
            });
        }

        protected override void InitializeEV() {
            base.Name = "Mimic";
            MaxHealth = 35;
            base.Damage = 20;
            base.XPValue = 75;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 1;
            MoneyDropChance = 0.4f;
            base.Speed = 400f;
            this.TurnSpeed = 10f;
            ProjectileSpeed = 775f;
            base.JumpHeight = 550f;
            CooldownTime = 2f;
            base.AnimationDelay = 0.05f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = true;
            base.CanBeKnockedBack = true;
            base.IsWeighted = true;
            this.Scale = EnemyEV.Mimic_Basic_Scale;
            base.ProjectileScale = EnemyEV.Mimic_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Mimic_Basic_Tint;
            MeleeRadius = 10;
            ProjectileRadius = 20;
            EngageRadius = 975;
            ProjectileDamage = base.Damage;
            base.KnockBack = EnemyEV.Mimic_Basic_KnockBack;
            switch (base.Difficulty) {
                case GameTypes.EnemyDifficulty.ADVANCED:
                    base.Name = "Mimicant";
                    MaxHealth = 40;
                    base.Damage = 23;
                    base.XPValue = 125;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    base.Speed = 600f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 1100f;
                    base.JumpHeight = 650f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.05f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Mimic_Advanced_Scale;
                    base.ProjectileScale = EnemyEV.Mimic_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Mimic_Advanced_Tint;
                    MeleeRadius = 10;
                    EngageRadius = 975;
                    ProjectileRadius = 20;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Mimic_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.EXPERT:
                    base.Name = "Mimicrunch";
                    MaxHealth = 70;
                    base.Damage = 25;
                    base.XPValue = 225;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 3;
                    MoneyDropChance = 1f;
                    base.Speed = 750f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 925f;
                    base.JumpHeight = 550f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.05f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Mimic_Expert_Scale;
                    base.ProjectileScale = EnemyEV.Mimic_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Mimic_Expert_Tint;
                    MeleeRadius = 10;
                    ProjectileRadius = 20;
                    EngageRadius = 975;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Mimic_Expert_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    base.Name = "Chesticles";
                    MaxHealth = 100;
                    base.Damage = 32;
                    base.XPValue = 750;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 4;
                    MoneyDropChance = 1f;
                    base.Speed = 0f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 900f;
                    base.JumpHeight = 750f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.05f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Mimic_Miniboss_Scale;
                    base.ProjectileScale = EnemyEV.Mimic_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Mimic_Miniboss_Tint;
                    MeleeRadius = 10;
                    ProjectileRadius = 20;
                    EngageRadius = 975;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Mimic_Miniboss_KnockBack;
                    break;
            }
            base.LockFlip = true;
        }

        protected override void InitializeLogic() {
            LogicSet logicSet = new LogicSet(this);
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyMimicShake_Character", false, false), Types.Sequence.Serial);
            logicSet.AddAction(new PlayAnimationLogicAction(false), Types.Sequence.Serial);
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyMimicIdle_Character", true, false), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(3f, false), Types.Sequence.Serial);
            LogicSet logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
            logicSet2.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet2.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet2.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyMimicAttack_Character", true, true), Types.Sequence.Serial);
            logicSet2.AddAction(new MoveDirectionLogicAction(-1f), Types.Sequence.Serial);
            logicSet2.AddAction(new Play3DSoundLogicAction(this, m_target, new[] {
                "Chest_Open_Large"
            }), Types.Sequence.Serial);
            logicSet2.AddAction(new JumpLogicAction(0f), Types.Sequence.Serial);
            logicSet2.AddAction(new DelayLogicAction(0.3f, false), Types.Sequence.Serial);
            logicSet2.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
            new LogicSet(this);
            m_generalBasicLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2
            });
            logicBlocksToDispose.Add(m_generalBasicLB);
            base.InitializeLogic();
        }

        protected override void RunBasicLogic() {
            switch (base.State) {
                case 0:
                case 1:
                case 2:
                case 3:
                    if (!m_isAttacking) {
                        bool arg_3A_1 = false;
                        LogicBlock arg_3A_2 = m_generalBasicLB;
                        int[] array = new int[2];
                        array[0] = 100;
                        base.RunLogicBlock(arg_3A_1, arg_3A_2, array);
                        return;
                    }
                    base.RunLogicBlock(false, m_generalBasicLB, new[] {
                        0,
                        100
                    });
                    return;
            }
        }

        protected override void RunAdvancedLogic() {
            switch (base.State) {
                case 0:
                case 1:
                case 2:
                case 3:
                    if (!m_isAttacking) {
                        bool arg_3A_1 = false;
                        LogicBlock arg_3A_2 = m_generalBasicLB;
                        int[] array = new int[2];
                        array[0] = 100;
                        base.RunLogicBlock(arg_3A_1, arg_3A_2, array);
                        return;
                    }
                    base.RunLogicBlock(false, m_generalBasicLB, new[] {
                        0,
                        100
                    });
                    return;
            }
        }

        protected override void RunExpertLogic() {
            switch (base.State) {
                case 0:
                case 1:
                case 2:
                case 3:
                    if (!m_isAttacking) {
                        bool arg_3A_1 = false;
                        LogicBlock arg_3A_2 = m_generalBasicLB;
                        int[] array = new int[2];
                        array[0] = 100;
                        base.RunLogicBlock(arg_3A_1, arg_3A_2, array);
                        return;
                    }
                    base.RunLogicBlock(false, m_generalBasicLB, new[] {
                        0,
                        100
                    });
                    return;
            }
        }

        protected override void RunMinibossLogic() {
            switch (base.State) {
                case 0:
                case 1:
                case 2:
                case 3:
                    if (!m_isAttacking) {
                        bool arg_3A_1 = false;
                        LogicBlock arg_3A_2 = m_generalBasicLB;
                        int[] array = new int[2];
                        array[0] = 100;
                        base.RunLogicBlock(arg_3A_1, arg_3A_2, array);
                        return;
                    }
                    base.RunLogicBlock(false, m_generalBasicLB, new[] {
                        0,
                        100
                    });
                    return;
            }
        }

        public override void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer) {
            if (!m_isAttacking) {
                m_currentActiveLB.StopLogicBlock();
                m_isAttacking = true;
                base.LockFlip = false;
            }
            base.HitEnemy(damage, collisionPt, isPlayer);
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType) {
            if (otherBox.AbsParent is PlayerObj && !m_isAttacking) {
                m_currentActiveLB.StopLogicBlock();
                m_isAttacking = true;
                base.LockFlip = false;
            }
            base.CollisionResponse(thisBox, otherBox, collisionResponseType);
        }

        public override void Update(GameTime gameTime) {
            if (base.SpriteName == "EnemyMimicAttack_Character")
                m_closeSound.Update();
            base.Update(gameTime);
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_closeSound.Dispose();
                m_closeSound = null;
                base.Dispose();
            }
        }
    }
}
