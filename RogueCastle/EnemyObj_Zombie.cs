using DS2DEngine;
using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class EnemyObj_Zombie : EnemyObj {
        private LogicBlock m_basicRiseLowerLS = new LogicBlock();
        private LogicBlock m_basicWalkLS = new LogicBlock();

        public EnemyObj_Zombie(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyZombieLower_Character", target, physicsManager, levelToAttachTo, difficulty) {
            base.GoToFrame(base.TotalFrames);
            Lowered = true;
            base.ForceDraw = true;
            base.StopAnimation();
            Type = 20;
            base.PlayAnimationOnRestart = false;
        }

        public bool Risen { get; set; }
        public bool Lowered { get; set; }

        protected override void InitializeEV() {
            base.Name = "Zombie";
            MaxHealth = 24;
            base.Damage = 25;
            base.XPValue = 25;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 1;
            MoneyDropChance = 0.4f;
            base.Speed = 160f;
            this.TurnSpeed = 10f;
            ProjectileSpeed = 650f;
            base.JumpHeight = 900f;
            CooldownTime = 2f;
            base.AnimationDelay = 0.0833333358f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = false;
            base.CanBeKnockedBack = true;
            base.IsWeighted = true;
            this.Scale = EnemyEV.Zombie_Basic_Scale;
            base.ProjectileScale = EnemyEV.Zombie_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Zombie_Basic_Tint;
            MeleeRadius = 100;
            ProjectileRadius = 150;
            EngageRadius = 435;
            ProjectileDamage = base.Damage;
            base.KnockBack = EnemyEV.Zombie_Basic_KnockBack;
            switch (base.Difficulty) {
                case GameTypes.EnemyDifficulty.BASIC:
                    break;
                case GameTypes.EnemyDifficulty.ADVANCED:
                    base.Name = "Zomboner";
                    MaxHealth = 39;
                    base.Damage = 29;
                    base.XPValue = 75;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    base.Speed = 260f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 650f;
                    base.JumpHeight = 900f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.0714285746f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Zombie_Advanced_Scale;
                    base.ProjectileScale = EnemyEV.Zombie_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Zombie_Advanced_Tint;
                    MeleeRadius = 100;
                    EngageRadius = 435;
                    ProjectileRadius = 150;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Zombie_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.EXPERT:
                    base.Name = "Zombishnu";
                    MaxHealth = 70;
                    base.Damage = 33;
                    base.XPValue = 200;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 3;
                    MoneyDropChance = 1f;
                    base.Speed = 350f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 650f;
                    base.JumpHeight = 900f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.0625f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = false;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Zombie_Expert_Scale;
                    base.ProjectileScale = EnemyEV.Zombie_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Zombie_Expert_Tint;
                    MeleeRadius = 100;
                    ProjectileRadius = 150;
                    EngageRadius = 435;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Zombie_Expert_KnockBack;
                    return;
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    base.Name = "Zomg";
                    MaxHealth = 800;
                    base.Damage = 40;
                    base.XPValue = 600;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 1f;
                    base.Speed = 600f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 650f;
                    base.JumpHeight = 900f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.0714285746f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = false;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Zombie_Miniboss_Scale;
                    base.ProjectileScale = EnemyEV.Zombie_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Zombie_Miniboss_Tint;
                    MeleeRadius = 100;
                    ProjectileRadius = 150;
                    EngageRadius = 435;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Zombie_Miniboss_KnockBack;
                    return;
                default:
                    return;
            }
        }

        protected override void InitializeLogic() {
            LogicSet logicSet = new LogicSet(this);
            logicSet.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyZombieWalk_Character", true, true), Types.Sequence.Serial);
            logicSet.AddAction(new MoveLogicAction(m_target, true, -1f), Types.Sequence.Serial);
            logicSet.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "Zombie_Groan_01",
                "Zombie_Groan_02",
                "Zombie_Groan_03",
                "Blank",
                "Blank",
                "Blank",
                "Blank",
                "Blank"
            }), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
            LogicSet logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet2.AddAction(new MoveLogicAction(m_target, false, 0f), Types.Sequence.Serial);
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyZombieRise_Character", false, false), Types.Sequence.Serial);
            logicSet2.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "Zombie_Rise"
            }), Types.Sequence.Serial);
            logicSet2.AddAction(new PlayAnimationLogicAction(false), Types.Sequence.Serial);
            logicSet2.AddAction(new ChangePropertyLogicAction(this, "Risen", true), Types.Sequence.Serial);
            logicSet2.AddAction(new ChangePropertyLogicAction(this, "Lowered", false), Types.Sequence.Serial);
            LogicSet logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet3.AddAction(new MoveLogicAction(m_target, false, 0f), Types.Sequence.Serial);
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyZombieLower_Character", false, false), Types.Sequence.Serial);
            logicSet3.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "Zombie_Lower"
            }), Types.Sequence.Serial);
            logicSet3.AddAction(new PlayAnimationLogicAction(false), Types.Sequence.Serial);
            logicSet3.AddAction(new ChangePropertyLogicAction(this, "Risen", false), Types.Sequence.Serial);
            logicSet3.AddAction(new ChangePropertyLogicAction(this, "Lowered", true), Types.Sequence.Serial);
            m_basicWalkLS.AddLogicSet(new LogicSet[] {
                logicSet
            });
            m_basicRiseLowerLS.AddLogicSet(new LogicSet[] {
                logicSet2,
                logicSet3
            });
            logicBlocksToDispose.Add(m_basicWalkLS);
            logicBlocksToDispose.Add(m_basicRiseLowerLS);
            base.InitializeLogic();
        }

        protected override void RunBasicLogic() {
            switch (base.State) {
                case 0:
                    if (!Lowered) {
                        base.RunLogicBlock(false, m_basicRiseLowerLS, new[] {
                            0,
                            100
                        });
                    }
                    return;
                case 1:
                case 2:
                case 3:
                    if (!Risen) {
                        bool arg_3B_1 = false;
                        LogicBlock arg_3B_2 = m_basicRiseLowerLS;
                        int[] array = new int[2];
                        array[0] = 100;
                        base.RunLogicBlock(arg_3B_1, arg_3B_2, array);
                        return;
                    }
                    base.RunLogicBlock(false, m_basicWalkLS, new[] {
                        100
                    });
                    return;
                default:
                    return;
            }
        }

        protected override void RunAdvancedLogic() {
            switch (base.State) {
                case 0:
                    if (!Lowered) {
                        base.RunLogicBlock(false, m_basicRiseLowerLS, new[] {
                            0,
                            100
                        });
                    }
                    return;
                case 1:
                case 2:
                case 3:
                    if (!Risen) {
                        bool arg_3B_1 = false;
                        LogicBlock arg_3B_2 = m_basicRiseLowerLS;
                        int[] array = new int[2];
                        array[0] = 100;
                        base.RunLogicBlock(arg_3B_1, arg_3B_2, array);
                        return;
                    }
                    base.RunLogicBlock(false, m_basicWalkLS, new[] {
                        100
                    });
                    return;
                default:
                    return;
            }
        }

        protected override void RunExpertLogic() {
            switch (base.State) {
                case 0:
                    if (!Lowered) {
                        base.RunLogicBlock(false, m_basicRiseLowerLS, new[] {
                            0,
                            100
                        });
                    }
                    return;
                case 1:
                case 2:
                case 3:
                    if (!Risen) {
                        bool arg_3B_1 = false;
                        LogicBlock arg_3B_2 = m_basicRiseLowerLS;
                        int[] array = new int[2];
                        array[0] = 100;
                        base.RunLogicBlock(arg_3B_1, arg_3B_2, array);
                        return;
                    }
                    base.RunLogicBlock(false, m_basicWalkLS, new[] {
                        100
                    });
                    return;
                default:
                    return;
            }
        }

        protected override void RunMinibossLogic() {
            switch (base.State) {
                case 0:
                    if (!Lowered) {
                        base.RunLogicBlock(false, m_basicRiseLowerLS, new[] {
                            0,
                            100
                        });
                    }
                    return;
                case 1:
                case 2:
                case 3:
                    if (!Risen) {
                        bool arg_3B_1 = false;
                        LogicBlock arg_3B_2 = m_basicRiseLowerLS;
                        int[] array = new int[2];
                        array[0] = 100;
                        base.RunLogicBlock(arg_3B_1, arg_3B_2, array);
                        return;
                    }
                    base.RunLogicBlock(false, m_basicWalkLS, new[] {
                        100
                    });
                    return;
                default:
                    return;
            }
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer) {
            SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Zombie_Hit");
            base.HitEnemy(damage, position, isPlayer);
        }

        public override void Update(GameTime gameTime) {
            if ((m_currentActiveLB == null || !m_currentActiveLB.IsActive) && !Risen && base.IsAnimating) {
                this.ChangeSprite("EnemyZombieRise_Character");
                base.StopAnimation();
            }
            base.Update(gameTime);
        }

        public override void ResetState() {
            Lowered = true;
            Risen = false;
            base.ResetState();
            this.ChangeSprite("EnemyZombieLower_Character");
            base.GoToFrame(base.TotalFrames);
            base.StopAnimation();
        }

        public override void Reset() {
            this.ChangeSprite("EnemyZombieRise_Character");
            base.StopAnimation();
            Lowered = true;
            Risen = false;
            base.Reset();
        }
    }
}
