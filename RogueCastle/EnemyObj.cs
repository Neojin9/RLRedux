using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using Tweener;


namespace RogueCastle {
    public abstract class EnemyObj : CharacterObj, IDealsDamageObj {
        protected const int STATE_WANDER = 0;
        protected const int STATE_ENGAGE = 1;
        protected const int STATE_PROJECTILE_ENGAGE = 2;
        protected const int STATE_MELEE_ENGAGE = 3;
        protected bool AlwaysFaceTarget;
        protected bool CanFallOffLedges = true;
        protected float CooldownTime;
        protected int DamageGainPerLevel;
        protected float DistanceToPlayer;
        protected int EngageRadius;
        protected int HealthGainPerLevel;
        public float InitialLogicDelay;
        protected float ItemDropChance;
        protected int MaxMoneyDropAmount;
        protected float MaxMoneyGainPerLevel;
        protected int MeleeRadius;
        protected int MinMoneyDropAmount;
        protected float MinMoneyGainPerLevel;
        protected float MoneyDropChance;
        protected int ProjectileDamage = 5;
        protected int ProjectileRadius;
        protected float ProjectileSpeed = 100f;
        public bool SaveToFile = true;
        protected float StatLevelDMGMod;
        protected float StatLevelHPMod;
        protected float StatLevelXPMod;
        protected GameObj TintablePart;
        public byte Type;
        protected int XPBonusPerLevel;
        protected List<LogicBlock> logicBlocksToDispose;
        protected bool m_bossVersionKilled;
        private LogicBlock m_cooldownLB;
        private int[] m_cooldownParams;
        private float m_cooldownTimer;
        protected LogicBlock m_currentActiveLB;
        protected int m_damage;
        private Texture2D m_engageRadiusTexture;
        protected TweenObject m_flipTween;
        protected float m_initialDelayCounter;
        protected float m_invincibilityTime = 0.4f;
        protected float m_invincibleCounter;
        protected float m_invincibleCounterProjectile;
        private bool m_isPaused;
        private int m_level;
        private Texture2D m_meleeRadiusTexture;
        private int m_numTouchingGrounds;
        private Texture2D m_projectileRadiusTexture;
        protected string m_resetSpriteName;
        private bool m_runCooldown;
        protected bool m_saveToEnemiesKilledList = true;
        protected PlayerObj m_target;
        private LogicBlock m_walkingLB;
        protected int m_xpValue;

        public EnemyObj(string spriteName, PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base(spriteName, physicsManager, levelToAttachTo) {
            base.DisableCollisionBoxRotations = true;
            Type = 0;
            base.CollisionTypeTag = 3;
            m_target = target;
            m_walkingLB = new LogicBlock();
            m_currentActiveLB = new LogicBlock();
            m_cooldownLB = new LogicBlock();
            logicBlocksToDispose = new List<LogicBlock>();
            m_resetSpriteName = spriteName;
            Difficulty = difficulty;
            ProjectileScale = new Vector2(1f, 1f);
            base.PlayAnimation(true);
            PlayAnimationOnRestart = true;
            base.OutlineWidth = 2;
            GivesLichHealth = true;
            DropsItem = true;
        }

        public Vector2 ProjectileScale { get; internal set; }
        public bool Procedural { get; set; }
        public bool NonKillable { get; set; }
        public bool GivesLichHealth { get; set; }
        public bool IsDemented { get; set; }

        public int Level {
            get { return m_level; }
            set {
                m_level = value;
                if (m_level < 0)
                    m_level = 0;
            }
        }

        protected float InvincibilityTime {
            get { return m_invincibilityTime; }
        }

        public GameTypes.EnemyDifficulty Difficulty { get; internal set; }
        public bool IsProcedural { get; set; }
        public bool PlayAnimationOnRestart { get; set; }
        public bool DropsItem { get; set; }

        private Rectangle GroundCollisionRect {
            get { return new Rectangle(Bounds.X - 10, Bounds.Y, this.Width + 20, this.Height + 10); }
        }

        private Rectangle RotatedGroundCollisionRect {
            get { return new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height + 40); }
        }

        public override Rectangle Bounds {
            get {
                if (base.IsWeighted)
                    return this.TerrainBounds;
                return base.Bounds;
            }
        }

        public override int MaxHealth {
            get { return base.MaxHealth + HealthGainPerLevel * (Level - 1); }
            internal set { base.MaxHealth = value; }
        }

        public int XPValue {
            get { return m_xpValue + XPBonusPerLevel * (Level - 1); }
            internal set { m_xpValue = value; }
        }

        public string ResetSpriteName {
            get { return m_resetSpriteName; }
        }

        public bool IsPaused {
            get { return m_isPaused; }
        }

        public override SpriteEffects Flip {
            get { return base.Flip; }
            set {
                if ((Game.PlayerStats.Traits.X == 18f || Game.PlayerStats.Traits.Y == 18f) && Flip != value && m_levelScreen != null) {
                    if (m_flipTween != null && m_flipTween.TweenedObject == this && m_flipTween.Active)
                        m_flipTween.StopTween(false);
                    float scaleY = this.ScaleY;
                    this.ScaleX = 0f;
                    m_flipTween = Tween.To(this, 0.15f, new Easing(Tween.EaseNone), new[] {
                        "ScaleX",
                        scaleY.ToString()
                    });
                }
                base.Flip = value;
            }
        }

        #region IDealsDamageObj Members

        public int Damage {
            get { return m_damage + DamageGainPerLevel * (Level - 1); }
            internal set { m_damage = value; }
        }

        #endregion

        private void InitializeBaseEV() {
            base.Speed = 1f;
            MaxHealth = 10;
            EngageRadius = 400;
            ProjectileRadius = 200;
            MeleeRadius = 50;
            base.KnockBack = Vector2.Zero;
            Damage = 5;
            ProjectileScale = new Vector2(1f, 1f);
            XPValue = 0;
            ProjectileDamage = 5;
            ItemDropChance = 0f;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 1;
            MoneyDropChance = 0.5f;
            StatLevelHPMod = 0.16f;
            StatLevelDMGMod = 0.091f;
            StatLevelXPMod = 0.025f;
            MinMoneyGainPerLevel = 0.23f;
            MaxMoneyGainPerLevel = 0.29f;
            base.ForceDraw = true;
        }

        protected override void InitializeEV() {}

        protected override void InitializeLogic() {
            LogicSet logicSet = new LogicSet(this);
            logicSet.AddAction(new PlayAnimationLogicAction(true), Types.Sequence.Serial);
            logicSet.AddAction(new MoveLogicAction(m_target, true, -1f), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
            LogicSet logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new PlayAnimationLogicAction(true), Types.Sequence.Serial);
            logicSet2.AddAction(new MoveLogicAction(m_target, false, -1f), Types.Sequence.Serial);
            logicSet2.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
            LogicSet logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new StopAnimationLogicAction(), Types.Sequence.Serial);
            logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
            logicSet3.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
            m_walkingLB.AddLogicSet(new LogicSet[] {
                logicSet,
                logicSet2,
                logicSet3
            });
        }

        public void SetDifficulty(GameTypes.EnemyDifficulty difficulty, bool reinitialize) {
            Difficulty = difficulty;
            if (reinitialize)
                Initialize();
        }

        public void Initialize() {
            if (TintablePart == null)
                TintablePart = base.GetChildAt(0);
            InitializeBaseEV();
            InitializeEV();
            HealthGainPerLevel = (int)(base.MaxHealth * StatLevelHPMod);
            DamageGainPerLevel = (int)(m_damage * StatLevelDMGMod);
            XPBonusPerLevel = (int)(m_xpValue * StatLevelXPMod);
            m_internalLockFlip = base.LockFlip;
            m_internalIsWeighted = base.IsWeighted;
            m_internalRotation = base.Rotation;
            m_internalAnimationDelay = base.AnimationDelay;
            m_internalScale = this.Scale;
            InternalFlip = Flip;
            foreach (LogicBlock current in logicBlocksToDispose)
                current.ClearAllLogicSets();
            if (m_levelScreen != null)
                InitializeLogic();
            m_initialDelayCounter = InitialLogicDelay;
            base.CurrentHealth = MaxHealth;
        }

        public void InitializeDebugRadii() {
            if (m_engageRadiusTexture == null) {
                int num = EngageRadius;
                int num2 = ProjectileRadius;
                int num3 = MeleeRadius;
                if (num > 1000)
                    num = 1000;
                if (num2 > 1000)
                    num2 = 1000;
                if (num3 > 1000)
                    num3 = 1000;
                m_engageRadiusTexture = DebugHelper.CreateCircleTexture(num, m_levelScreen.Camera.GraphicsDevice);
                m_projectileRadiusTexture = DebugHelper.CreateCircleTexture(num2, m_levelScreen.Camera.GraphicsDevice);
                m_meleeRadiusTexture = DebugHelper.CreateCircleTexture(num3, m_levelScreen.Camera.GraphicsDevice);
            }
        }

        public void SetPlayerTarget(PlayerObj target) {
            m_target = target;
        }

        public void SetLevelScreen(ProceduralLevelScreen levelScreen) {
            m_levelScreen = levelScreen;
        }

        public override void Update(GameTime gameTime) {
            float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (m_initialDelayCounter > 0f)
                m_initialDelayCounter -= num;
            else {
                if (m_invincibleCounter > 0f)
                    m_invincibleCounter -= num;
                if (m_invincibleCounterProjectile > 0f)
                    m_invincibleCounterProjectile -= num;
                if (m_invincibleCounter <= 0f && m_invincibleCounterProjectile <= 0f && !base.IsWeighted) {
                    if (base.AccelerationY < 0f)
                        base.AccelerationY += 15f;
                    else if (base.AccelerationY > 0f)
                        base.AccelerationY -= 15f;
                    if (base.AccelerationX < 0f)
                        base.AccelerationX += 15f;
                    else if (base.AccelerationX > 0f)
                        base.AccelerationX -= 15f;
                    if (base.AccelerationY < 3.6f && base.AccelerationY > -3.6f)
                        base.AccelerationY = 0f;
                    if (base.AccelerationX < 3.6f && base.AccelerationX > -3.6f)
                        base.AccelerationX = 0f;
                }
                if (!base.IsKilled && !IsPaused) {
                    DistanceToPlayer = CDGMath.DistanceBetweenPts(base.Position, m_target.Position);
                    if (DistanceToPlayer > EngageRadius)
                        base.State = 0;
                    else if (DistanceToPlayer < EngageRadius && DistanceToPlayer >= ProjectileRadius)
                        base.State = 1;
                    else if (DistanceToPlayer < ProjectileRadius && DistanceToPlayer >= MeleeRadius)
                        base.State = 2;
                    else
                        base.State = 3;
                    if (m_cooldownTimer > 0f && m_currentActiveLB == m_cooldownLB)
                        m_cooldownTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (m_cooldownTimer <= 0f && m_runCooldown)
                        m_runCooldown = false;
                    if (!base.LockFlip) {
                        if (!AlwaysFaceTarget) {
                            if (base.Heading.X < 0f)
                                Flip = SpriteEffects.FlipHorizontally;
                            else
                                Flip = SpriteEffects.None;
                        }
                        else if (base.X > m_target.X)
                            Flip = SpriteEffects.FlipHorizontally;
                        else
                            Flip = SpriteEffects.None;
                    }
                    if (!m_currentActiveLB.IsActive && !m_runCooldown) {
                        switch (Difficulty) {
                            case GameTypes.EnemyDifficulty.BASIC:
                                RunBasicLogic();
                                break;
                            case GameTypes.EnemyDifficulty.ADVANCED:
                                RunAdvancedLogic();
                                break;
                            case GameTypes.EnemyDifficulty.EXPERT:
                                RunExpertLogic();
                                break;
                            case GameTypes.EnemyDifficulty.MINIBOSS:
                                RunMinibossLogic();
                                break;
                        }
                        if (m_runCooldown && m_currentActiveLB.ActiveLS.Tag == 2)
                            m_cooldownTimer = CooldownTime;
                    }
                    if (!m_currentActiveLB.IsActive && m_runCooldown && m_cooldownTimer > 0f && !m_cooldownLB.IsActive) {
                        m_currentActiveLB = m_cooldownLB;
                        m_currentActiveLB.RunLogicBlock(m_cooldownParams);
                    }
                    if (base.IsWeighted && m_invincibleCounter <= 0f && m_invincibleCounterProjectile <= 0f) {
                        if (base.HeadingX > 0f)
                            base.HeadingX = 1f;
                        else if (base.HeadingX < 0f)
                            base.HeadingX = -1f;
                        base.X += base.HeadingX * (base.CurrentSpeed * num);
                    }
                    else if (m_isTouchingGround || !base.IsWeighted)
                        base.Position += base.Heading * (base.CurrentSpeed * num);
                    if (base.X < (float)m_levelScreen.CurrentRoom.Bounds.Left)
                        base.X = (float)m_levelScreen.CurrentRoom.Bounds.Left;
                    else if (base.X > (float)m_levelScreen.CurrentRoom.Bounds.Right)
                        base.X = (float)m_levelScreen.CurrentRoom.Bounds.Right;
                    if (base.Y < (float)m_levelScreen.CurrentRoom.Bounds.Top)
                        base.Y = (float)m_levelScreen.CurrentRoom.Bounds.Top;
                    else if (base.Y > (float)m_levelScreen.CurrentRoom.Bounds.Bottom)
                        base.Y = (float)m_levelScreen.CurrentRoom.Bounds.Bottom;
                    if (m_currentActiveLB == m_cooldownLB)
                        m_currentActiveLB.Update(gameTime);
                    else {
                        m_currentActiveLB.Update(gameTime);
                        m_cooldownLB.Update(gameTime);
                    }
                }
            }
            if (base.IsWeighted)
                CheckGroundCollision();
            if (base.CurrentHealth <= 0 && !base.IsKilled && !m_bossVersionKilled)
                Kill(true);
            base.Update(gameTime);
        }

        public void CheckGroundCollisionOld() {
            m_numTouchingGrounds = 0;
            float num = 2.14748365E+09f;
            int num2 = 10;
            bool flag = true;
            foreach (IPhysicsObj current in m_levelScreen.PhysicsManager.ObjectList) {
                if (current != this && current.CollidesTop && (current.CollisionTypeTag == 1 || current.CollisionTypeTag == 5 || current.CollisionTypeTag == 4 || current.CollisionTypeTag == 10) && Math.Abs(current.Bounds.Top - Bounds.Bottom) < num2) {
                    foreach (CollisionBox current2 in current.CollisionBoxes) {
                        if (current2.Type == 0) {
                            Rectangle a = GroundCollisionRect;
                            if (current2.AbsRotation != 0f)
                                a = RotatedGroundCollisionRect;
                            if (CollisionMath.RotatedRectIntersects(a, 0f, Vector2.Zero, current2.AbsRect, current2.AbsRotation, Vector2.Zero)) {
                                m_numTouchingGrounds++;
                                if (current2.AbsParent.Rotation == 0f)
                                    flag = false;
                                Vector2 vector = CollisionMath.RotatedRectIntersectsMTD(GroundCollisionRect, 0f, Vector2.Zero, current2.AbsRect, current2.AbsRotation, Vector2.Zero);
                                if (flag)
                                    flag = !CollisionMath.RotatedRectIntersects(Bounds, 0f, Vector2.Zero, current2.AbsRect, current2.AbsRotation, Vector2.Zero);
                                float y = vector.Y;
                                if (num > y)
                                    num = y;
                            }
                        }
                    }
                }
            }
            if (num <= 2f && base.AccelerationY >= 0f)
                m_isTouchingGround = true;
        }

        private void CheckGroundCollision() {
            m_isTouchingGround = false;
            m_numTouchingGrounds = 0;
            if (base.AccelerationY >= 0f) {
                IPhysicsObj physicsObj = null;
                float num = 3.40282347E+38f;
                IPhysicsObj physicsObj2 = null;
                float num2 = 3.40282347E+38f;
                Rectangle terrainBounds = this.TerrainBounds;
                terrainBounds.Height += 10;
                foreach (TerrainObj current in m_levelScreen.CurrentRoom.TerrainObjList) {
                    if (current.Visible && current.IsCollidable && current.CollidesTop && current.HasTerrainHitBox && (current.CollisionTypeTag == 1 || current.CollisionTypeTag == 10 || current.CollisionTypeTag == 6 || current.CollisionTypeTag == 4)) {
                        if (current.Rotation == 0f) {
                            Rectangle left = terrainBounds;
                            left.X -= 30;
                            left.Width += 60;
                            Vector2 value = CollisionMath.CalculateMTD(left, current.Bounds);
                            if (value != Vector2.Zero)
                                m_numTouchingGrounds++;
                            if (CollisionMath.CalculateMTD(terrainBounds, current.Bounds).Y < 0f) {
                                int num3 = current.Bounds.Top - Bounds.Bottom;
                                if (num3 < num) {
                                    physicsObj = current;
                                    num = num3;
                                }
                            }
                        }
                        else {
                            Vector2 value2 = CollisionMath.RotatedRectIntersectsMTD(terrainBounds, base.Rotation, Vector2.Zero, current.TerrainBounds, current.Rotation, Vector2.Zero);
                            if (value2 != Vector2.Zero)
                                m_numTouchingGrounds++;
                            if (value2.Y < 0f) {
                                float y = value2.Y;
                                if (y < num2 && value2.Y < 0f) {
                                    physicsObj2 = current;
                                    num2 = y;
                                }
                            }
                            Rectangle terrainBounds2 = this.TerrainBounds;
                            terrainBounds2.Height += 50;
                            int num4 = 15;
                            Vector2 pt = CollisionMath.RotatedRectIntersectsMTD(terrainBounds2, base.Rotation, Vector2.Zero, current.TerrainBounds, current.Rotation, Vector2.Zero);
                            if (pt.Y < 0f) {
                                float num5 = CDGMath.DistanceBetweenPts(pt, Vector2.Zero);
                                float num6 = (float)(50.0 - Math.Sqrt((num5 * num5 * 2f)));
                                if (num6 > 0f && num6 < num4)
                                    base.Y += num6;
                                float y2 = value2.Y;
                                if (y2 < num2) {
                                    physicsObj2 = current;
                                    num2 = y2;
                                }
                            }
                        }
                    }
                    if (physicsObj != null)
                        m_isTouchingGround = true;
                    if (physicsObj2 != null)
                        m_isTouchingGround = true;
                }
            }
        }

        private void HookToSlope(IPhysicsObj collisionObj) {
            base.UpdateCollisionBoxes();
            Rectangle terrainBounds = this.TerrainBounds;
            terrainBounds.Height += 100;
            float num = base.X;
            if (CollisionMath.RotatedRectIntersectsMTD(terrainBounds, base.Rotation, Vector2.Zero, collisionObj.TerrainBounds, collisionObj.Rotation, Vector2.Zero).Y < 0f) {
                bool flag = false;
                Vector2 vector;
                Vector2 vector2;
                if (collisionObj.Width > collisionObj.Height) {
                    vector = CollisionMath.UpperLeftCorner(collisionObj.TerrainBounds, collisionObj.Rotation, Vector2.Zero);
                    vector2 = CollisionMath.UpperRightCorner(collisionObj.TerrainBounds, collisionObj.Rotation, Vector2.Zero);
                    if (collisionObj.Rotation > 0f)
                        num = (float)this.TerrainBounds.Left;
                    else
                        num = (float)this.TerrainBounds.Right;
                    if (num > vector.X && num < vector2.X)
                        flag = true;
                }
                else if (collisionObj.Rotation > 0f) {
                    vector = CollisionMath.LowerLeftCorner(collisionObj.TerrainBounds, collisionObj.Rotation, Vector2.Zero);
                    vector2 = CollisionMath.UpperLeftCorner(collisionObj.TerrainBounds, collisionObj.Rotation, Vector2.Zero);
                    num = (float)this.TerrainBounds.Right;
                    if (num > vector.X && num < vector2.X)
                        flag = true;
                }
                else {
                    vector = CollisionMath.UpperRightCorner(collisionObj.TerrainBounds, collisionObj.Rotation, Vector2.Zero);
                    vector2 = CollisionMath.LowerRightCorner(collisionObj.TerrainBounds, collisionObj.Rotation, Vector2.Zero);
                    num = (float)this.TerrainBounds.Left;
                    if (num > vector.X && num < vector2.X)
                        flag = true;
                }
                if (flag) {
                    float num2 = vector2.X - vector.X;
                    float num3 = vector2.Y - vector.Y;
                    float x = vector.X;
                    float y = vector.Y;
                    float num4 = y + (num - x) * (num3 / num2);
                    num4 -= (float)this.TerrainBounds.Bottom - base.Y - 2f;
                    base.Y = (float)((int)num4);
                }
            }
        }

        protected void SetCooldownLogicBlock(LogicBlock cooldownLB, params int[] percentage) {
            m_cooldownLB = cooldownLB;
            m_cooldownParams = percentage;
        }

        protected void RunLogicBlock(bool runCDLogicAfterward, LogicBlock block, params int[] percentage) {
            m_runCooldown = runCDLogicAfterward;
            m_currentActiveLB = block;
            m_currentActiveLB.RunLogicBlock(percentage);
        }

        protected virtual void RunBasicLogic() {}

        protected virtual void RunAdvancedLogic() {
            RunBasicLogic();
        }

        protected virtual void RunExpertLogic() {
            RunBasicLogic();
        }

        protected virtual void RunMinibossLogic() {
            RunBasicLogic();
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType) {
            IPhysicsObj physicsObj = otherBox.AbsParent as IPhysicsObj;
            Vector2 vector = CollisionMath.CalculateMTD(thisBox.AbsRect, otherBox.AbsRect);
            if (collisionResponseType == 2 && (physicsObj.CollisionTypeTag == 2 || physicsObj.CollisionTypeTag == 10 || (physicsObj.CollisionTypeTag == 10 && base.IsWeighted)) && ((!(otherBox.AbsParent is ProjectileObj) && m_invincibleCounter <= 0f) || (otherBox.AbsParent is ProjectileObj && (m_invincibleCounterProjectile <= 0f || (otherBox.AbsParent as ProjectileObj).IgnoreInvincibleCounter)))) {
                if (IsDemented) {
                    m_invincibleCounter = InvincibilityTime;
                    m_invincibleCounterProjectile = InvincibilityTime;
                    m_levelScreen.ImpactEffectPool.DisplayQuestionMark(new Vector2(base.X, Bounds.Top));
                    return;
                }
                int num = (physicsObj as IDealsDamageObj).Damage;
                bool isPlayer = false;
                if (physicsObj == m_target) {
                    if (CDGMath.RandomFloat(0f, 1f) <= m_target.TotalCritChance && !NonKillable && physicsObj == m_target) {
                        m_levelScreen.ImpactEffectPool.DisplayCriticalText(new Vector2(base.X, Bounds.Top));
                        num = (int)(num * m_target.TotalCriticalDamage);
                    }
                    isPlayer = true;
                }
                ProjectileObj projectileObj = otherBox.AbsParent as ProjectileObj;
                if (projectileObj != null) {
                    m_invincibleCounterProjectile = InvincibilityTime;
                    if (projectileObj.DestroysWithEnemy && !NonKillable)
                        projectileObj.RunDestroyAnimation(false);
                }
                Point center = Rectangle.Intersect(thisBox.AbsRect, otherBox.AbsRect).Center;
                if (thisBox.AbsRotation != 0f || otherBox.AbsRotation != 0f)
                    center = Rectangle.Intersect(thisBox.AbsParent.Bounds, otherBox.AbsParent.Bounds).Center;
                Vector2 collisionPt = new Vector2(center.X, center.Y);
                if (projectileObj == null || (projectileObj != null && projectileObj.Spell != 20)) {
                    if (projectileObj != null || physicsObj.CollisionTypeTag != 10 || (physicsObj.CollisionTypeTag == 10 && base.IsWeighted))
                        HitEnemy(num, collisionPt, isPlayer);
                }
                else if (projectileObj != null && projectileObj.Spell == 20 && base.CanBeKnockedBack && !IsPaused) {
                    base.CurrentSpeed = 0f;
                    float num2 = 3f;
                    if (base.KnockBack == Vector2.Zero) {
                        if (base.X < m_target.X)
                            base.AccelerationX = -m_target.EnemyKnockBack.X * num2;
                        else
                            base.AccelerationX = m_target.EnemyKnockBack.X * num2;
                        base.AccelerationY = -m_target.EnemyKnockBack.Y * num2;
                    }
                    else {
                        if (base.X < m_target.X)
                            base.AccelerationX = -base.KnockBack.X * num2;
                        else
                            base.AccelerationX = base.KnockBack.X * num2;
                        base.AccelerationY = -base.KnockBack.Y * num2;
                    }
                }
                if (physicsObj == m_target)
                    m_invincibleCounter = InvincibilityTime;
            }
            if (collisionResponseType == 1 && (physicsObj.CollisionTypeTag == 1 || physicsObj.CollisionTypeTag == 6 || physicsObj.CollisionTypeTag == 10) && base.CollisionTypeTag != 4) {
                if (base.CurrentSpeed != 0f && vector.X != 0f && Math.Abs(vector.X) > 10f && ((vector.X > 0f && physicsObj.CollidesRight) || (vector.X < 0f && physicsObj.CollidesLeft)))
                    base.CurrentSpeed = 0f;
                if (m_numTouchingGrounds <= 1 && base.CurrentSpeed != 0f && vector.Y < 0f && !CanFallOffLedges) {
                    if (Bounds.Left < physicsObj.Bounds.Left && base.HeadingX < 0f) {
                        base.X = (float)physicsObj.Bounds.Left + (base.AbsX - (float)Bounds.Left);
                        base.CurrentSpeed = 0f;
                    }
                    else if (Bounds.Right > physicsObj.Bounds.Right && base.HeadingX > 0f) {
                        base.X = (float)physicsObj.Bounds.Right - ((float)Bounds.Right - base.AbsX);
                        base.CurrentSpeed = 0f;
                    }
                    m_isTouchingGround = true;
                }
                if (base.AccelerationX != 0f && m_isTouchingGround)
                    base.AccelerationX = 0f;
                bool flag = false;
                if (Math.Abs(vector.X) < 10f && vector.X != 0f && Math.Abs(vector.Y) < 10f && vector.Y != 0f)
                    flag = true;
                if (m_isTouchingGround && !physicsObj.CollidesBottom && physicsObj.CollidesTop && physicsObj.TerrainBounds.Top < this.TerrainBounds.Bottom - 30)
                    flag = true;
                if (!physicsObj.CollidesRight && !physicsObj.CollidesLeft && physicsObj.CollidesTop && physicsObj.CollidesBottom)
                    flag = true;
                Vector2 vector2 = CollisionMath.RotatedRectIntersectsMTD(thisBox.AbsRect, thisBox.AbsRotation, Vector2.Zero, otherBox.AbsRect, otherBox.AbsRotation, Vector2.Zero);
                if (!flag)
                    base.CollisionResponse(thisBox, otherBox, collisionResponseType);
                if (vector2.Y < 0f && otherBox.AbsRotation != 0f && base.IsWeighted)
                    base.X -= vector2.X;
            }
        }

        public virtual void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer) {
            if (m_target != null && m_target.CurrentHealth > 0) {
                SoundManager.Play3DSound(this, Game.ScreenManager.Player, new[] {
                    "EnemyHit1",
                    "EnemyHit2",
                    "EnemyHit3",
                    "EnemyHit4",
                    "EnemyHit5",
                    "EnemyHit6"
                });
                base.Blink(Color.Red, 0.1f);
                m_levelScreen.ImpactEffectPool.DisplayEnemyImpactEffect(collisionPt);
                if (isPlayer && (Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14)) {
                    base.CurrentHealth -= damage;
                    m_target.CurrentMana += ((int)(damage * 0.3f));
                    m_levelScreen.TextManager.DisplayNumberText(damage, Color.Red, new Vector2(base.X, Bounds.Top));
                    m_levelScreen.TextManager.DisplayNumberStringText((int)(damage * 0.3f), "mp", Color.RoyalBlue, new Vector2(m_target.X, (m_target.Bounds.Top - 30)));
                }
                else {
                    base.CurrentHealth -= damage;
                    m_levelScreen.TextManager.DisplayNumberText(damage, Color.Red, new Vector2(base.X, Bounds.Top));
                }
                if (isPlayer) {
                    PlayerObj expr_198 = m_target;
                    expr_198.NumSequentialAttacks += 1;
                    if (m_target.IsAirAttacking) {
                        m_target.IsAirAttacking = false;
                        m_target.AccelerationY = -m_target.AirAttackKnockBack;
                        m_target.NumAirBounces++;
                    }
                }
                if (base.CanBeKnockedBack && !IsPaused && Game.PlayerStats.Traits.X != 17f && Game.PlayerStats.Traits.Y != 17f) {
                    base.CurrentSpeed = 0f;
                    float num = 1f;
                    if (Game.PlayerStats.Traits.X == 16f || Game.PlayerStats.Traits.Y == 16f)
                        num = 2f;
                    if (base.KnockBack == Vector2.Zero) {
                        if (base.X < m_target.X)
                            base.AccelerationX = -m_target.EnemyKnockBack.X * num;
                        else
                            base.AccelerationX = m_target.EnemyKnockBack.X * num;
                        base.AccelerationY = -m_target.EnemyKnockBack.Y * num;
                    }
                    else {
                        if (base.X < m_target.X)
                            base.AccelerationX = -base.KnockBack.X * num;
                        else
                            base.AccelerationX = base.KnockBack.X * num;
                        base.AccelerationY = -base.KnockBack.Y * num;
                    }
                }
                m_levelScreen.SetLastEnemyHit(this);
            }
        }

        public void KillSilently() {
            base.Kill(false);
        }

        public override void Kill(bool giveXP = true) {
            int totalVampBonus = m_target.TotalVampBonus;
            if (totalVampBonus > 0) {
                m_target.CurrentHealth += totalVampBonus;
                m_levelScreen.TextManager.DisplayNumberStringText(totalVampBonus, "hp", Color.LightGreen, new Vector2(m_target.X, (m_target.Bounds.Top - 60)));
            }
            if (m_target.ManaGain > 0f) {
                m_target.CurrentMana += m_target.ManaGain;
                m_levelScreen.TextManager.DisplayNumberStringText((int)m_target.ManaGain, "mp", Color.RoyalBlue, new Vector2(m_target.X, (m_target.Bounds.Top - 90)));
            }
            if (Game.PlayerStats.SpecialItem == 5) {
                m_levelScreen.ItemDropManager.DropItem(base.Position, 1, 10f);
                m_levelScreen.ItemDropManager.DropItem(base.Position, 1, 10f);
            }
            m_levelScreen.KillEnemy(this);
            SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Enemy_Death");
            if (DropsItem) {
                if (Type == 26)
                    m_levelScreen.ItemDropManager.DropItem(base.Position, 2, 0.1f);
                else if (CDGMath.RandomInt(1, 100) <= 2) {
                    if (CDGMath.RandomPlusMinus() < 0)
                        m_levelScreen.ItemDropManager.DropItem(base.Position, 2, 0.1f);
                    else
                        m_levelScreen.ItemDropManager.DropItem(base.Position, 3, 0.1f);
                }
                if (CDGMath.RandomFloat(0f, 1f) <= MoneyDropChance) {
                    int num = CDGMath.RandomInt(MinMoneyDropAmount, MaxMoneyDropAmount) * 10 + (int)(CDGMath.RandomFloat(MinMoneyGainPerLevel, MaxMoneyGainPerLevel) * (float)Level * 10f);
                    int num2 = num / 500;
                    num -= num2 * 500;
                    int num3 = num / 100;
                    num -= num3 * 100;
                    int num4 = num / 10;
                    for (int i = 0; i < num2; i++)
                        m_levelScreen.ItemDropManager.DropItem(base.Position, 11, 500f);
                    for (int j = 0; j < num3; j++)
                        m_levelScreen.ItemDropManager.DropItem(base.Position, 10, 100f);
                    for (int k = 0; k < num4; k++)
                        m_levelScreen.ItemDropManager.DropItem(base.Position, 1, 10f);
                }
            }
            if (m_currentActiveLB.IsActive)
                m_currentActiveLB.StopLogicBlock();
            m_levelScreen.ImpactEffectPool.DisplayDeathEffect(base.Position);
            if ((Game.PlayerStats.Class == 7 || Game.PlayerStats.Class == 15) && GivesLichHealth) {
                int num5 = 0;
                int currentLevel = Game.PlayerStats.CurrentLevel;
                int num6 = (int)(Level * 2.75f);
                if (currentLevel < num6)
                    num5 = 4;
                else if (currentLevel >= num6)
                    num5 = 4;
                int num7 = (int)Math.Round((((m_target.BaseHealth + m_target.GetEquipmentHealth() + Game.PlayerStats.BonusHealth * 5) + SkillSystem.GetSkill(SkillType.Health_Up).ModifierAmount + SkillSystem.GetSkill(SkillType.Health_Up_Final).ModifierAmount) * 1f), MidpointRounding.AwayFromZero);
                if (m_target.MaxHealth + num5 < num7) {
                    Game.PlayerStats.LichHealth += num5;
                    m_target.CurrentHealth += num5;
                    m_levelScreen.TextManager.DisplayNumberStringText(num5, "max hp", Color.LightGreen, new Vector2(m_target.X, (m_target.Bounds.Top - 30)));
                }
            }
            Game.PlayerStats.NumEnemiesBeaten++;
            if (m_saveToEnemiesKilledList) {
                Vector4 value = Game.PlayerStats.EnemiesKilledList[Type];
                switch (Difficulty) {
                    case GameTypes.EnemyDifficulty.BASIC:
                        value.X += 1f;
                        break;
                    case GameTypes.EnemyDifficulty.ADVANCED:
                        value.Y += 1f;
                        break;
                    case GameTypes.EnemyDifficulty.EXPERT:
                        value.Z += 1f;
                        break;
                    case GameTypes.EnemyDifficulty.MINIBOSS:
                        value.W += 1f;
                        break;
                }
                Game.PlayerStats.EnemiesKilledList[Type] = value;
            }
            if (giveXP && Type == 26)
                GameUtil.UnlockAchievement("FEAR_OF_CHICKENS");
            base.Kill(true);
        }

        public void PauseEnemy(bool forcePause = false) {
            if ((!IsPaused && !base.IsKilled && !m_bossVersionKilled) || forcePause) {
                m_isPaused = true;
                base.DisableAllWeight = true;
                base.PauseAnimation();
            }
        }

        public void UnpauseEnemy(bool forceUnpause = false) {
            if ((IsPaused && !base.IsKilled && !m_bossVersionKilled) || forceUnpause) {
                m_isPaused = false;
                base.DisableAllWeight = false;
                base.ResumeAnimation();
            }
        }

        public void DrawDetectionRadii(Camera2D camera) {
            camera.Draw(m_engageRadiusTexture, new Vector2(base.Position.X - (float)EngageRadius, base.Position.Y - (float)EngageRadius), Color.Red * 0.5f);
            camera.Draw(m_projectileRadiusTexture, new Vector2(base.Position.X - (float)ProjectileRadius, base.Position.Y - (float)ProjectileRadius), Color.Blue * 0.5f);
            camera.Draw(m_meleeRadiusTexture, new Vector2(base.Position.X - (float)MeleeRadius, base.Position.Y - (float)MeleeRadius), Color.Green * 0.5f);
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                if (m_currentActiveLB.IsActive)
                    m_currentActiveLB.StopLogicBlock();
                m_currentActiveLB = null;
                foreach (LogicBlock current in logicBlocksToDispose)
                    current.Dispose();
                for (int i = 0; i < logicBlocksToDispose.Count; i++)
                    logicBlocksToDispose[i] = null;
                logicBlocksToDispose.Clear();
                logicBlocksToDispose = null;
                m_target = null;
                m_walkingLB.Dispose();
                m_walkingLB = null;
                if (m_cooldownLB.IsActive)
                    m_cooldownLB.StopLogicBlock();
                m_cooldownLB.Dispose();
                m_cooldownLB = null;
                if (m_engageRadiusTexture != null)
                    m_engageRadiusTexture.Dispose();
                m_engageRadiusTexture = null;
                if (m_engageRadiusTexture != null)
                    m_projectileRadiusTexture.Dispose();
                m_projectileRadiusTexture = null;
                if (m_engageRadiusTexture != null)
                    m_meleeRadiusTexture.Dispose();
                m_meleeRadiusTexture = null;
                if (m_cooldownParams != null)
                    Array.Clear(m_cooldownParams, 0, m_cooldownParams.Length);
                m_cooldownParams = null;
                TintablePart = null;
                m_flipTween = null;
                base.Dispose();
            }
        }

        public override void Reset() {
            if (m_currentActiveLB.IsActive)
                m_currentActiveLB.StopLogicBlock();
            if (m_cooldownLB.IsActive)
                m_cooldownLB.StopLogicBlock();
            m_invincibleCounter = 0f;
            m_invincibleCounterProjectile = 0f;
            base.State = 0;
            this.ChangeSprite(m_resetSpriteName);
            if (PlayAnimationOnRestart)
                base.PlayAnimation(true);
            m_initialDelayCounter = InitialLogicDelay;
            UnpauseEnemy(true);
            m_bossVersionKilled = false;
            m_blinkTimer = 0f;
            base.Reset();
        }

        public virtual void ResetState() {
            if (m_currentActiveLB.IsActive)
                m_currentActiveLB.StopLogicBlock();
            if (m_cooldownLB.IsActive)
                m_cooldownLB.StopLogicBlock();
            m_invincibleCounter = 0f;
            m_invincibleCounterProjectile = 0f;
            base.State = 0;
            if (Type != 32)
                this.ChangeSprite(m_resetSpriteName);
            if (PlayAnimationOnRestart)
                base.PlayAnimation(true);
            m_initialDelayCounter = InitialLogicDelay;
            base.LockFlip = m_internalLockFlip;
            Flip = InternalFlip;
            base.AnimationDelay = m_internalAnimationDelay;
            UnpauseEnemy(true);
            base.CurrentHealth = MaxHealth;
            m_blinkTimer = 0f;
        }

        protected float ParseTagToFloat(string key) {
            if (this.Tag != "") {
                int num = this.Tag.IndexOf(key + ":") + key.Length + 1;
                int num2 = this.Tag.IndexOf(",", num);
                if (num2 == -1)
                    num2 = this.Tag.Length;
                try {
                    CultureInfo cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                    cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
                    float result = float.Parse(this.Tag.Substring(num, num2 - num), NumberStyles.Any, cultureInfo);
                    return result;
                } catch (Exception ex) {
                    Console.WriteLine(string.Concat(new[] {
                        "Could not parse key:",
                        key,
                        " with string:",
                        this.Tag,
                        ".  Original Error: ",
                        ex.Message
                    }));
                    float result = 0f;
                    return result;
                }
            }
            return 0f;
        }

        protected string ParseTagToString(string key) {
            int num = this.Tag.IndexOf(key + ":") + key.Length + 1;
            int num2 = this.Tag.IndexOf(",", num);
            if (num2 == -1)
                num2 = this.Tag.Length;
            return this.Tag.Substring(num, num2 - num);
        }

        protected override GameObj CreateCloneInstance() {
            return EnemyBuilder.BuildEnemy((int)Type, m_target, null, m_levelScreen, Difficulty, false);
        }

        protected override void FillCloneInstance(object obj) {
            base.FillCloneInstance(obj);
            EnemyObj enemyObj = obj as EnemyObj;
            enemyObj.IsProcedural = IsProcedural;
            enemyObj.InitialLogicDelay = InitialLogicDelay;
            enemyObj.NonKillable = NonKillable;
            enemyObj.GivesLichHealth = GivesLichHealth;
            enemyObj.DropsItem = DropsItem;
            enemyObj.IsDemented = IsDemented;
        }
    }
}
