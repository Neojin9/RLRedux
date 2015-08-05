using System;
using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class EnemyObj_Ninja : EnemyObj {
        private float ChanceToTeleport = 0.35f;
        private float PauseAfterProjectile = 0.45f;
        private float PauseBeforeProjectile = 0.45f;
        private float TeleportDelay = 0.35f;
        private LogicBlock m_basicTeleportAttackLB = new LogicBlock();
        private TerrainObj m_closestCeiling;
        private LogicBlock m_expertTeleportAttackLB = new LogicBlock();
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalCooldownLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();
        private SpriteObj m_log;
        private SpriteObj m_smoke;
        private RoomObj m_storedRoom;
        private float m_teleportDamageReduc = 0.6f;

        public EnemyObj_Ninja(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyNinjaIdle_Character", target, physicsManager, levelToAttachTo, difficulty) {
            Type = 13;
            m_smoke = new SpriteObj("NinjaSmoke_Sprite");
            m_smoke.AnimationDelay = 0.05f;
            m_log = new SpriteObj("Log_Sprite");
            m_smoke.Visible = false;
            m_smoke.Scale = new Vector2(5f, 5f);
            m_log.Visible = false;
            m_log.OutlineWidth = 2;
        }

        protected override void InitializeEV() {
            base.Name = "Ninjo";
            MaxHealth = 30;
            base.Damage = 20;
            base.XPValue = 150;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 2;
            MoneyDropChance = 0.4f;
            base.Speed = 250f;
            this.TurnSpeed = 10f;
            ProjectileSpeed = 550f;
            base.JumpHeight = 600f;
            CooldownTime = 1.5f;
            base.AnimationDelay = 0.1f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = false;
            base.CanBeKnockedBack = true;
            base.IsWeighted = true;
            this.Scale = EnemyEV.Ninja_Basic_Scale;
            base.ProjectileScale = EnemyEV.Ninja_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Ninja_Basic_Tint;
            MeleeRadius = 225;
            ProjectileRadius = 700;
            EngageRadius = 1000;
            ProjectileDamage = base.Damage;
            base.KnockBack = EnemyEV.Ninja_Basic_KnockBack;
            switch (base.Difficulty) {
                case GameTypes.EnemyDifficulty.BASIC:
                    break;
                case GameTypes.EnemyDifficulty.ADVANCED:
                    ChanceToTeleport = 0.5f;
                    base.Name = "Ninpo";
                    MaxHealth = 44;
                    base.Damage = 25;
                    base.XPValue = 250;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    base.Speed = 325f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 625f;
                    base.JumpHeight = 600f;
                    CooldownTime = 1.5f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Ninja_Advanced_Scale;
                    base.ProjectileScale = EnemyEV.Ninja_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Ninja_Advanced_Tint;
                    MeleeRadius = 225;
                    EngageRadius = 1000;
                    ProjectileRadius = 700;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Ninja_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.EXPERT:
                    ChanceToTeleport = 0.65f;
                    base.Name = "Ninopojo";
                    MaxHealth = 62;
                    base.Damage = 29;
                    base.XPValue = 450;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 4;
                    MoneyDropChance = 1f;
                    base.Speed = 400f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 700f;
                    base.JumpHeight = 600f;
                    CooldownTime = 1.5f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Ninja_Expert_Scale;
                    base.ProjectileScale = EnemyEV.Ninja_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Ninja_Expert_Tint;
                    MeleeRadius = 225;
                    ProjectileRadius = 700;
                    EngageRadius = 1000;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Ninja_Expert_KnockBack;
                    return;
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    base.Name = "Master Ninja";
                    MaxHealth = 900;
                    base.Damage = 38;
                    base.XPValue = 1250;
                    MinMoneyDropAmount = 10;
                    MaxMoneyDropAmount = 15;
                    MoneyDropChance = 1f;
                    base.Speed = 150f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 600f;
                    base.JumpHeight = 600f;
                    CooldownTime = 1.5f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Ninja_Miniboss_Scale;
                    base.ProjectileScale = EnemyEV.Ninja_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Ninja_Miniboss_Tint;
                    MeleeRadius = 225;
                    ProjectileRadius = 700;
                    EngageRadius = 1000;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Ninja_Miniboss_KnockBack;
                    return;
                default:
                    return;
            }
        }

        protected override void InitializeLogic() {
            LogicSet logicSet = new LogicSet(this);
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyNinjaRun_Character", true, true), Types.Sequence.Serial);
            logicSet.AddAction(new PlayAnimationLogicAction(true), Types.Sequence.Serial);
            logicSet.AddAction(new MoveLogicAction(m_target, true, -1f), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(0.25f, 0.85f, false), Types.Sequence.Serial);
            LogicSet logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyNinjaRun_Character", true, true), Types.Sequence.Serial);
            logicSet2.AddAction(new PlayAnimationLogicAction(true), Types.Sequence.Serial);
            logicSet2.AddAction(new MoveLogicAction(m_target, false, -1f), Types.Sequence.Serial);
            logicSet2.AddAction(new DelayLogicAction(0.25f, 0.85f, false), Types.Sequence.Serial);
            LogicSet logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyNinjaIdle_Character", true, true), Types.Sequence.Serial);
            logicSet3.AddAction(new StopAnimationLogicAction(), Types.Sequence.Serial);
            logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet3.AddAction(new DelayLogicAction(0.25f, 0.85f, false), Types.Sequence.Serial);
            ProjectileData projectileData = new ProjectileData(this) {
                SpriteName = "ShurikenProjectile1_Sprite",
                SourceAnchor = new Vector2(15f, 0f),
                Target = m_target,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 20f,
                Damage = base.Damage,
                AngleOffset = 0f,
                CollidesWithTerrain = true,
                Scale = base.ProjectileScale
            };
            LogicSet logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyNinjaIdle_Character", true, true), Types.Sequence.Serial);
            logicSet4.AddAction(new DelayLogicAction(PauseBeforeProjectile, false), Types.Sequence.Serial);
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyNinjaThrow_Character", false, false), Types.Sequence.Serial);
            logicSet4.AddAction(new PlayAnimationLogicAction(1, 3, false), Types.Sequence.Serial);
            logicSet4.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "Ninja_ThrowStar_01",
                "Ninja_ThrowStar_02",
                "Ninja_ThrowStar_03"
            }), Types.Sequence.Serial);
            logicSet4.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet4.AddAction(new PlayAnimationLogicAction(4, 5, false), Types.Sequence.Serial);
            logicSet4.AddAction(new DelayLogicAction(PauseAfterProjectile, false), Types.Sequence.Serial);
            logicSet4.Tag = 2;
            LogicSet logicSet5 = new LogicSet(this);
            logicSet5.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyNinjaIdle_Character", true, true), Types.Sequence.Serial);
            logicSet5.AddAction(new DelayLogicAction(PauseBeforeProjectile, false), Types.Sequence.Serial);
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyNinjaThrow_Character", false, false), Types.Sequence.Serial);
            logicSet5.AddAction(new PlayAnimationLogicAction(1, 3, false), Types.Sequence.Serial);
            logicSet5.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "Ninja_ThrowStar_01",
                "Ninja_ThrowStar_02",
                "Ninja_ThrowStar_03"
            }), Types.Sequence.Serial);
            logicSet5.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.AngleOffset = -10f;
            logicSet5.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.AngleOffset = 10f;
            logicSet5.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet5.AddAction(new PlayAnimationLogicAction(4, 5, false), Types.Sequence.Serial);
            logicSet5.AddAction(new DelayLogicAction(PauseAfterProjectile, false), Types.Sequence.Serial);
            logicSet5.Tag = 2;
            LogicSet logicSet6 = new LogicSet(this);
            logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyNinjaIdle_Character", true, true), Types.Sequence.Serial);
            logicSet6.AddAction(new DelayLogicAction(PauseBeforeProjectile, false), Types.Sequence.Serial);
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyNinjaThrow_Character", false, false), Types.Sequence.Serial);
            logicSet6.AddAction(new PlayAnimationLogicAction(1, 3, false), Types.Sequence.Serial);
            logicSet6.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "Ninja_ThrowStar_01",
                "Ninja_ThrowStar_02",
                "Ninja_ThrowStar_03"
            }), Types.Sequence.Serial);
            projectileData.AngleOffset = 0f;
            logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.AngleOffset = -5f;
            logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.AngleOffset = 5f;
            logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.AngleOffset = -25f;
            logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.AngleOffset = 25f;
            logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet6.AddAction(new PlayAnimationLogicAction(4, 5, false), Types.Sequence.Serial);
            logicSet6.AddAction(new DelayLogicAction(PauseAfterProjectile, false), Types.Sequence.Serial);
            logicSet6.Tag = 2;
            LogicSet logicSet7 = new LogicSet(this);
            logicSet7.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet7.AddAction(new RunFunctionLogicAction(this, "CreateLog", null), Types.Sequence.Serial);
            logicSet7.AddAction(new DelayLogicAction(TeleportDelay, false), Types.Sequence.Serial);
            logicSet7.AddAction(new RunFunctionLogicAction(this, "CreateSmoke", null), Types.Sequence.Serial);
            logicSet7.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
            logicSet7.AddAction(new ChangePropertyLogicAction(this, "IsWeighted", true), Types.Sequence.Serial);
            logicSet7.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
            logicSet7.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
            logicSet7.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
            logicSet7.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet7.Tag = 2;
            LogicSet logicSet8 = new LogicSet(this);
            logicSet8.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet8.AddAction(new RunFunctionLogicAction(this, "CreateLog", null), Types.Sequence.Serial);
            logicSet8.AddAction(new DelayLogicAction(TeleportDelay, false), Types.Sequence.Serial);
            logicSet8.AddAction(new RunFunctionLogicAction(this, "CreateSmoke", null), Types.Sequence.Serial);
            projectileData.Target = null;
            logicSet8.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "Ninja_ThrowStar_01",
                "Ninja_ThrowStar_02",
                "Ninja_ThrowStar_03"
            }), Types.Sequence.Serial);
            projectileData.AngleOffset = 45f;
            logicSet8.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.AngleOffset = 135f;
            logicSet8.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.AngleOffset = -45f;
            logicSet8.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.AngleOffset = -135f;
            logicSet8.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            logicSet8.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
            logicSet8.AddAction(new ChangePropertyLogicAction(this, "IsWeighted", true), Types.Sequence.Serial);
            logicSet8.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
            logicSet8.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
            logicSet8.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
            logicSet8.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet8.Tag = 2;
            m_basicTeleportAttackLB.AddLogicSet(new LogicSet[] {
                logicSet7
            });
            m_expertTeleportAttackLB.AddLogicSet(new LogicSet[] {
                logicSet8
            });
            logicBlocksToDispose.Add(m_basicTeleportAttackLB);
            logicBlocksToDispose.Add(m_expertTeleportAttackLB);
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
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);
            LogicBlock arg_906_1 = m_generalCooldownLB;
            int[] array = new int[3];
            array[0] = 50;
            array[1] = 50;
            base.SetCooldownLogicBlock(arg_906_1, array);
            projectileData.Dispose();
            base.InitializeLogic();
        }

        protected override void RunBasicLogic() {
            switch (base.State) {
                case 0: {
                    bool arg_7D_1 = true;
                    LogicBlock arg_7D_2 = m_generalBasicLB;
                    int[] array = new int[4];
                    array[0] = 50;
                    array[1] = 50;
                    base.RunLogicBlock(arg_7D_1, arg_7D_2, array);
                    return;
                }
                case 1: {
                    bool arg_5D_1 = true;
                    LogicBlock arg_5D_2 = m_generalBasicLB;
                    int[] array2 = new int[4];
                    array2[0] = 65;
                    array2[1] = 35;
                    base.RunLogicBlock(arg_5D_1, arg_5D_2, array2);
                    return;
                }
                case 2:
                case 3:
                    base.RunLogicBlock(true, m_generalBasicLB, new[] {
                        40,
                        30,
                        0,
                        30
                    });
                    return;
                default:
                    return;
            }
        }

        protected override void RunAdvancedLogic() {
            switch (base.State) {
                case 0: {
                    bool arg_7D_1 = true;
                    LogicBlock arg_7D_2 = m_generalAdvancedLB;
                    int[] array = new int[4];
                    array[0] = 50;
                    array[1] = 50;
                    base.RunLogicBlock(arg_7D_1, arg_7D_2, array);
                    return;
                }
                case 1: {
                    bool arg_5D_1 = true;
                    LogicBlock arg_5D_2 = m_generalAdvancedLB;
                    int[] array2 = new int[4];
                    array2[0] = 65;
                    array2[1] = 35;
                    base.RunLogicBlock(arg_5D_1, arg_5D_2, array2);
                    return;
                }
                case 2:
                case 3:
                    base.RunLogicBlock(true, m_generalAdvancedLB, new[] {
                        40,
                        30,
                        0,
                        30
                    });
                    return;
                default:
                    return;
            }
        }

        protected override void RunExpertLogic() {
            switch (base.State) {
                case 0: {
                    bool arg_7D_1 = true;
                    LogicBlock arg_7D_2 = m_generalExpertLB;
                    int[] array = new int[4];
                    array[0] = 50;
                    array[1] = 50;
                    base.RunLogicBlock(arg_7D_1, arg_7D_2, array);
                    return;
                }
                case 1: {
                    bool arg_5D_1 = true;
                    LogicBlock arg_5D_2 = m_generalExpertLB;
                    int[] array2 = new int[4];
                    array2[0] = 65;
                    array2[1] = 35;
                    base.RunLogicBlock(arg_5D_1, arg_5D_2, array2);
                    return;
                }
                case 2:
                case 3:
                    base.RunLogicBlock(true, m_generalExpertLB, new[] {
                        40,
                        30,
                        0,
                        30
                    });
                    return;
                default:
                    return;
            }
        }

        protected override void RunMinibossLogic() {
            switch (base.State) {
                case 0: {
                    bool arg_7D_1 = true;
                    LogicBlock arg_7D_2 = m_generalBasicLB;
                    int[] array = new int[4];
                    array[0] = 50;
                    array[1] = 50;
                    base.RunLogicBlock(arg_7D_1, arg_7D_2, array);
                    return;
                }
                case 1: {
                    bool arg_5D_1 = true;
                    LogicBlock arg_5D_2 = m_generalBasicLB;
                    int[] array2 = new int[4];
                    array2[0] = 65;
                    array2[1] = 35;
                    base.RunLogicBlock(arg_5D_1, arg_5D_2, array2);
                    return;
                }
                case 2:
                case 3:
                    base.RunLogicBlock(true, m_generalBasicLB, new[] {
                        40,
                        30,
                        0,
                        30
                    });
                    return;
                default:
                    return;
            }
        }

        public override void Update(GameTime gameTime) {
            if (base.Y < m_levelScreen.CurrentRoom.Y)
                base.Y = m_levelScreen.CurrentRoom.Y;
            base.Update(gameTime);
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer) {
            if (m_target != null && m_target.CurrentHealth > 0 && m_currentActiveLB != m_basicTeleportAttackLB && m_currentActiveLB != m_expertTeleportAttackLB && CDGMath.RandomFloat(0f, 1f) <= ChanceToTeleport && m_closestCeiling != null) {
                m_closestCeiling = FindClosestCeiling();
                int num = this.TerrainBounds.Top - m_closestCeiling.Bounds.Bottom;
                if (m_closestCeiling != null && num > 150 && num < 700) {
                    m_currentActiveLB.StopLogicBlock();
                    if (base.Difficulty == GameTypes.EnemyDifficulty.EXPERT) {
                        base.RunLogicBlock(false, m_expertTeleportAttackLB, new[] {
                            100
                        });
                    }
                    else {
                        base.RunLogicBlock(false, m_basicTeleportAttackLB, new[] {
                            100
                        });
                    }
                    damage = (int)Math.Round((damage * (1f - m_teleportDamageReduc)), MidpointRounding.AwayFromZero);
                }
            }
            base.HitEnemy(damage, position, isPlayer);
        }

        private TerrainObj FindClosestCeiling() {
            int num = 2147483647;
            TerrainObj result = null;
            RoomObj currentRoom = m_levelScreen.CurrentRoom;
            foreach (TerrainObj current in currentRoom.TerrainObjList) {
                Rectangle b = new Rectangle(Bounds.Left, Bounds.Top - 2000, Bounds.Width, Bounds.Height + 2000);
                if (current.CollidesBottom && CollisionMath.Intersects(current.Bounds, b)) {
                    float num2 = 3.40282347E+38f;
                    if (current.Bounds.Bottom < this.TerrainBounds.Top)
                        num2 = (float)(this.TerrainBounds.Top - current.Bounds.Bottom);
                    if (num2 < num) {
                        num = (int)num2;
                        result = current;
                    }
                }
            }
            return result;
        }

        public void CreateLog() {
            m_log.Position = base.Position;
            m_smoke.Position = base.Position;
            m_smoke.Visible = true;
            m_log.Visible = true;
            m_log.Opacity = 1f;
            m_smoke.PlayAnimation(false);
            Tween.By(m_log, 0.1f, new Easing(Linear.EaseNone), new[] {
                "delay",
                "0.2",
                "Y",
                "10"
            });
            Tween.To(m_log, 0.2f, new Easing(Linear.EaseNone), new[] {
                "delay",
                "0.3",
                "Opacity",
                "0"
            });
            SoundManager.Play3DSound(this, m_target, "Ninja_Teleport");
            base.Visible = false;
            base.IsCollidable = false;
            base.IsWeighted = false;
            m_storedRoom = m_levelScreen.CurrentRoom;
        }

        public void CreateSmoke() {
            if (m_levelScreen.CurrentRoom == m_storedRoom && m_closestCeiling != null) {
                base.UpdateCollisionBoxes();
                base.Y = (float)m_closestCeiling.Bounds.Bottom + (base.Y - (float)this.TerrainBounds.Top);
                base.X = m_target.X;
                this.ChangeSprite("EnemyNinjaAttack_Character");
                base.Visible = true;
                base.AccelerationX = 0f;
                base.AccelerationY = 0f;
                base.CurrentSpeed = 0f;
                base.IsCollidable = true;
                m_smoke.Position = base.Position;
                m_smoke.Visible = true;
                m_smoke.PlayAnimation(false);
                m_closestCeiling = null;
            }
        }

        public override void Draw(Camera2D camera) {
            base.Draw(camera);
            m_log.Draw(camera);
            m_smoke.Draw(camera);
        }

        public override void Kill(bool giveXP = true) {
            m_smoke.Visible = false;
            m_log.Visible = false;
            base.Kill(giveXP);
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_storedRoom = null;
                m_smoke.Dispose();
                m_smoke = null;
                m_log.Dispose();
                m_log = null;
                m_closestCeiling = null;
                base.Dispose();
            }
        }
    }
}
