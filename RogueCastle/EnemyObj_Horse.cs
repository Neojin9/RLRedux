using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace RogueCastle {
    public class EnemyObj_Horse : EnemyObj {
        private float m_collisionCheckTimer = 0.5f;
        private float m_fireDistance = 110f;
        private float m_fireDropInterval = 0.075f;
        private float m_fireDropLifespan = 0.75f;
        private float m_fireDropTimer = 0.5f;
        private float m_fireRotationSpeed = 1.5f;
        private List<ProjectileObj> m_fireShieldList;
        private float m_fireShieldScale = 2.5f;
        private FrameSoundObj m_gallopSound;
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalCooldownLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();
        private LogicBlock m_generalMiniBossLB = new LogicBlock();
        private int m_numFireShieldObjs = 2;
        private LogicBlock m_turnLB = new LogicBlock();
        private bool m_turning;
        private int m_wallDistanceCheck = 430;

        public EnemyObj_Horse(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyHorseRun_Character", target, physicsManager, levelToAttachTo, difficulty) {
            Type = 10;
            m_fireShieldList = new List<ProjectileObj>();
        }

        private Rectangle WallCollisionPoint {
            get {
                if (base.HeadingX < 0f)
                    return new Rectangle((int)base.X - m_wallDistanceCheck, (int)base.Y, 2, 2);
                return new Rectangle((int)base.X + m_wallDistanceCheck, (int)base.Y, 2, 2);
            }
        }

        private Rectangle GroundCollisionPoint {
            get {
                if (base.HeadingX < 0f)
                    return new Rectangle((int)(base.X - (float)m_wallDistanceCheck * this.ScaleX), (int)(base.Y + 60f * this.ScaleY), 2, 2);
                return new Rectangle((int)(base.X + (float)m_wallDistanceCheck * this.ScaleX), (int)(base.Y + 60f * this.ScaleY), 2, 2);
            }
        }

        protected override void InitializeEV() {
            base.LockFlip = true;
            base.Name = "Headless Horse";
            MaxHealth = 30;
            base.Damage = 23;
            base.XPValue = 25;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 2;
            MoneyDropChance = 0.4f;
            base.Speed = 425f;
            this.TurnSpeed = 10f;
            ProjectileSpeed = 0f;
            base.JumpHeight = 900f;
            CooldownTime = 2f;
            base.AnimationDelay = 0.06666667f;
            AlwaysFaceTarget = false;
            CanFallOffLedges = false;
            base.CanBeKnockedBack = false;
            base.IsWeighted = true;
            this.Scale = EnemyEV.Horse_Basic_Scale;
            base.ProjectileScale = EnemyEV.Horse_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Horse_Basic_Tint;
            MeleeRadius = 700;
            ProjectileRadius = 1800;
            EngageRadius = 2100;
            ProjectileDamage = base.Damage;
            base.KnockBack = EnemyEV.Horse_Basic_KnockBack;
            switch (base.Difficulty) {
                case GameTypes.EnemyDifficulty.BASIC:
                    break;
                case GameTypes.EnemyDifficulty.ADVANCED:
                    base.Name = "Dark Stallion";
                    MaxHealth = 37;
                    base.Damage = 27;
                    base.XPValue = 75;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    base.Speed = 500f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 0f;
                    base.JumpHeight = 900f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.06666667f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = false;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Horse_Advanced_Scale;
                    base.ProjectileScale = EnemyEV.Horse_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Horse_Advanced_Tint;
                    MeleeRadius = 700;
                    EngageRadius = 2100;
                    ProjectileRadius = 1800;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Horse_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.EXPERT:
                    base.Name = "Night Mare";
                    MaxHealth = 60;
                    base.Damage = 30;
                    base.XPValue = 200;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 3;
                    MoneyDropChance = 1f;
                    base.Speed = 550f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 0f;
                    base.JumpHeight = 900f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.06666667f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = false;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Horse_Expert_Scale;
                    base.ProjectileScale = EnemyEV.Horse_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Horse_Expert_Tint;
                    MeleeRadius = 700;
                    ProjectileRadius = 1800;
                    EngageRadius = 2100;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Horse_Expert_KnockBack;
                    return;
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    base.Name = "My Little Pony";
                    MaxHealth = 800;
                    base.Damage = 40;
                    base.XPValue = 600;
                    MinMoneyDropAmount = 10;
                    MaxMoneyDropAmount = 15;
                    MoneyDropChance = 1f;
                    base.Speed = 900f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 0f;
                    base.JumpHeight = 900f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.06666667f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = false;
                    base.IsWeighted = true;
                    this.Scale = EnemyEV.Horse_Miniboss_Scale;
                    base.ProjectileScale = EnemyEV.Horse_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Horse_Miniboss_Tint;
                    MeleeRadius = 700;
                    ProjectileRadius = 1800;
                    EngageRadius = 2100;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Horse_Miniboss_KnockBack;
                    return;
                default:
                    return;
            }
        }

        protected override void InitializeLogic() {
            LogicSet logicSet = new LogicSet(this);
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyHorseRun_Character", true, true), Types.Sequence.Serial);
            logicSet.AddAction(new MoveDirectionLogicAction(new Vector2(-1f, 0f), -1f), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
            LogicSet logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyHorseRun_Character", true, true), Types.Sequence.Serial);
            logicSet2.AddAction(new MoveDirectionLogicAction(new Vector2(1f, 0f), -1f), Types.Sequence.Serial);
            logicSet2.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
            LogicSet logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyHorseTurn_Character", true, true), Types.Sequence.Serial);
            logicSet3.AddAction(new MoveDirectionLogicAction(new Vector2(-1f, 0f), -1f), Types.Sequence.Serial);
            logicSet3.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
            logicSet3.AddAction(new ChangePropertyLogicAction(this, "Flip", SpriteEffects.None), Types.Sequence.Serial);
            logicSet3.AddAction(new RunFunctionLogicAction(this, "ResetTurn", new object[0]), Types.Sequence.Serial);
            LogicSet logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyHorseTurn_Character", true, true), Types.Sequence.Serial);
            logicSet4.AddAction(new MoveDirectionLogicAction(new Vector2(1f, 0f), -1f), Types.Sequence.Serial);
            logicSet4.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
            logicSet4.AddAction(new ChangePropertyLogicAction(this, "Flip", SpriteEffects.FlipHorizontally), Types.Sequence.Serial);
            logicSet4.AddAction(new RunFunctionLogicAction(this, "ResetTurn", new object[0]), Types.Sequence.Serial);
            LogicSet logicSet5 = new LogicSet(this);
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyHorseRun_Character", true, true), Types.Sequence.Serial);
            logicSet5.AddAction(new MoveDirectionLogicAction(new Vector2(-1f, 0f), -1f), Types.Sequence.Serial);
            ThrowStandingProjectiles(logicSet5, true);
            LogicSet logicSet6 = new LogicSet(this);
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyHorseRun_Character", true, true), Types.Sequence.Serial);
            logicSet6.AddAction(new MoveDirectionLogicAction(new Vector2(1f, 0f), -1f), Types.Sequence.Serial);
            ThrowStandingProjectiles(logicSet6, true);
            m_generalBasicLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2
            });
            m_turnLB.AddLogicSet(new LogicSet[] {
                logicSet4,
                logicSet3
            });
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_turnLB);
            m_gallopSound = new FrameSoundObj(this, m_target, 2, new[] {
                "Enemy_Horse_Gallop_01",
                "Enemy_Horse_Gallop_02",
                "Enemy_Horse_Gallop_03"
            });
            base.InitializeLogic();
        }

        private void ThrowStandingProjectiles(LogicSet ls, bool useBossProjectile = false) {
            ProjectileData projectileData = new ProjectileData(this) {
                SpriteName = "SpellDamageShield_Sprite",
                SourceAnchor = new Vector2(0f, 60f),
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = base.Damage,
                AngleOffset = 0f,
                Angle = new Vector2(0f, 0f),
                CollidesWithTerrain = false,
                Scale = base.ProjectileScale,
                Lifespan = 0.75f
            };
            ls.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new[] {
                "FairyAttack1"
            }), Types.Sequence.Serial);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            ls.AddAction(new DelayLogicAction(0.075f, false), Types.Sequence.Serial);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            ls.AddAction(new DelayLogicAction(0.075f, false), Types.Sequence.Serial);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
            projectileData.Dispose();
        }

        protected override void RunBasicLogic() {
            switch (base.State) {
                case 0:
                case 1:
                case 2:
                case 3:
                    if (Flip == SpriteEffects.FlipHorizontally) {
                        bool arg_3C_1 = true;
                        LogicBlock arg_3C_2 = m_generalBasicLB;
                        int[] array = new int[2];
                        array[0] = 100;
                        base.RunLogicBlock(arg_3C_1, arg_3C_2, array);
                        return;
                    }
                    base.RunLogicBlock(true, m_generalBasicLB, new[] {
                        0,
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
                case 1:
                case 2:
                case 3:
                    if (Flip == SpriteEffects.FlipHorizontally) {
                        bool arg_3C_1 = true;
                        LogicBlock arg_3C_2 = m_generalBasicLB;
                        int[] array = new int[2];
                        array[0] = 100;
                        base.RunLogicBlock(arg_3C_1, arg_3C_2, array);
                        return;
                    }
                    base.RunLogicBlock(true, m_generalBasicLB, new[] {
                        0,
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
                case 1:
                case 2:
                case 3:
                    if (Flip == SpriteEffects.FlipHorizontally) {
                        bool arg_3C_1 = true;
                        LogicBlock arg_3C_2 = m_generalBasicLB;
                        int[] array = new int[2];
                        array[0] = 100;
                        base.RunLogicBlock(arg_3C_1, arg_3C_2, array);
                        return;
                    }
                    base.RunLogicBlock(true, m_generalBasicLB, new[] {
                        0,
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
                case 1:
                case 2:
                case 3:
                    if (Flip == SpriteEffects.FlipHorizontally) {
                        bool arg_3C_1 = true;
                        LogicBlock arg_3C_2 = m_generalBasicLB;
                        int[] array = new int[2];
                        array[0] = 100;
                        base.RunLogicBlock(arg_3C_1, arg_3C_2, array);
                        return;
                    }
                    base.RunLogicBlock(true, m_generalBasicLB, new[] {
                        0,
                        100
                    });
                    return;
                default:
                    return;
            }
        }

        public override void Update(GameTime gameTime) {
            if (m_target.AttachedLevel.CurrentRoom.Name != "Ending")
                m_gallopSound.Update();
            float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (base.Difficulty >= GameTypes.EnemyDifficulty.ADVANCED && m_fireDropTimer > 0f) {
                m_fireDropTimer -= num;
                if (m_fireDropTimer <= 0f) {
                    DropFireProjectile();
                    m_fireDropTimer = m_fireDropInterval;
                }
            }
            if (base.Difficulty == GameTypes.EnemyDifficulty.EXPERT && !base.IsPaused && m_fireShieldList.Count < 1)
                CastFireShield(m_numFireShieldObjs);
            if ((Bounds.Left < m_levelScreen.CurrentRoom.Bounds.Left || Bounds.Right > m_levelScreen.CurrentRoom.Bounds.Right) && m_collisionCheckTimer <= 0f)
                TurnHorse();
            Rectangle b = default(Rectangle);
            Rectangle b2 = default(Rectangle);
            if (Flip == SpriteEffects.FlipHorizontally) {
                b = new Rectangle(Bounds.Left - 10, Bounds.Bottom + 20, 5, 5);
                b2 = new Rectangle(Bounds.Right + 50, Bounds.Bottom - 20, 5, 5);
            }
            else {
                b = new Rectangle(Bounds.Right + 10, Bounds.Bottom + 20, 5, 5);
                b2 = new Rectangle(Bounds.Left - 50, Bounds.Bottom - 20, 5, 5);
            }
            bool flag = true;
            foreach (TerrainObj current in m_levelScreen.CurrentRoom.TerrainObjList) {
                if (CollisionMath.Intersects(current.Bounds, b) || CollisionMath.Intersects(current.Bounds, b2)) {
                    flag = false;
                    break;
                }
            }
            if (flag)
                TurnHorse();
            if (m_collisionCheckTimer > 0f)
                m_collisionCheckTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        public void ResetTurn() {
            m_turning = false;
        }

        private void DropFireProjectile() {
            base.UpdateCollisionBoxes();
            ProjectileData projectileData = new ProjectileData(this) {
                SpriteName = "SpellDamageShield_Sprite",
                SourceAnchor = new Vector2(0f, (float)Bounds.Bottom - base.Y - 10f),
                Speed = new Vector2(0f, 0f),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = base.Damage,
                Angle = new Vector2(0f, 0f),
                AngleOffset = 0f,
                CollidesWithTerrain = false,
                Scale = base.ProjectileScale,
                Lifespan = m_fireDropLifespan,
                LockPosition = true
            };
            m_levelScreen.ProjectileManager.FireProjectile(projectileData);
            projectileData.Dispose();
        }

        private void CastFireShield(int numFires) {
            ProjectileData data = new ProjectileData(this) {
                SpriteName = "SpellDamageShield_Sprite",
                SourceAnchor = new Vector2(0f, (float)Bounds.Bottom - base.Y - 10f),
                Speed = new Vector2(m_fireRotationSpeed, m_fireRotationSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Target = this,
                Damage = base.Damage,
                Angle = new Vector2(0f, 0f),
                AngleOffset = 0f,
                CollidesWithTerrain = false,
                Scale = new Vector2(m_fireShieldScale, m_fireShieldScale),
                Lifespan = 999999f,
                DestroysWithEnemy = false,
                LockPosition = true
            };
            SoundManager.PlaySound("Cast_FireShield");
            float fireDistance = m_fireDistance;
            for (int i = 0; i < numFires; i++) {
                float altX = 360f / numFires * i;
                ProjectileObj projectileObj = m_levelScreen.ProjectileManager.FireProjectile(data);
                projectileObj.AltX = altX;
                projectileObj.AltY = fireDistance;
                projectileObj.Spell = 11;
                projectileObj.CanBeFusRohDahed = false;
                projectileObj.AccelerationXEnabled = false;
                projectileObj.AccelerationYEnabled = false;
                projectileObj.IgnoreBoundsCheck = true;
                m_fireShieldList.Add(projectileObj);
            }
        }

        private void TurnHorse() {
            if (!m_turning) {
                m_turning = true;
                if (base.HeadingX < 0f) {
                    m_currentActiveLB.StopLogicBlock();
                    base.RunLogicBlock(false, m_turnLB, new[] {
                        0,
                        100
                    });
                }
                else {
                    m_currentActiveLB.StopLogicBlock();
                    bool arg_63_1 = false;
                    LogicBlock arg_63_2 = m_turnLB;
                    int[] array = new int[2];
                    array[0] = 100;
                    base.RunLogicBlock(arg_63_1, arg_63_2, array);
                }
                m_collisionCheckTimer = 0.5f;
            }
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType) {
            TerrainObj terrainObj = otherBox.AbsParent as TerrainObj;
            if (otherBox.AbsParent.Bounds.Top < this.TerrainBounds.Bottom - 20 && terrainObj != null && terrainObj.CollidesLeft && terrainObj.CollidesRight && terrainObj.CollidesBottom && collisionResponseType == 1 && otherBox.AbsRotation == 0f && m_collisionCheckTimer <= 0f && CollisionMath.CalculateMTD(thisBox.AbsRect, otherBox.AbsRect).X != 0f)
                TurnHorse();
            base.CollisionResponse(thisBox, otherBox, collisionResponseType);
        }

        public override void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer) {
            SoundManager.Play3DSound(this, m_target, new[] {
                "Enemy_Horse_Hit_01",
                "Enemy_Horse_Hit_02",
                "Enemy_Horse_Hit_03"
            });
            base.HitEnemy(damage, collisionPt, isPlayer);
        }

        public override void Kill(bool giveXP = true) {
            foreach (ProjectileObj current in m_fireShieldList)
                current.RunDestroyAnimation(false);
            m_fireShieldList.Clear();
            SoundManager.Play3DSound(this, m_target, "Enemy_Horse_Dead");
            base.Kill(giveXP);
        }

        public override void ResetState() {
            m_fireShieldList.Clear();
            base.ResetState();
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                if (m_gallopSound != null)
                    m_gallopSound.Dispose();
                m_gallopSound = null;
                base.Dispose();
            }
        }
    }
}
