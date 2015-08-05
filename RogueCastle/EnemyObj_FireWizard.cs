using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace RogueCastle {
    public class EnemyObj_FireWizard : EnemyObj {
        private float MoveDuration = 1f;
        private float SpellDelay = 0.7f;
        private float SpellInterval = 0.5f;
        private float TeleportDelay = 0.5f;
        private float TeleportDuration = 1f;
        private float m_fireParticleEffectCounter = 0.5f;
        private ProjectileObj m_fireballSummon;
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalCooldownLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();
        private Vector2 m_spellOffset = new Vector2(40f, -80f);

        public EnemyObj_FireWizard(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyWizardIdle_Character", target, physicsManager, levelToAttachTo, difficulty) {
            Type = 9;
            base.PlayAnimation(true);
            TintablePart = this._objectList[0];
        }

        protected override void InitializeEV() {
            SpellInterval = 0.5f;
            base.Name = "Flamelock";
            MaxHealth = 32;
            base.Damage = 20;
            base.XPValue = 175;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 2;
            MoneyDropChance = 0.4f;
            base.Speed = 270f;
            this.TurnSpeed = 0.04f;
            ProjectileSpeed = 650f;
            base.JumpHeight = 300f;
            CooldownTime = 1.25f;
            base.AnimationDelay = 0.1f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = true;
            base.CanBeKnockedBack = true;
            base.IsWeighted = false;
            this.Scale = EnemyEV.FireWizard_Basic_Scale;
            base.ProjectileScale = EnemyEV.FireWizard_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.FireWizard_Basic_Tint;
            MeleeRadius = 225;
            ProjectileRadius = 700;
            EngageRadius = 900;
            ProjectileDamage = base.Damage;
            base.KnockBack = EnemyEV.FireWizard_Basic_KnockBack;
            switch (base.Difficulty) {
                case GameTypes.EnemyDifficulty.BASIC:
                    break;
                case GameTypes.EnemyDifficulty.ADVANCED:
                    SpellInterval = 0.15f;
                    base.Name = "Blazelock";
                    MaxHealth = 45;
                    base.Damage = 28;
                    base.XPValue = 200;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    base.Speed = 270f;
                    this.TurnSpeed = 0.04f;
                    ProjectileSpeed = 650f;
                    base.JumpHeight = 300f;
                    CooldownTime = 1.25f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = false;
                    this.Scale = EnemyEV.FireWizard_Advanced_Scale;
                    base.ProjectileScale = EnemyEV.FireWizard_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.FireWizard_Advanced_Tint;
                    MeleeRadius = 225;
                    EngageRadius = 900;
                    ProjectileRadius = 700;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.FireWizard_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.EXPERT:
                    m_spellOffset = new Vector2(40f, -130f);
                    SpellDelay = 1f;
                    SpellInterval = 1f;
                    base.Name = "Sollock";
                    MaxHealth = 72;
                    base.Damage = 35;
                    base.XPValue = 400;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 3;
                    MoneyDropChance = 1f;
                    base.Speed = 270f;
                    this.TurnSpeed = 0.04f;
                    ProjectileSpeed = 300f;
                    base.JumpHeight = 300f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = false;
                    this.Scale = EnemyEV.FireWizard_Expert_Scale;
                    base.ProjectileScale = EnemyEV.FireWizard_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.FireWizard_Expert_Tint;
                    MeleeRadius = 225;
                    ProjectileRadius = 700;
                    EngageRadius = 900;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.FireWizard_Expert_KnockBack;
                    return;
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    base.Name = "Sol Mage";
                    MaxHealth = 240;
                    base.Damage = 40;
                    base.XPValue = 1000;
                    MinMoneyDropAmount = 18;
                    MaxMoneyDropAmount = 25;
                    MoneyDropChance = 1f;
                    base.Speed = 270f;
                    this.TurnSpeed = 0.04f;
                    ProjectileSpeed = 650f;
                    base.JumpHeight = 300f;
                    CooldownTime = 1.25f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    base.CanBeKnockedBack = true;
                    base.IsWeighted = false;
                    this.Scale = EnemyEV.FireWizard_Miniboss_Scale;
                    base.ProjectileScale = EnemyEV.FireWizard_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.FireWizard_Miniboss_Tint;
                    MeleeRadius = 225;
                    ProjectileRadius = 700;
                    EngageRadius = 900;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.FireWizard_Miniboss_KnockBack;
                    return;
                default:
                    return;
            }
        }

        protected override void InitializeLogic() {
            LogicSet logicSet = new LogicSet(this);
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyWizardIdle_Character", true, true), Types.Sequence.Serial);
            logicSet.AddAction(new ChaseLogicAction(m_target, new Vector2(-255f, -175f), new Vector2(255f, -75f), true, MoveDuration, -1f), Types.Sequence.Serial);
            LogicSet logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyWizardIdle_Character", true, true), Types.Sequence.Serial);
            logicSet2.AddAction(new ChaseLogicAction(m_target, false, MoveDuration, -1f), Types.Sequence.Serial);
            LogicSet logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyWizardIdle_Character", true, true), Types.Sequence.Serial);
            logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet3.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
            LogicSet logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet4.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyWizardSpell_Character", true, true), Types.Sequence.Serial);
            logicSet4.AddAction(new PlayAnimationLogicAction("Start", "BeforeSpell", false), Types.Sequence.Serial);
            logicSet4.AddAction(new RunFunctionLogicAction(this, "SummonFireball", null), Types.Sequence.Serial);
            logicSet4.AddAction(new DelayLogicAction(SpellDelay, false), Types.Sequence.Serial);
            logicSet4.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
            logicSet4.AddAction(new DelayLogicAction(SpellInterval, false), Types.Sequence.Serial);
            logicSet4.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
            logicSet4.AddAction(new DelayLogicAction(SpellInterval, false), Types.Sequence.Serial);
            logicSet4.AddAction(new RunFunctionLogicAction(this, "ResetFireball", null), Types.Sequence.Serial);
            logicSet4.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
            logicSet4.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
            logicSet4.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet4.Tag = 2;
            LogicSet logicSet5 = new LogicSet(this);
            logicSet5.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet5.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyWizardSpell_Character", true, true), Types.Sequence.Serial);
            logicSet5.AddAction(new PlayAnimationLogicAction("Start", "BeforeSpell", false), Types.Sequence.Serial);
            logicSet5.AddAction(new RunFunctionLogicAction(this, "SummonFireball", null), Types.Sequence.Serial);
            logicSet5.AddAction(new DelayLogicAction(SpellDelay, false), Types.Sequence.Serial);
            logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
            logicSet5.AddAction(new DelayLogicAction(SpellInterval, false), Types.Sequence.Serial);
            logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
            logicSet5.AddAction(new DelayLogicAction(SpellInterval, false), Types.Sequence.Serial);
            logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
            logicSet5.AddAction(new DelayLogicAction(SpellInterval, false), Types.Sequence.Serial);
            logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
            logicSet5.AddAction(new DelayLogicAction(SpellInterval, false), Types.Sequence.Serial);
            logicSet5.AddAction(new RunFunctionLogicAction(this, "ResetFireball", null), Types.Sequence.Serial);
            logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
            logicSet5.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
            logicSet5.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet5.Tag = 2;
            LogicSet logicSet6 = new LogicSet(this);
            logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet6.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyWizardSpell_Character", true, true), Types.Sequence.Serial);
            logicSet6.AddAction(new PlayAnimationLogicAction("Start", "BeforeSpell", false), Types.Sequence.Serial);
            logicSet6.AddAction(new RunFunctionLogicAction(this, "SummonFireball", null), Types.Sequence.Serial);
            logicSet6.AddAction(new DelayLogicAction(SpellDelay, false), Types.Sequence.Serial);
            logicSet6.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
            logicSet6.AddAction(new DelayLogicAction(SpellInterval, false), Types.Sequence.Serial);
            logicSet6.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
            logicSet6.AddAction(new DelayLogicAction(SpellInterval, false), Types.Sequence.Serial);
            logicSet6.AddAction(new RunFunctionLogicAction(this, "ResetFireball", null), Types.Sequence.Serial);
            logicSet6.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
            logicSet6.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
            logicSet6.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet6.Tag = 2;
            LogicSet logicSet7 = new LogicSet(this);
            logicSet7.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet7.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
            logicSet7.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.0333333351f), Types.Sequence.Serial);
            logicSet7.AddAction(new ChangeSpriteLogicAction("EnemyWizardTeleportOut_Character", false, false), Types.Sequence.Serial);
            logicSet7.AddAction(new PlayAnimationLogicAction("Start", "BeforeTeleport", false), Types.Sequence.Serial);
            logicSet7.AddAction(new DelayLogicAction(TeleportDelay, false), Types.Sequence.Serial);
            logicSet7.AddAction(new PlayAnimationLogicAction("TeleportStart", "End", false), Types.Sequence.Serial);
            logicSet7.AddAction(new ChangePropertyLogicAction(this, "IsCollidable", false), Types.Sequence.Serial);
            logicSet7.AddAction(new DelayLogicAction(TeleportDuration, false), Types.Sequence.Serial);
            logicSet7.AddAction(new TeleportLogicAction(m_target, new Vector2(-400f, -400f), new Vector2(400f, 400f)), Types.Sequence.Serial);
            logicSet7.AddAction(new ChangePropertyLogicAction(this, "IsCollidable", true), Types.Sequence.Serial);
            logicSet7.AddAction(new ChangeSpriteLogicAction("EnemyWizardTeleportIn_Character", true, false), Types.Sequence.Serial);
            logicSet7.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.1f), Types.Sequence.Serial);
            logicSet7.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
            logicSet7.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
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
                logicSet5,
                logicSet7
            });
            m_generalExpertLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2,
                logicSet3,
                logicSet6,
                logicSet7
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
            LogicBlock arg_738_1 = m_generalCooldownLB;
            int[] array = new int[3];
            array[0] = 100;
            base.SetCooldownLogicBlock(arg_738_1, array);
            base.InitializeLogic();
        }

        protected override void RunBasicLogic() {
            switch (base.State) {
                case 0: {
                    bool arg_6E_1 = true;
                    LogicBlock arg_6E_2 = m_generalBasicLB;
                    int[] array = new int[5];
                    array[2] = 100;
                    base.RunLogicBlock(arg_6E_1, arg_6E_2, array);
                    return;
                }
                case 1: {
                    bool arg_53_1 = true;
                    LogicBlock arg_53_2 = m_generalBasicLB;
                    int[] array2 = new int[5];
                    array2[0] = 100;
                    base.RunLogicBlock(arg_53_1, arg_53_2, array2);
                    return;
                }
                case 2:
                case 3: {
                    bool arg_38_1 = true;
                    LogicBlock arg_38_2 = m_generalBasicLB;
                    int[] array3 = new int[5];
                    array3[0] = 40;
                    array3[3] = 60;
                    base.RunLogicBlock(arg_38_1, arg_38_2, array3);
                    return;
                }
                default:
                    return;
            }
        }

        protected override void RunAdvancedLogic() {
            switch (base.State) {
                case 0: {
                    bool arg_6E_1 = true;
                    LogicBlock arg_6E_2 = m_generalAdvancedLB;
                    int[] array = new int[5];
                    array[2] = 100;
                    base.RunLogicBlock(arg_6E_1, arg_6E_2, array);
                    return;
                }
                case 1: {
                    bool arg_53_1 = true;
                    LogicBlock arg_53_2 = m_generalAdvancedLB;
                    int[] array2 = new int[5];
                    array2[0] = 100;
                    base.RunLogicBlock(arg_53_1, arg_53_2, array2);
                    return;
                }
                case 2:
                case 3: {
                    bool arg_38_1 = true;
                    LogicBlock arg_38_2 = m_generalAdvancedLB;
                    int[] array3 = new int[5];
                    array3[0] = 40;
                    array3[3] = 60;
                    base.RunLogicBlock(arg_38_1, arg_38_2, array3);
                    return;
                }
                default:
                    return;
            }
        }

        protected override void RunExpertLogic() {
            switch (base.State) {
                case 0: {
                    bool arg_6E_1 = true;
                    LogicBlock arg_6E_2 = m_generalExpertLB;
                    int[] array = new int[5];
                    array[2] = 100;
                    base.RunLogicBlock(arg_6E_1, arg_6E_2, array);
                    return;
                }
                case 1: {
                    bool arg_53_1 = true;
                    LogicBlock arg_53_2 = m_generalExpertLB;
                    int[] array2 = new int[5];
                    array2[0] = 100;
                    base.RunLogicBlock(arg_53_1, arg_53_2, array2);
                    return;
                }
                case 2:
                case 3: {
                    bool arg_38_1 = true;
                    LogicBlock arg_38_2 = m_generalExpertLB;
                    int[] array3 = new int[5];
                    array3[0] = 40;
                    array3[3] = 60;
                    base.RunLogicBlock(arg_38_1, arg_38_2, array3);
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
                case 3:
                    return;
            }
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            if (m_fireballSummon != null) {
                if (Flip == SpriteEffects.None)
                    m_fireballSummon.Position = new Vector2(base.X + m_spellOffset.X, base.Y + m_spellOffset.Y);
                else
                    m_fireballSummon.Position = new Vector2(base.X - m_spellOffset.X, base.Y + m_spellOffset.Y);
            }
            if (m_fireParticleEffectCounter > 0f) {
                m_fireParticleEffectCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (m_fireParticleEffectCounter <= 0f) {
                    m_levelScreen.ImpactEffectPool.DisplayFireParticleEffect(this);
                    m_fireParticleEffectCounter = 0.15f;
                }
            }
        }

        public void CastFireball() {
            ProjectileData projectileData = new ProjectileData(this) {
                SpriteName = "WizardFireballProjectile_Sprite",
                SourceAnchor = m_spellOffset,
                Target = m_target,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = base.Damage,
                AngleOffset = 0f,
                CollidesWithTerrain = false,
                Scale = base.ProjectileScale
            };
            if (base.Difficulty == GameTypes.EnemyDifficulty.ADVANCED)
                projectileData.AngleOffset = (float)CDGMath.RandomInt(-25, 25);
            if (base.Difficulty == GameTypes.EnemyDifficulty.EXPERT) {
                projectileData.SpriteName = "GhostBossProjectile_Sprite";
                projectileData.CollidesWithTerrain = false;
            }
            SoundManager.Play3DSound(this, m_target, new[] {
                "FireWizard_Attack_01",
                "FireWizard_Attack_02",
                "FireWizard_Attack_03",
                "FireWizard_Attack_04"
            });
            ProjectileObj projectileObj = m_levelScreen.ProjectileManager.FireProjectile(projectileData);
            projectileObj.Rotation = 0f;
            if (base.Difficulty != GameTypes.EnemyDifficulty.EXPERT) {
                Tween.RunFunction(0.15f, this, "ChangeFireballState", new object[] {
                    projectileObj
                });
            }
        }

        public void ChangeFireballState(ProjectileObj fireball) {
            fireball.CollidesWithTerrain = true;
        }

        public void SummonFireball() {
            ResetFireball();
            ProjectileData projectileData = new ProjectileData(this) {
                SpriteName = "WizardFireballProjectile_Sprite",
                SourceAnchor = m_spellOffset,
                Target = m_target,
                Speed = new Vector2(0f, 0f),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = base.Damage,
                AngleOffset = 0f,
                CollidesWithTerrain = false,
                DestroysWithEnemy = false,
                Scale = base.ProjectileScale
            };
            if (base.Difficulty == GameTypes.EnemyDifficulty.EXPERT) {
                projectileData.SpriteName = "GhostBossProjectile_Sprite";
                projectileData.CollidesWithTerrain = false;
            }
            SoundManager.Play3DSound(this, m_target, "Fire_Wizard_Form");
            m_fireballSummon = m_levelScreen.ProjectileManager.FireProjectile(projectileData);
            m_fireballSummon.Opacity = 0f;
            m_fireballSummon.Scale = Vector2.Zero;
            m_fireballSummon.AnimationDelay = 0.1f;
            m_fireballSummon.PlayAnimation(true);
            m_fireballSummon.Rotation = 0f;
            object arg_186_0 = m_fireballSummon;
            float arg_186_1 = 0.5f;
            Easing arg_186_2 = new Easing(Back.EaseOut);
            string[] array = new string[6];
            array[0] = "Opacity";
            array[1] = "1";
            array[2] = "ScaleX";
            string[] arg_165_0 = array;
            int arg_165_1 = 3;
            float x = base.ProjectileScale.X;
            arg_165_0[arg_165_1] = x.ToString();
            array[4] = "ScaleY";
            string[] arg_184_0 = array;
            int arg_184_1 = 5;
            float y = base.ProjectileScale.Y;
            arg_184_0[arg_184_1] = y.ToString();
            Tween.To(arg_186_0, arg_186_1, arg_186_2, array);
            projectileData.Dispose();
        }

        public void ResetFireball() {
            if (m_fireballSummon != null) {
                m_levelScreen.ProjectileManager.DestroyProjectile(m_fireballSummon);
                m_fireballSummon = null;
            }
        }

        public override void Kill(bool giveXP = true) {
            if (m_currentActiveLB != null && m_currentActiveLB.IsActive) {
                m_currentActiveLB.StopLogicBlock();
                ResetFireball();
            }
            base.Kill(giveXP);
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType) {
            if (otherBox.AbsParent is PlayerObj)
                base.CurrentSpeed = 0f;
            if (collisionResponseType != 1) {
                base.CollisionResponse(thisBox, otherBox, collisionResponseType);
                return;
            }
            if (!(otherBox.AbsParent is PlayerObj)) {
                IPhysicsObj physicsObj = otherBox.AbsParent as IPhysicsObj;
                if (physicsObj.CollidesBottom && physicsObj.CollidesTop && physicsObj.CollidesLeft && physicsObj.CollidesRight)
                    base.Position += CollisionMath.RotatedRectIntersectsMTD(thisBox.AbsRect, thisBox.AbsRotation, Vector2.Zero, otherBox.AbsRect, otherBox.AbsRotation, Vector2.Zero);
            }
        }

        public override void ResetState() {
            ResetFireball();
            base.ResetState();
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_fireballSummon = null;
                base.Dispose();
            }
        }
    }
}
