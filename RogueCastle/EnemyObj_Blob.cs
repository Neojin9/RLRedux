using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Tweener;
using Tweener.Ease;


namespace RogueCastle {
    public class EnemyObj_Blob : EnemyObj {
        private Vector2 BlobSizeChange = new Vector2(0.4f, 0.4f);
        private float BlobSpeedChange = 1.2f;
        private float ExpertBlobProjectileDuration = 5f;
        private float JumpDelay = 0.4f;
        private int NumHits;
        public RoomObj SpawnRoom;
        private int m_bossCoins = 40;
        private int m_bossDiamonds = 8;
        private int m_bossEarthWizardLevelReduction = 12;
        private int m_bossMoneyBags = 16;
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalBossCooldownLB = new LogicBlock();
        private LogicBlock m_generalCooldownLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();
        private LogicBlock m_generalMiniBossLB = new LogicBlock();
        private LogicBlock m_generalNeoLB = new LogicBlock();
        private bool m_isNeo;

        public EnemyObj_Blob(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyBlobIdle_Character", target, physicsManager, levelToAttachTo, difficulty) {
            MainBlob = true;
            TintablePart = this._objectList[0];
            base.PlayAnimation(true);
            m_invincibleCounter = 0.5f;
            Type = 2;
        }

        public bool MainBlob { get; set; }
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
            SetNumberOfHits(2);
            BlobSizeChange = new Vector2(0.725f, 0.725f);
            BlobSpeedChange = 2f;
            base.Name = "Bloob";
            MaxHealth = 14;
            base.Damage = 13;
            base.XPValue = 25;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 1;
            MoneyDropChance = 0.225f;
            base.Speed = 50f;
            this.TurnSpeed = 10f;
            ProjectileSpeed = 0f;
            base.JumpHeight = 975f;
            CooldownTime = 2f;
            base.AnimationDelay = 0.05f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = true;
            base.CanBeKnockedBack = true;
            base.IsWeighted = true;
            this.Scale = EnemyEV.Blob_Basic_Scale;
            base.ProjectileScale = EnemyEV.Blob_Basic_ProjectileScale;
            MeleeRadius = 225;
            ProjectileRadius = 500;
            EngageRadius = 750;
            ProjectileDamage = base.Damage;
            base.KnockBack = EnemyEV.Blob_Basic_KnockBack;
            switch (base.Difficulty) {
                case GameTypes.EnemyDifficulty.ADVANCED:
                    SetNumberOfHits(3);
                    BlobSizeChange = new Vector2(0.6f, 0.6f);
                    BlobSpeedChange = 2.25f;
                    base.Name = "Bloobite";
                    MaxHealth = 18;
                    base.Damage = 14;
                    base.XPValue = 29;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 1;
                    MoneyDropChance = 0.2f;
                    base.Speed = 80f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 0f;
                    base.JumpHeight = 975f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.05f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Blob_Advanced_Scale;
                    base.ProjectileScale = EnemyEV.Blob_Advanced_ProjectileScale;
                    MeleeRadius = 225;
                    EngageRadius = 750;
                    ProjectileRadius = 500;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Blob_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.EXPERT:
                    SetNumberOfHits(4);
                    BlobSizeChange = new Vector2(0.65f, 0.65f);
                    BlobSpeedChange = 2.25f;
                    base.Name = "Bloobasaurus Rex";
                    MaxHealth = 22;
                    base.Damage = 16;
                    base.XPValue = 35;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 1;
                    MoneyDropChance = 0.1f;
                    base.Speed = 90f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 0f;
                    base.JumpHeight = 975f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.05f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Blob_Expert_Scale;
                    base.ProjectileScale = EnemyEV.Blob_Expert_ProjectileScale;
                    MeleeRadius = 225;
                    ProjectileRadius = 500;
                    EngageRadius = 750;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Blob_Expert_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    SetNumberOfHits(5);
                    BlobSizeChange = new Vector2(0.6f, 0.6f);
                    BlobSpeedChange = 2f;
                    base.ForceDraw = true;
                    base.Name = "Herodotus";
                    MaxHealth = 32;
                    base.Damage = 16;
                    base.XPValue = 70;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 4;
                    MoneyDropChance = 0.1f;
                    base.Speed = 110f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 0f;
                    base.JumpHeight = 975f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.05f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Blob_Miniboss_Scale;
                    base.ProjectileScale = EnemyEV.Blob_Miniboss_ProjectileScale;
                    MeleeRadius = 225;
                    ProjectileRadius = 500;
                    EngageRadius = 750;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Blob_Miniboss_KnockBack;
                    if (LevelEV.WEAKEN_BOSSES) {
                        MaxHealth = 1;
                        SetNumberOfHits(1);
                    }
                    break;
            }
            if (base.Difficulty == GameTypes.EnemyDifficulty.BASIC) {
                this._objectList[0].TextureColor = Color.Green;
                this._objectList[2].TextureColor = Color.LightGreen;
                this._objectList[2].Opacity = 0.8f;
                (this._objectList[1] as SpriteObj).OutlineColour = Color.Red;
                this._objectList[1].TextureColor = Color.Red;
            }
            else if (base.Difficulty == GameTypes.EnemyDifficulty.ADVANCED) {
                this._objectList[0].TextureColor = Color.Yellow;
                this._objectList[2].TextureColor = Color.LightYellow;
                this._objectList[2].Opacity = 0.8f;
                (this._objectList[1] as SpriteObj).OutlineColour = Color.Pink;
                this._objectList[1].TextureColor = Color.Pink;
            }
            else if (base.Difficulty == GameTypes.EnemyDifficulty.EXPERT) {
                this._objectList[0].TextureColor = Color.Red;
                this._objectList[2].TextureColor = Color.Pink;
                this._objectList[2].Opacity = 0.8f;
                (this._objectList[1] as SpriteObj).OutlineColour = Color.Yellow;
                this._objectList[1].TextureColor = Color.Yellow;
            }
            if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
                m_resetSpriteName = "EnemyBlobBossIdle_Character";
        }

        protected override void InitializeLogic() {
            LogicSet logicSet = new LogicSet(this);
            logicSet.AddAction(new MoveLogicAction(m_target, true, -1f), Types.Sequence.Serial);
            logicSet.AddAction(new Play3DSoundLogicAction(this, m_target, new[] {
                "Blob_Move01",
                "Blob_Move02",
                "Blob_Move03",
                "Blank",
                "Blank",
                "Blank",
                "Blank",
                "Blank"
            }), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(1.1f, 1.9f, false), Types.Sequence.Serial);
            LogicSet logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new MoveLogicAction(m_target, false, -1f), Types.Sequence.Serial);
            logicSet2.AddAction(new Play3DSoundLogicAction(this, m_target, new[] {
                "Blob_Move01",
                "Blob_Move02",
                "Blob_Move03",
                "Blank",
                "Blank",
                "Blank",
                "Blank",
                "Blank"
            }), Types.Sequence.Serial);
            logicSet2.AddAction(new DelayLogicAction(1f, 1.5f, false), Types.Sequence.Serial);
            LogicSet logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet3.AddAction(new DelayLogicAction(0.5f, 0.9f, false), Types.Sequence.Serial);
            LogicSet logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet4.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
            logicSet4.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyBlobJump_Character", false, false), Types.Sequence.Serial);
            logicSet4.AddAction(new PlayAnimationLogicAction("Start", "BeforeJump", false), Types.Sequence.Serial);
            logicSet4.AddAction(new DelayLogicAction(JumpDelay, false), Types.Sequence.Serial);
            logicSet4.AddAction(new MoveLogicAction(m_target, true, base.Speed * 6.75f), Types.Sequence.Serial);
            logicSet4.AddAction(new Play3DSoundLogicAction(this, m_target, new[] {
                "Blob_Jump"
            }), Types.Sequence.Serial);
            logicSet4.AddAction(new JumpLogicAction(0f), Types.Sequence.Serial);
            logicSet4.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyBlobAir_Character", true, true), Types.Sequence.Serial);
            logicSet4.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
            logicSet4.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
            logicSet4.AddAction(new Play3DSoundLogicAction(this, m_target, new[] {
                "Blob_Land"
            }), Types.Sequence.Serial);
            logicSet4.AddAction(new MoveLogicAction(m_target, true, base.Speed), Types.Sequence.Serial);
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyBlobJump_Character", false, false), Types.Sequence.Serial);
            logicSet4.AddAction(new PlayAnimationLogicAction("Start", "Jump", false), Types.Sequence.Serial);
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyBlobIdle_Character", true, true), Types.Sequence.Serial);
            logicSet4.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
            logicSet4.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet4.Tag = 2;
            ProjectileData data = new ProjectileData(this) {
                SpriteName = "SpellDamageShield_Sprite",
                SourceAnchor = new Vector2(0f, 10f),
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = base.Damage,
                Angle = new Vector2(0f, 0f),
                AngleOffset = 0f,
                CollidesWithTerrain = false,
                Scale = base.ProjectileScale,
                Lifespan = ExpertBlobProjectileDuration,
                LockPosition = true
            };
            LogicSet logicSet5 = new LogicSet(this);
            logicSet5.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet5.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
            logicSet5.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyBlobJump_Character", false, false), Types.Sequence.Serial);
            logicSet5.AddAction(new PlayAnimationLogicAction("Start", "BeforeJump", false), Types.Sequence.Serial);
            logicSet5.AddAction(new DelayLogicAction(JumpDelay, false), Types.Sequence.Serial);
            logicSet5.AddAction(new MoveLogicAction(m_target, true, base.Speed * 6.75f), Types.Sequence.Serial);
            logicSet5.AddAction(new Play3DSoundLogicAction(this, m_target, new[] {
                "Blob_Jump"
            }), Types.Sequence.Serial);
            logicSet5.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, data), Types.Sequence.Serial);
            logicSet5.AddAction(new JumpLogicAction(0f), Types.Sequence.Serial);
            logicSet5.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyBlobAir_Character", true, true), Types.Sequence.Serial);
            logicSet5.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
            logicSet5.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
            logicSet5.AddAction(new Play3DSoundLogicAction(this, m_target, new[] {
                "Blob_Land"
            }), Types.Sequence.Serial);
            logicSet5.AddAction(new MoveLogicAction(m_target, true, base.Speed), Types.Sequence.Serial);
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyBlobJump_Character", false, false), Types.Sequence.Serial);
            logicSet5.AddAction(new PlayAnimationLogicAction("Start", "Jump", false), Types.Sequence.Serial);
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyBlobIdle_Character", true, true), Types.Sequence.Serial);
            logicSet5.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
            logicSet5.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet5.Tag = 2;
            LogicSet logicSet6 = new LogicSet(this);
            logicSet6.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet6.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
            logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyBlobJump_Character", false, false), Types.Sequence.Serial);
            logicSet6.AddAction(new PlayAnimationLogicAction("Start", "BeforeJump", false), Types.Sequence.Serial);
            logicSet6.AddAction(new DelayLogicAction(JumpDelay, false), Types.Sequence.Serial);
            logicSet6.AddAction(new MoveLogicAction(m_target, true, base.Speed * 6.75f), Types.Sequence.Serial);
            logicSet6.AddAction(new Play3DSoundLogicAction(this, m_target, new[] {
                "Blob_Jump"
            }), Types.Sequence.Serial);
            logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, data), Types.Sequence.Serial);
            logicSet6.AddAction(new JumpLogicAction(0f), Types.Sequence.Serial);
            logicSet6.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyBlobAir_Character", true, true), Types.Sequence.Serial);
            logicSet6.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
            logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, data), Types.Sequence.Serial);
            logicSet6.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
            logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, data), Types.Sequence.Serial);
            logicSet6.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
            logicSet6.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
            logicSet6.AddAction(new Play3DSoundLogicAction(this, m_target, new[] {
                "Blob_Land"
            }), Types.Sequence.Serial);
            logicSet6.AddAction(new MoveLogicAction(m_target, true, base.Speed), Types.Sequence.Serial);
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyBlobJump_Character", false, false), Types.Sequence.Serial);
            logicSet6.AddAction(new PlayAnimationLogicAction("Start", "Jump", false), Types.Sequence.Serial);
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyBlobIdle_Character", true, true), Types.Sequence.Serial);
            logicSet6.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
            logicSet6.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet6.Tag = 2;
            LogicSet logicSet7 = new LogicSet(this);
            logicSet7.AddAction(new MoveLogicAction(m_target, true, -1f), Types.Sequence.Serial);
            logicSet7.AddAction(new Play3DSoundLogicAction(this, m_target, new[] {
                "Blob_Move01",
                "Blob_Move02",
                "Blob_Move03",
                "Blank",
                "Blank",
                "Blank",
                "Blank",
                "Blank"
            }), Types.Sequence.Serial);
            logicSet7.AddAction(new DelayLogicAction(1.1f, 1.9f, false), Types.Sequence.Serial);
            LogicSet logicSet8 = new LogicSet(this);
            logicSet8.AddAction(new MoveLogicAction(m_target, false, -1f), Types.Sequence.Serial);
            logicSet8.AddAction(new Play3DSoundLogicAction(this, m_target, new[] {
                "Blob_Move01",
                "Blob_Move02",
                "Blob_Move03",
                "Blank",
                "Blank",
                "Blank",
                "Blank",
                "Blank"
            }), Types.Sequence.Serial);
            logicSet8.AddAction(new DelayLogicAction(1f, 1.5f, false), Types.Sequence.Serial);
            LogicSet logicSet9 = new LogicSet(this);
            logicSet9.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet9.AddAction(new DelayLogicAction(0.5f, 0.9f, false), Types.Sequence.Serial);
            LogicSet logicSet10 = new LogicSet(this);
            logicSet10.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet10.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
            logicSet10.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyBlobBossJump_Character", false, false), Types.Sequence.Serial);
            logicSet10.AddAction(new PlayAnimationLogicAction("Start", "BeforeJump", false), Types.Sequence.Serial);
            logicSet10.AddAction(new DelayLogicAction(JumpDelay, false), Types.Sequence.Serial);
            logicSet10.AddAction(new MoveLogicAction(m_target, true, base.Speed * 6.75f), Types.Sequence.Serial);
            logicSet10.AddAction(new Play3DSoundLogicAction(this, m_target, new[] {
                "Blob_Jump"
            }), Types.Sequence.Serial);
            logicSet10.AddAction(new JumpLogicAction(0f), Types.Sequence.Serial);
            logicSet10.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyBlobBossAir_Character", true, true), Types.Sequence.Serial);
            logicSet10.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
            logicSet10.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
            logicSet10.AddAction(new Play3DSoundLogicAction(this, m_target, new[] {
                "Blob_Land"
            }), Types.Sequence.Serial);
            logicSet10.AddAction(new MoveLogicAction(m_target, true, base.Speed), Types.Sequence.Serial);
            logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyBlobBossJump_Character", false, false), Types.Sequence.Serial);
            logicSet10.AddAction(new PlayAnimationLogicAction("Start", "Jump", false), Types.Sequence.Serial);
            logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyBlobBossIdle_Character", true, true), Types.Sequence.Serial);
            logicSet10.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
            logicSet10.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet10.Tag = 2;
            LogicSet logicSet11 = new LogicSet(this);
            logicSet11.AddAction(new ChangeSpriteLogicAction("EnemyBlobBossAir_Character", true, true), Types.Sequence.Serial);
            logicSet11.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "FairyMove1",
                "FairyMove2",
                "FairyMove3"
            }), Types.Sequence.Serial);
            logicSet11.AddAction(new ChaseLogicAction(m_target, true, 1f, -1f), Types.Sequence.Serial);
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
            m_generalCooldownLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2,
                logicSet3
            });
            m_generalMiniBossLB.AddLogicSet(new LogicSet[] {
                logicSet7,
                logicSet8,
                logicSet9,
                logicSet10
            });
            m_generalNeoLB.AddLogicSet(new LogicSet[] {
                logicSet11
            });
            m_generalBossCooldownLB.AddLogicSet(new LogicSet[] {
                logicSet7,
                logicSet8,
                logicSet9
            });
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);
            logicBlocksToDispose.Add(m_generalNeoLB);
            logicBlocksToDispose.Add(m_generalMiniBossLB);
            logicBlocksToDispose.Add(m_generalBossCooldownLB);
            if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS) {
                base.SetCooldownLogicBlock(m_generalBossCooldownLB, new[] {
                    40,
                    40,
                    20
                });
                this.ChangeSprite("EnemyBlobBossIdle_Character");
            }
            else {
                base.SetCooldownLogicBlock(m_generalCooldownLB, new[] {
                    40,
                    40,
                    20
                });
            }
            base.InitializeLogic();
        }

        protected override void RunBasicLogic() {
            if (m_isTouchingGround) {
                switch (base.State) {
                    case 0:
                        base.RunLogicBlock(true, m_generalBasicLB, new[] {
                            10,
                            10,
                            75,
                            5
                        });
                        break;
                    case 1:
                    case 2:
                    case 3:
                        base.RunLogicBlock(true, m_generalBasicLB, new[] {
                            45,
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

        protected override void RunAdvancedLogic() {
            if (m_isTouchingGround) {
                switch (base.State) {
                    case 0:
                        base.RunLogicBlock(true, m_generalAdvancedLB, new[] {
                            10,
                            10,
                            75,
                            5
                        });
                        break;
                    case 1:
                    case 2:
                    case 3:
                        base.RunLogicBlock(true, m_generalAdvancedLB, new[] {
                            45,
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

        protected override void RunExpertLogic() {
            if (m_isTouchingGround) {
                switch (base.State) {
                    case 0:
                        base.RunLogicBlock(true, m_generalExpertLB, new[] {
                            10,
                            10,
                            75,
                            5
                        });
                        break;
                    case 1:
                    case 2:
                    case 3:
                        base.RunLogicBlock(true, m_generalExpertLB, new[] {
                            45,
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

        protected override void RunMinibossLogic() {
            if (m_isTouchingGround) {
                switch (base.State) {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        if (!IsNeo) {
                            base.RunLogicBlock(true, m_generalMiniBossLB, new[] {
                                45,
                                0,
                                0,
                                55
                            });
                            return;
                        }
                        break;
                    default:
                        return;
                }
            }
            else if (IsNeo) {
                base.AnimationDelay = 0.1f;
                base.RunLogicBlock(true, m_generalNeoLB, new[] {
                    100
                });
            }
        }

        public void SetNumberOfHits(int hits) {
            NumHits = hits;
        }

        private void CreateBlob(GameTypes.EnemyDifficulty difficulty, int numHits, bool isNeo = false) {
            EnemyObj_Blob enemyObj_Blob = new EnemyObj_Blob(null, null, null, difficulty);
            enemyObj_Blob.InitializeEV();
            enemyObj_Blob.Position = base.Position;
            if (m_target.X < enemyObj_Blob.X)
                enemyObj_Blob.Orientation = MathHelper.ToRadians(0f);
            else
                enemyObj_Blob.Orientation = MathHelper.ToRadians(180f);
            enemyObj_Blob.Level = base.Level;
            m_levelScreen.AddEnemyToCurrentRoom(enemyObj_Blob);
            enemyObj_Blob.Scale = new Vector2(this.ScaleX * BlobSizeChange.X, this.ScaleY * BlobSizeChange.Y);
            enemyObj_Blob.SetNumberOfHits(numHits);
            enemyObj_Blob.Speed *= BlobSpeedChange;
            enemyObj_Blob.MainBlob = false;
            enemyObj_Blob.SavedStartingPos = enemyObj_Blob.Position;
            enemyObj_Blob.IsNeo = isNeo;
            if (isNeo) {
                enemyObj_Blob.Name = base.Name;
                enemyObj_Blob.IsWeighted = false;
                enemyObj_Blob.TurnSpeed = this.TurnSpeed;
                enemyObj_Blob.Speed = base.Speed * BlobSpeedChange;
                enemyObj_Blob.Level = base.Level;
                enemyObj_Blob.MaxHealth = MaxHealth;
                enemyObj_Blob.CurrentHealth = enemyObj_Blob.MaxHealth;
                enemyObj_Blob.Damage = base.Damage;
                enemyObj_Blob.ChangeNeoStats(BlobSizeChange.X, BlobSpeedChange, numHits);
            }
            int num = CDGMath.RandomInt(-500, -300);
            int num2 = CDGMath.RandomInt(300, 700);
            if (enemyObj_Blob.X < m_target.X)
                enemyObj_Blob.AccelerationX += -(m_target.EnemyKnockBack.X + num);
            else
                enemyObj_Blob.AccelerationX += m_target.EnemyKnockBack.X + (float)num;
            enemyObj_Blob.AccelerationY += -(m_target.EnemyKnockBack.Y + num2);
            if (enemyObj_Blob.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS) {
                for (int i = 0; i < base.NumChildren; i++) {
                    enemyObj_Blob.GetChildAt(i).Opacity = base.GetChildAt(i).Opacity;
                    enemyObj_Blob.GetChildAt(i).TextureColor = base.GetChildAt(i).TextureColor;
                }
                enemyObj_Blob.ChangeSprite("EnemyBlobBossAir_Character");
            }
            else
                enemyObj_Blob.ChangeSprite("EnemyBlobAir_Character");
            enemyObj_Blob.PlayAnimation(true);
            if (LevelEV.SHOW_ENEMY_RADII)
                enemyObj_Blob.InitializeDebugRadii();
            enemyObj_Blob.SaveToFile = false;
            enemyObj_Blob.SpawnRoom = m_levelScreen.CurrentRoom;
            enemyObj_Blob.GivesLichHealth = false;
        }

        public void CreateWizard() {
            EnemyObj_EarthWizard enemyObj_EarthWizard = new EnemyObj_EarthWizard(null, null, null, GameTypes.EnemyDifficulty.ADVANCED);
            enemyObj_EarthWizard.PublicInitializeEV();
            enemyObj_EarthWizard.Position = base.Position;
            if (m_target.X < enemyObj_EarthWizard.X)
                enemyObj_EarthWizard.Orientation = MathHelper.ToRadians(0f);
            else
                enemyObj_EarthWizard.Orientation = MathHelper.ToRadians(180f);
            enemyObj_EarthWizard.Level = base.Level;
            enemyObj_EarthWizard.Level -= m_bossEarthWizardLevelReduction;
            m_levelScreen.AddEnemyToCurrentRoom(enemyObj_EarthWizard);
            enemyObj_EarthWizard.SavedStartingPos = enemyObj_EarthWizard.Position;
            int num = CDGMath.RandomInt(-500, -300);
            int num2 = CDGMath.RandomInt(300, 700);
            if (enemyObj_EarthWizard.X < m_target.X)
                enemyObj_EarthWizard.AccelerationX += -(m_target.EnemyKnockBack.X + num);
            else
                enemyObj_EarthWizard.AccelerationX += m_target.EnemyKnockBack.X + (float)num;
            enemyObj_EarthWizard.AccelerationY += -(m_target.EnemyKnockBack.Y + num2);
            enemyObj_EarthWizard.PlayAnimation(true);
            if (LevelEV.SHOW_ENEMY_RADII)
                enemyObj_EarthWizard.InitializeDebugRadii();
            enemyObj_EarthWizard.SaveToFile = false;
            enemyObj_EarthWizard.SpawnRoom = m_levelScreen.CurrentRoom;
            enemyObj_EarthWizard.GivesLichHealth = false;
        }

        public override void Update(GameTime gameTime) {
            if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS) {
                if (!m_isTouchingGround && base.IsWeighted && base.CurrentSpeed == 0f && base.SpriteName == "EnemyBlobBossAir_Character")
                    base.CurrentSpeed = base.Speed;
                if (!m_currentActiveLB.IsActive && m_isTouchingGround && base.SpriteName != "EnemyBlobBossIdle_Character") {
                    this.ChangeSprite("EnemyBlobBossIdle_Character");
                    base.PlayAnimation(true);
                }
            }
            else {
                if (!m_isTouchingGround && base.IsWeighted && base.CurrentSpeed == 0f && base.SpriteName == "EnemyBlobAir_Character")
                    base.CurrentSpeed = base.Speed;
                if (!m_currentActiveLB.IsActive && m_isTouchingGround && base.SpriteName != "EnemyBlobIdle_Character") {
                    this.ChangeSprite("EnemyBlobIdle_Character");
                    base.PlayAnimation(true);
                }
            }
            if (IsNeo) {
                foreach (EnemyObj current in m_levelScreen.CurrentRoom.EnemyList) {
                    if (current != this && current is EnemyObj_Blob) {
                        float num = Vector2.Distance(base.Position, current.Position);
                        if (num < 150f) {
                            Vector2 facePosition = 2f * base.Position - current.Position;
                            CDGMath.TurnToFace(this, facePosition);
                        }
                    }
                }
                foreach (EnemyObj current2 in m_levelScreen.CurrentRoom.TempEnemyList) {
                    if (current2 != this && current2 is EnemyObj_Blob) {
                        float num2 = Vector2.Distance(base.Position, current2.Position);
                        if (num2 < 150f) {
                            Vector2 facePosition2 = 2f * base.Position - current2.Position;
                            CDGMath.TurnToFace(this, facePosition2);
                        }
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer = true) {
            if (m_target != null && m_target.CurrentHealth > 0 && !m_bossVersionKilled) {
                base.HitEnemy(damage, position, isPlayer);
                if (base.CurrentHealth <= 0) {
                    base.CurrentHealth = MaxHealth;
                    NumHits--;
                    if (!base.IsKilled && NumHits > 0) {
                        if (!IsNeo) {
                            if (m_flipTween != null && m_flipTween.TweenedObject == this && m_flipTween.Active)
                                m_flipTween.StopTween(false);
                            this.ScaleX = this.ScaleY;
                            CreateBlob(base.Difficulty, NumHits, false);
                            this.Scale = new Vector2(this.ScaleX * BlobSizeChange.X, this.ScaleY * BlobSizeChange.Y);
                            base.Speed *= BlobSpeedChange;
                            if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
                                CreateWizard();
                        }
                        else {
                            if (m_flipTween != null && m_flipTween.TweenedObject == this && m_flipTween.Active)
                                m_flipTween.StopTween(false);
                            this.ScaleX = this.ScaleY;
                            CreateBlob(base.Difficulty, NumHits, true);
                            this.Scale = new Vector2(this.ScaleX * BlobSizeChange.X, this.ScaleY * BlobSizeChange.Y);
                            base.Speed *= BlobSpeedChange;
                        }
                    }
                }
                if (NumHits <= 0)
                    Kill(true);
            }
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType) {
            if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS && !m_bossVersionKilled) {
                PlayerObj playerObj = otherBox.AbsParent as PlayerObj;
                if (playerObj != null && otherBox.Type == 1 && !playerObj.IsInvincible && playerObj.State == 8)
                    playerObj.HitPlayer(this);
            }
            base.CollisionResponse(thisBox, otherBox, collisionResponseType);
        }

        public override void Kill(bool giveXP = true) {
            if (base.Difficulty != GameTypes.EnemyDifficulty.MINIBOSS) {
                base.Kill(giveXP);
                return;
            }
            if (m_target.CurrentHealth > 0) {
                BlobBossRoom blobBossRoom = m_levelScreen.CurrentRoom as BlobBossRoom;
                BlobChallengeRoom blobChallengeRoom = m_levelScreen.CurrentRoom as BlobChallengeRoom;
                if (((blobBossRoom != null && blobBossRoom.NumActiveBlobs == 1) || (blobChallengeRoom != null && blobChallengeRoom.NumActiveBlobs == 1)) && !m_bossVersionKilled) {
                    Game.PlayerStats.BlobBossBeaten = true;
                    SoundManager.StopMusic(0f);
                    m_bossVersionKilled = true;
                    m_target.LockControls();
                    m_levelScreen.PauseScreen();
                    m_levelScreen.ProjectileManager.DestroyAllProjectiles(false);
                    m_levelScreen.RunWhiteSlashEffect();
                    Tween.RunFunction(1f, this, "Part2", new object[0]);
                    SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flash");
                    SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Eyeball_Freeze");
                    GameUtil.UnlockAchievement("FEAR_OF_SLIME");
                    if (IsNeo) {
                        Tween.To(m_target.AttachedLevel.Camera, 0.5f, new Easing(Quad.EaseInOut), new[] {
                            "Zoom",
                            "1",
                            "X",
                            m_target.X.ToString(),
                            "Y",
                            m_target.Y.ToString()
                        });
                        Tween.AddEndHandlerToLastTween(this, "LockCamera", new object[0]);
                        return;
                    }
                }
                else
                    base.Kill(giveXP);
            }
        }

        public void LockCamera() {
            m_target.AttachedLevel.CameraLockedToPlayer = true;
        }

        public void Part2() {
            m_levelScreen.UnpauseScreen();
            m_target.UnlockControls();
            if (m_currentActiveLB != null)
                m_currentActiveLB.StopLogicBlock();
            m_target.CurrentSpeed = 0f;
            m_target.ForceInvincible = true;
            foreach (EnemyObj current in m_levelScreen.CurrentRoom.TempEnemyList) {
                if (!current.IsKilled)
                    current.Kill(true);
            }
            SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Blob_Death");
            base.Kill(true);
            if (!IsNeo) {
                List<int> list = new List<int>();
                for (int i = 0; i < m_bossCoins; i++)
                    list.Add(0);
                for (int j = 0; j < m_bossMoneyBags; j++)
                    list.Add(1);
                for (int k = 0; k < m_bossDiamonds; k++)
                    list.Add(2);
                CDGMath.Shuffle<int>(list);
                float num = 0f;
                for (int l = 0; l < list.Count; l++) {
                    Vector2 position = base.Position;
                    if (list[l] == 0) {
                        Tween.RunFunction((float)l * num, m_levelScreen.ItemDropManager, "DropItemWide", new object[] {
                            position,
                            1,
                            10
                        });
                    }
                    else if (list[l] == 1) {
                        Tween.RunFunction((float)l * num, m_levelScreen.ItemDropManager, "DropItemWide", new object[] {
                            position,
                            10,
                            100
                        });
                    }
                    else {
                        Tween.RunFunction((float)l * num, m_levelScreen.ItemDropManager, "DropItemWide", new object[] {
                            position,
                            11,
                            500
                        });
                    }
                }
            }
        }

        public override void Reset() {
            if (!MainBlob) {
                m_levelScreen.RemoveEnemyFromRoom(this, SpawnRoom, SavedStartingPos);
                Dispose();
                return;
            }
            switch (base.Difficulty) {
                case GameTypes.EnemyDifficulty.BASIC:
                    base.Speed = 50f;
                    this.Scale = EnemyEV.Blob_Basic_Scale;
                    NumHits = 2;
                    break;
                case GameTypes.EnemyDifficulty.ADVANCED:
                    base.Speed = 80f;
                    this.Scale = EnemyEV.Blob_Advanced_Scale;
                    NumHits = 3;
                    break;
                case GameTypes.EnemyDifficulty.EXPERT:
                    base.Speed = 90f;
                    this.Scale = EnemyEV.Blob_Expert_Scale;
                    NumHits = 4;
                    break;
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    base.Speed = 110f;
                    NumHits = 6;
                    break;
            }
            base.Reset();
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                SpawnRoom = null;
                base.Dispose();
            }
        }

        public void ChangeNeoStats(float blobSizeChange, float blobSpeedChange, int numHits) {
            NumHits = numHits;
            BlobSizeChange = new Vector2(blobSizeChange, blobSizeChange);
            BlobSpeedChange = blobSpeedChange;
        }
    }
}
