using System;
using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class EnemyObj_BouncySpike : EnemyObj {
        private float RotationSpeed = 250f;
        public RoomObj SpawnRoom;
        private float m_internalOrientation;
        private int m_selfDestructCounter;
        private float m_selfDestructTimer = 0.7f;
        private int m_selfDestructTotalBounces = 12;

        public EnemyObj_BouncySpike(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyBouncySpike_Character", target, physicsManager, levelToAttachTo, difficulty) {
            Type = 3;
            base.NonKillable = true;
        }

        public Vector2 SavedStartingPos { get; set; }

        protected override void InitializeEV() {
            int num = CDGMath.RandomInt(0, 11);
            if (num >= 9)
                base.Orientation = 0f;
            else if (num >= 6)
                base.Orientation = 180f;
            else if (num >= 4)
                base.Orientation = 90f;
            else if (num >= 1)
                base.Orientation = 270f;
            else
                base.Orientation = 45f;
            m_internalOrientation = base.Orientation;
            base.HeadingX = (float)Math.Cos(MathHelper.ToRadians(base.Orientation));
            base.HeadingY = (float)Math.Sin(MathHelper.ToRadians(base.Orientation));
            base.Name = "Spiketor";
            MaxHealth = 5;
            base.Damage = 27;
            base.XPValue = 25;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 1;
            MoneyDropChance = 0.4f;
            base.Speed = 300f;
            this.TurnSpeed = 10f;
            ProjectileSpeed = 650f;
            base.JumpHeight = 300f;
            CooldownTime = 2f;
            base.AnimationDelay = 0.1f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = false;
            base.CanBeKnockedBack = false;
            base.IsWeighted = false;
            this.Scale = EnemyEV.BouncySpike_Basic_Scale;
            base.ProjectileScale = EnemyEV.BouncySpike_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.BouncySpike_Basic_Tint;
            MeleeRadius = 50;
            ProjectileRadius = 100;
            EngageRadius = 150;
            ProjectileDamage = base.Damage;
            base.KnockBack = new Vector2(1f, 2f);
            base.LockFlip = true;
            switch (base.Difficulty) {
                case GameTypes.EnemyDifficulty.BASIC:
                    break;
                case GameTypes.EnemyDifficulty.ADVANCED:
                    base.Name = "Spiketex";
                    MaxHealth = 5;
                    base.Damage = 32;
                    base.XPValue = 25;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 1;
                    MoneyDropChance = 0.5f;
                    base.Speed = 300f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 650f;
                    base.JumpHeight = 300f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = false;
                    base.IsWeighted = false;
                    this.Scale = EnemyEV.BouncySpike_Advanced_Scale;
                    base.ProjectileScale = EnemyEV.BouncySpike_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.BouncySpike_Advanced_Tint;
                    MeleeRadius = 50;
                    EngageRadius = 150;
                    ProjectileRadius = 100;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = new Vector2(1f, 2f);
                    break;
                case GameTypes.EnemyDifficulty.EXPERT:
                    base.Name = "Spiketus";
                    MaxHealth = 5;
                    base.Damage = 38;
                    base.XPValue = 25;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 1;
                    MoneyDropChance = 1f;
                    base.Speed = 300f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 650f;
                    base.JumpHeight = 300f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = false;
                    base.IsWeighted = false;
                    this.Scale = EnemyEV.BouncySpike_Expert_Scale;
                    base.ProjectileScale = EnemyEV.BouncySpike_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.BouncySpike_Expert_Tint;
                    MeleeRadius = 50;
                    ProjectileRadius = 100;
                    EngageRadius = 150;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = new Vector2(1f, 2f);
                    return;
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    base.Name = "Spiker";
                    MaxHealth = 5;
                    base.Damage = 40;
                    base.XPValue = 25;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 1;
                    MoneyDropChance = 1f;
                    base.Speed = 300f;
                    this.TurnSpeed = 10f;
                    ProjectileSpeed = 650f;
                    base.JumpHeight = 300f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = false;
                    base.IsWeighted = false;
                    this.Scale = EnemyEV.BouncySpike_Miniboss_Scale;
                    base.ProjectileScale = EnemyEV.BouncySpike_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.BouncySpike_Miniboss_Tint;
                    MeleeRadius = 50;
                    ProjectileRadius = 100;
                    EngageRadius = 150;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = new Vector2(1f, 2f);
                    return;
                default:
                    return;
            }
        }

        protected override void RunBasicLogic() {
            switch (base.State) {
                case 0:
                case 1:
                case 2:
                case 3:
                    return;
            }
        }

        protected override void RunAdvancedLogic() {
            switch (base.State) {
                case 0:
                case 1:
                case 2:
                case 3:
                    return;
            }
        }

        protected override void RunExpertLogic() {
            switch (base.State) {
                case 0:
                case 1:
                case 2:
                case 3:
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
            if (!base.IsPaused) {
                float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Vector2 value = Vector2.Zero;
                Rectangle bounds = m_levelScreen.CurrentRoom.Bounds;
                if (base.Y < (float)(bounds.Top + 10))
                    value = CollisionMath.CalculateMTD(Bounds, new Rectangle(bounds.Left, bounds.Top, bounds.Width, 10));
                else if (base.Y > (float)(bounds.Bottom - 10))
                    value = CollisionMath.CalculateMTD(Bounds, new Rectangle(bounds.Left, bounds.Bottom - 10, bounds.Width, 10));
                if (base.X > (float)(bounds.Right - 10))
                    value = CollisionMath.CalculateMTD(Bounds, new Rectangle(bounds.Right - 10, bounds.Top, 10, bounds.Height));
                else if (base.X < (float)(bounds.Left + 10))
                    value = CollisionMath.CalculateMTD(Bounds, new Rectangle(bounds.Left, bounds.Top, 10, bounds.Height));
                if (value != Vector2.Zero) {
                    Vector2 heading = base.Heading;
                    Vector2 vector = new Vector2(value.Y, value.X * -1f);
                    Vector2 heading2 = 2f * (CDGMath.DotProduct(heading, vector) / CDGMath.DotProduct(vector, vector)) * vector - heading;
                    base.Heading = heading2;
                    SoundManager.Play3DSound(this, Game.ScreenManager.Player, new[] {
                        "GiantSpike_Bounce_01",
                        "GiantSpike_Bounce_02",
                        "GiantSpike_Bounce_03"
                    });
                    m_selfDestructCounter++;
                    m_selfDestructTimer = 1f;
                }
                if (m_selfDestructTimer > 0f) {
                    m_selfDestructTimer -= num;
                    if (m_selfDestructTimer <= 0f)
                        m_selfDestructCounter = 0;
                }
                if (m_selfDestructCounter >= m_selfDestructTotalBounces)
                    Kill(false);
                if (base.CurrentSpeed == 0f)
                    base.CurrentSpeed = base.Speed;
                if (base.HeadingX > 0f)
                    base.Rotation += RotationSpeed * num;
                else
                    base.Rotation -= RotationSpeed * num;
            }
            base.Update(gameTime);
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType) {
            TerrainObj terrainObj = otherBox.Parent as TerrainObj;
            if (terrainObj != null && !(terrainObj is DoorObj) && terrainObj.CollidesBottom && terrainObj.CollidesLeft && terrainObj.CollidesRight && terrainObj.CollidesTop) {
                Vector2 value = CollisionMath.RotatedRectIntersectsMTD(thisBox.AbsRect, (float)((int)thisBox.AbsRotation), Vector2.Zero, otherBox.AbsRect, (float)((int)otherBox.AbsRotation), Vector2.Zero);
                if (value != Vector2.Zero) {
                    Vector2 heading = base.Heading;
                    Vector2 vector = new Vector2(value.Y, value.X * -1f);
                    Vector2 heading2 = 2f * (CDGMath.DotProduct(heading, vector) / CDGMath.DotProduct(vector, vector)) * vector - heading;
                    base.X += value.X;
                    base.Y += value.Y;
                    base.Heading = heading2;
                    SoundManager.Play3DSound(this, Game.ScreenManager.Player, new[] {
                        "GiantSpike_Bounce_01",
                        "GiantSpike_Bounce_02",
                        "GiantSpike_Bounce_03"
                    });
                    m_selfDestructCounter++;
                    m_selfDestructTimer = 1f;
                }
            }
        }

        public override void Reset() {
            if (SpawnRoom != null) {
                m_levelScreen.RemoveEnemyFromRoom(this, SpawnRoom, SavedStartingPos);
                Dispose();
                return;
            }
            base.Orientation = m_internalOrientation;
            base.Reset();
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                SpawnRoom = null;
                base.Dispose();
            }
        }
    }
}
