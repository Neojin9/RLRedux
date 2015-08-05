using DS2DEngine;
using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class EnemyObj_Wolf : EnemyObj {
        private Color FurColour = Color.White;
        private float PounceDelay = 0.3f;
        private float PounceLandDelay = 0.5f;
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalCooldownLB = new LogicBlock();
        private FrameSoundObj m_runFrameSound;
        private float m_startDelay = 1f;
        private float m_startDelayCounter;
        private LogicBlock m_wolfHitLB = new LogicBlock();

        public EnemyObj_Wolf(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyWargIdle_Character", target, physicsManager, levelToAttachTo, difficulty) {
            Type = 19;
            m_startDelayCounter = m_startDelay;
            m_runFrameSound = new FrameSoundObj(this, 1, new[] {
                "Wolf_Move01",
                "Wolf_Move02",
                "Wolf_Move03"
            });
        }

        public bool Chasing { get; set; }

        protected override void InitializeEV() {
            base.Name = "Warg";
            MaxHealth = 18;
            base.Damage = 25;
            base.XPValue = 75;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 2;
            MoneyDropChance = 0.4f;
            base.Speed = 600f;
            this.TurnSpeed = 10f;
            ProjectileSpeed = 650f;
            base.JumpHeight = 1035f;
            CooldownTime = 2f;
            base.AnimationDelay = 0.0333333351f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = true;
            base.CanBeKnockedBack = true;
            base.IsWeighted = true;
            this.Scale = EnemyEV.Wolf_Basic_Scale;
            base.ProjectileScale = EnemyEV.Wolf_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Wolf_Basic_Tint;
            MeleeRadius = 50;
            ProjectileRadius = 400;
            EngageRadius = 550;
            ProjectileDamage = base.Damage;
            base.KnockBack = EnemyEV.Wolf_Basic_KnockBack;
            InitialLogicDelay = 1f;
            switch (base.Difficulty) {
                case GameTypes.EnemyDifficulty.BASIC:
                    break;
                case GameTypes.EnemyDifficulty.ADVANCED:
                    base.Name = "Wargen";
                    MaxHealth = 25;
                    base.Damage = 28;
                    base.XPValue = 125;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    base.Speed = 700f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 650f;
                    base.JumpHeight = 1035f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.0333333351f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Wolf_Advanced_Scale;
                    base.ProjectileScale = EnemyEV.Wolf_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Wolf_Advanced_Tint;
                    MeleeRadius = 50;
                    EngageRadius = 575;
                    ProjectileRadius = 400;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Wolf_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.EXPERT:
                    base.Name = "Wargenflorgen";
                    MaxHealth = 52;
                    base.Damage = 32;
                    base.XPValue = 225;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 3;
                    MoneyDropChance = 1f;
                    base.Speed = 850f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 650f;
                    base.JumpHeight = 1035f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.0333333351f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Wolf_Expert_Scale;
                    base.ProjectileScale = EnemyEV.Wolf_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Wolf_Expert_Tint;
                    MeleeRadius = 50;
                    ProjectileRadius = 400;
                    EngageRadius = 625;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Wolf_Expert_KnockBack;
                    return;
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    base.Name = "Zorg Warg";
                    MaxHealth = 500;
                    base.Damage = 35;
                    base.XPValue = 750;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 1f;
                    base.Speed = 925f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 650f;
                    base.JumpHeight = 1035f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.0333333351f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Wolf_Miniboss_Scale;
                    base.ProjectileScale = EnemyEV.Wolf_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Wolf_Miniboss_Tint;
                    MeleeRadius = 50;
                    ProjectileRadius = 400;
                    EngageRadius = 700;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Wolf_Miniboss_KnockBack;
                    return;
                default:
                    return;
            }
        }

        protected override void InitializeLogic() {
            LogicSet logicSet = new LogicSet(this);
            logicSet.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet.AddAction(new MoveLogicAction(m_target, true, -1f), Types.Sequence.Serial);
            logicSet.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyWargRun_Character", true, true), Types.Sequence.Serial);
            logicSet.AddAction(new ChangePropertyLogicAction(this, "Chasing", true), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
            LogicSet logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet2.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyWargIdle_Character", true, true), Types.Sequence.Serial);
            logicSet2.AddAction(new ChangePropertyLogicAction(this, "Chasing", false), Types.Sequence.Serial);
            logicSet2.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
            LogicSet logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
            logicSet3.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet3.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet3.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "Wolf_Attack"
            }), Types.Sequence.Serial);
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyWargPounce_Character", true, true), Types.Sequence.Serial);
            logicSet3.AddAction(new DelayLogicAction(PounceDelay, false), Types.Sequence.Serial);
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyWargJump_Character", false, false), Types.Sequence.Serial);
            logicSet3.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
            logicSet3.AddAction(new MoveDirectionLogicAction(-1f), Types.Sequence.Serial);
            logicSet3.AddAction(new JumpLogicAction(0f), Types.Sequence.Serial);
            logicSet3.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
            logicSet3.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyWargIdle_Character", true, true), Types.Sequence.Serial);
            logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet3.AddAction(new DelayLogicAction(PounceLandDelay, false), Types.Sequence.Serial);
            LogicSet logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyWargHit_Character", false, false), Types.Sequence.Serial);
            logicSet4.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
            logicSet4.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
            m_generalBasicLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2,
                logicSet3
            });
            m_wolfHitLB.AddLogicSet(new LogicSet[] {
                logicSet4
            });
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);
            logicBlocksToDispose.Add(m_wolfHitLB);
            base.SetCooldownLogicBlock(m_generalCooldownLB, new[] {
                40,
                40,
                20
            });
            base.InitializeLogic();
        }

        protected override void RunBasicLogic() {
            if (m_startDelayCounter <= 0f) {
                switch (base.State) {
                    case 0:
                        if (Chasing) {
                            bool arg_C7_1 = false;
                            LogicBlock arg_C7_2 = m_generalBasicLB;
                            int[] array = new int[3];
                            array[1] = 100;
                            base.RunLogicBlock(arg_C7_1, arg_C7_2, array);
                        }
                        break;
                    case 1:
                        if (!Chasing) {
                            bool arg_A1_1 = false;
                            LogicBlock arg_A1_2 = m_generalBasicLB;
                            int[] array2 = new int[3];
                            array2[0] = 100;
                            base.RunLogicBlock(arg_A1_1, arg_A1_2, array2);
                            return;
                        }
                        break;
                    case 2:
                    case 3: {
                        if (m_target.Y < base.Y - (float)m_target.Height) {
                            base.RunLogicBlock(false, m_generalBasicLB, new[] {
                                0,
                                0,
                                100
                            });
                            return;
                        }
                        bool arg_7E_1 = false;
                        LogicBlock arg_7E_2 = m_generalBasicLB;
                        int[] array3 = new int[3];
                        array3[0] = 100;
                        base.RunLogicBlock(arg_7E_1, arg_7E_2, array3);
                        return;
                    }
                    default:
                        return;
                }
            }
        }

        protected override void RunAdvancedLogic() {
            switch (base.State) {
                case 0:
                    if (Chasing) {
                        bool arg_B7_1 = false;
                        LogicBlock arg_B7_2 = m_generalBasicLB;
                        int[] array = new int[3];
                        array[1] = 100;
                        base.RunLogicBlock(arg_B7_1, arg_B7_2, array);
                    }
                    break;
                case 1:
                    if (!Chasing) {
                        bool arg_91_1 = false;
                        LogicBlock arg_91_2 = m_generalBasicLB;
                        int[] array2 = new int[3];
                        array2[0] = 100;
                        base.RunLogicBlock(arg_91_1, arg_91_2, array2);
                        return;
                    }
                    break;
                case 2:
                case 3: {
                    if (m_target.Y < base.Y - (float)m_target.Height) {
                        base.RunLogicBlock(false, m_generalBasicLB, new[] {
                            0,
                            0,
                            100
                        });
                        return;
                    }
                    bool arg_6E_1 = false;
                    LogicBlock arg_6E_2 = m_generalBasicLB;
                    int[] array3 = new int[3];
                    array3[0] = 100;
                    base.RunLogicBlock(arg_6E_1, arg_6E_2, array3);
                    return;
                }
                default:
                    return;
            }
        }

        protected override void RunExpertLogic() {
            switch (base.State) {
                case 0:
                    if (Chasing) {
                        bool arg_B7_1 = false;
                        LogicBlock arg_B7_2 = m_generalBasicLB;
                        int[] array = new int[3];
                        array[1] = 100;
                        base.RunLogicBlock(arg_B7_1, arg_B7_2, array);
                    }
                    break;
                case 1:
                    if (!Chasing) {
                        bool arg_91_1 = false;
                        LogicBlock arg_91_2 = m_generalBasicLB;
                        int[] array2 = new int[3];
                        array2[0] = 100;
                        base.RunLogicBlock(arg_91_1, arg_91_2, array2);
                        return;
                    }
                    break;
                case 2:
                case 3: {
                    if (m_target.Y < base.Y - (float)m_target.Height) {
                        base.RunLogicBlock(false, m_generalBasicLB, new[] {
                            0,
                            0,
                            100
                        });
                        return;
                    }
                    bool arg_6E_1 = false;
                    LogicBlock arg_6E_2 = m_generalBasicLB;
                    int[] array3 = new int[3];
                    array3[0] = 100;
                    base.RunLogicBlock(arg_6E_1, arg_6E_2, array3);
                    return;
                }
                default:
                    return;
            }
        }

        protected override void RunMinibossLogic() {
            switch (base.State) {
                case 0:
                    if (Chasing) {
                        bool arg_B7_1 = false;
                        LogicBlock arg_B7_2 = m_generalBasicLB;
                        int[] array = new int[3];
                        array[1] = 100;
                        base.RunLogicBlock(arg_B7_1, arg_B7_2, array);
                    }
                    break;
                case 1:
                    if (!Chasing) {
                        bool arg_91_1 = false;
                        LogicBlock arg_91_2 = m_generalBasicLB;
                        int[] array2 = new int[3];
                        array2[0] = 100;
                        base.RunLogicBlock(arg_91_1, arg_91_2, array2);
                        return;
                    }
                    break;
                case 2:
                case 3: {
                    if (m_target.Y < base.Y - (float)m_target.Height) {
                        base.RunLogicBlock(false, m_generalBasicLB, new[] {
                            0,
                            0,
                            100
                        });
                        return;
                    }
                    bool arg_6E_1 = false;
                    LogicBlock arg_6E_2 = m_generalBasicLB;
                    int[] array3 = new int[3];
                    array3[0] = 100;
                    base.RunLogicBlock(arg_6E_1, arg_6E_2, array3);
                    return;
                }
                default:
                    return;
            }
        }

        public override void Update(GameTime gameTime) {
            if (m_startDelayCounter > 0f)
                m_startDelayCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!m_isTouchingGround && base.IsWeighted && base.CurrentSpeed == 0f && base.SpriteName == "EnemyWargJump_Character")
                base.CurrentSpeed = base.Speed;
            base.Update(gameTime);
            if (m_isTouchingGround && base.CurrentSpeed == 0f && !base.IsAnimating) {
                this.ChangeSprite("EnemyWargIdle_Character");
                base.PlayAnimation(true);
            }
            if (base.SpriteName == "EnemyWargRun_Character")
                m_runFrameSound.Update();
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer) {
            SoundManager.Play3DSound(this, Game.ScreenManager.Player, new[] {
                "Wolf_Hit_01",
                "Wolf_Hit_02",
                "Wolf_Hit_03"
            });
            if (m_currentActiveLB != null && m_currentActiveLB.IsActive)
                m_currentActiveLB.StopLogicBlock();
            m_currentActiveLB = m_wolfHitLB;
            m_currentActiveLB.RunLogicBlock(new[] {
                100
            });
            base.HitEnemy(damage, position, isPlayer);
        }

        public override void ResetState() {
            m_startDelayCounter = m_startDelay;
            base.ResetState();
        }

        public override void Reset() {
            m_startDelayCounter = m_startDelay;
            base.Reset();
        }
    }
}
