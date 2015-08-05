using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class EnemyObj_Starburst : EnemyObj {
        private float FireballDelay = 0.5f;
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();
        private LogicBlock m_generalMiniBossLB = new LogicBlock();

        public EnemyObj_Starburst(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyStarburstIdle_Character", target, physicsManager, levelToAttachTo, difficulty) {
            Type = 31;
        }

        protected override void InitializeEV() {
            base.Name = "Plinky";
            MaxHealth = 18;
            base.Damage = 15;
            base.XPValue = 50;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 1;
            MoneyDropChance = 0.4f;
            base.Speed = 435f;
            this.TurnSpeed = 10f;
            ProjectileSpeed = 435f;
            base.JumpHeight = 950f;
            CooldownTime = 0f;
            base.AnimationDelay = 0.05f;
            AlwaysFaceTarget = false;
            CanFallOffLedges = false;
            base.CanBeKnockedBack = false;
            base.IsWeighted = false;
            this.Scale = EnemyEV.Starburst_Basic_Scale;
            base.ProjectileScale = EnemyEV.Starburst_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Starburst_Basic_Tint;
            MeleeRadius = 325;
            ProjectileRadius = 690;
            EngageRadius = 850;
            ProjectileDamage = base.Damage;
            base.KnockBack = EnemyEV.Starburst_Basic_KnockBack;
            switch (base.Difficulty) {
                case GameTypes.EnemyDifficulty.BASIC:
                    break;
                case GameTypes.EnemyDifficulty.ADVANCED:
                    base.Name = "Planky";
                    MaxHealth = 25;
                    base.Damage = 18;
                    base.XPValue = 75;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    base.Speed = 435f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 435f;
                    base.JumpHeight = 950f;
                    CooldownTime = 0f;
                    base.AnimationDelay = 0.05f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = false;
                    base.IsWeighted = false;
                    this.Scale = EnemyEV.Starburst_Advanced_Scale;
                    base.ProjectileScale = EnemyEV.Starburst_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Starburst_Advanced_Tint;
                    MeleeRadius = 325;
                    EngageRadius = 850;
                    ProjectileRadius = 690;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Starburst_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.EXPERT:
                    base.Name = "Plonky";
                    MaxHealth = 42;
                    base.Damage = 21;
                    base.XPValue = 125;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 3;
                    MoneyDropChance = 1f;
                    base.Speed = 435f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 435f;
                    base.JumpHeight = 950f;
                    CooldownTime = 0f;
                    base.AnimationDelay = 0.05f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = false;
                    base.IsWeighted = false;
                    this.Scale = EnemyEV.Starburst_Expert_Scale;
                    base.ProjectileScale = EnemyEV.Starburst_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Starburst_Expert_Tint;
                    MeleeRadius = 325;
                    ProjectileRadius = 690;
                    EngageRadius = 850;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Starburst_Expert_KnockBack;
                    return;
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    base.Name = "Ploo";
                    MaxHealth = 750;
                    base.Damage = 30;
                    base.XPValue = 1100;
                    MinMoneyDropAmount = 8;
                    MaxMoneyDropAmount = 16;
                    MoneyDropChance = 1f;
                    base.Speed = 435f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 370f;
                    base.JumpHeight = 1350f;
                    CooldownTime = 0f;
                    base.AnimationDelay = 0.05f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = false;
                    base.IsWeighted = false;
                    this.Scale = EnemyEV.Starburst_Miniboss_Scale;
                    base.ProjectileScale = EnemyEV.Starburst_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Starburst_Miniboss_Tint;
                    MeleeRadius = 325;
                    ProjectileRadius = 690;
                    EngageRadius = 850;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Starburst_Miniboss_KnockBack;
                    return;
                default:
                    return;
            }
        }

        protected override void InitializeLogic() {
            ProjectileData projectileData = new ProjectileData(this) {
                SpriteName = "TurretProjectile_Sprite",
                SourceAnchor = Vector2.Zero,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = base.Damage,
                AngleOffset = 0f,
                CollidesWithTerrain = true,
                Scale = base.ProjectileScale
            };
            LogicSet logicSet = new LogicSet(this);
            projectileData.Angle = new Vector2(0f, 0f);
            logicSet.AddAction(new RunFunctionLogicAction(this, "FireAnimation", new object[0]), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(FireballDelay, false), Types.Sequence.Serial);
            logicSet.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "Eyeball_ProjectileAttack"
            }), Types.Sequence.Serial);
            logicSet.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-90f, -90f);
            logicSet.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(90f, 90f);
            logicSet.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(180f, 180f);
            logicSet.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyStarburstIdle_Character", true, true), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(1f, 1f, false), Types.Sequence.Serial);
            logicSet.Tag = 2;
            LogicSet logicSet2 = new LogicSet(this);
            projectileData.Angle = new Vector2(45f, 45f);
            logicSet2.AddAction(new ChangePropertyLogicAction(this._objectList[1], "Rotation", 45), Types.Sequence.Serial);
            logicSet2.AddAction(new RunFunctionLogicAction(this, "FireAnimation", new object[0]), Types.Sequence.Serial);
            logicSet2.AddAction(new ChangePropertyLogicAction(this._objectList[1], "Rotation", 45), Types.Sequence.Serial);
            logicSet2.AddAction(new DelayLogicAction(FireballDelay, false), Types.Sequence.Serial);
            logicSet2.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "Eyeball_ProjectileAttack"
            }), Types.Sequence.Serial);
            logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-45f, -45f);
            logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(135f, 135f);
            logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-135f, -135f);
            logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-90f, -90f);
            logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(90f, 90f);
            logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(180f, 180f);
            logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(0f, 0f);
            logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyStarburstIdle_Character", true, true), Types.Sequence.Serial);
            logicSet2.AddAction(new ChangePropertyLogicAction(this._objectList[1], "Rotation", 45), Types.Sequence.Serial);
            logicSet2.AddAction(new DelayLogicAction(1f, 1f, false), Types.Sequence.Serial);
            logicSet2.Tag = 2;
            LogicSet logicSet3 = new LogicSet(this);
            projectileData.Angle = new Vector2(45f, 45f);
            projectileData.CollidesWithTerrain = false;
            projectileData.SpriteName = "GhostProjectile_Sprite";
            logicSet3.AddAction(new ChangePropertyLogicAction(this._objectList[1], "Rotation", 45), Types.Sequence.Serial);
            logicSet3.AddAction(new RunFunctionLogicAction(this, "FireAnimation", new object[0]), Types.Sequence.Serial);
            logicSet3.AddAction(new ChangePropertyLogicAction(this._objectList[1], "Rotation", 45), Types.Sequence.Serial);
            logicSet3.AddAction(new DelayLogicAction(FireballDelay, false), Types.Sequence.Serial);
            logicSet3.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "Eyeball_ProjectileAttack"
            }), Types.Sequence.Serial);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-45f, -45f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(135f, 135f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-135f, -135f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-90f, -90f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(90f, 90f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(180f, 180f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(0f, 0f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyStarburstIdle_Character", true, true), Types.Sequence.Serial);
            logicSet3.AddAction(new DelayLogicAction(1f, 1f, false), Types.Sequence.Serial);
            logicSet3.AddAction(new RunFunctionLogicAction(this, "FireAnimation", new object[0]), Types.Sequence.Serial);
            logicSet3.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "Eyeball_ProjectileAttack"
            }), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(25f, 25f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-25f, -25f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(115f, 115f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-115f, -115f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-70f, -70f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(70f, 70f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(160f, 160f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Angle = new Vector2(-160f, -160f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyStarburstIdle_Character", true, true), Types.Sequence.Serial);
            logicSet3.AddAction(new ChangePropertyLogicAction(this._objectList[1], "Rotation", 45), Types.Sequence.Serial);
            logicSet3.AddAction(new DelayLogicAction(1.25f, 1.25f, false), Types.Sequence.Serial);
            logicSet3.Tag = 2;
            LogicSet logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new DelayLogicAction(0.5f, 0.5f, false), Types.Sequence.Serial);
            m_generalBasicLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet4
            });
            m_generalAdvancedLB.AddLogicSet(new LogicSet[] {
                logicSet2,
                logicSet4
            });
            m_generalExpertLB.AddLogicSet(new LogicSet[] {
                logicSet3,
                logicSet4
            });
            m_generalMiniBossLB.AddLogicSet(new LogicSet[] {
                logicSet2,
                logicSet4
            });
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalMiniBossLB);
            projectileData.Dispose();
            base.InitializeLogic();
        }

        public void FireAnimation() {
            this.ChangeSprite("EnemyStarburstAttack_Character");
            (this._objectList[0] as IAnimateableObj).PlayAnimation(true);
            (this._objectList[1] as IAnimateableObj).PlayAnimation(false);
        }

        protected override void RunBasicLogic() {
            switch (base.State) {
                case 0:
                    base.RunLogicBlock(true, m_generalBasicLB, new[] {
                        0,
                        100
                    });
                    return;
                case 1:
                case 2:
                case 3: {
                    bool arg_33_1 = true;
                    LogicBlock arg_33_2 = m_generalBasicLB;
                    int[] array = new int[2];
                    array[0] = 100;
                    base.RunLogicBlock(arg_33_1, arg_33_2, array);
                    return;
                }
                default:
                    return;
            }
        }

        protected override void RunAdvancedLogic() {
            switch (base.State) {
                case 0:
                    base.RunLogicBlock(true, m_generalAdvancedLB, new[] {
                        0,
                        100
                    });
                    return;
                case 1:
                case 2:
                case 3: {
                    bool arg_33_1 = true;
                    LogicBlock arg_33_2 = m_generalAdvancedLB;
                    int[] array = new int[2];
                    array[0] = 100;
                    base.RunLogicBlock(arg_33_1, arg_33_2, array);
                    return;
                }
                default:
                    return;
            }
        }

        protected override void RunExpertLogic() {
            switch (base.State) {
                case 0:
                    base.RunLogicBlock(true, m_generalExpertLB, new[] {
                        0,
                        100
                    });
                    return;
                case 1:
                case 2:
                case 3: {
                    bool arg_33_1 = true;
                    LogicBlock arg_33_2 = m_generalExpertLB;
                    int[] array = new int[2];
                    array[0] = 100;
                    base.RunLogicBlock(arg_33_1, arg_33_2, array);
                    return;
                }
                default:
                    return;
            }
        }

        protected override void RunMinibossLogic() {
            switch (base.State) {
                case 0:
                case 1:
                case 2:
                case 3: {
                    bool arg_38_1 = true;
                    LogicBlock arg_38_2 = m_generalMiniBossLB;
                    int[] array = new int[3];
                    array[0] = 60;
                    array[1] = 40;
                    base.RunLogicBlock(arg_38_1, arg_38_2, array);
                    return;
                }
                default:
                    return;
            }
        }
    }
}
