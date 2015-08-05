using System;
using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class EnemyObj_Spark : EnemyObj {
        private byte m_collisionBoxSize = 10;
        private bool m_hookedToGround;

        public EnemyObj_Spark(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemySpark_Character", target, physicsManager, levelToAttachTo, difficulty) {
            base.IsWeighted = false;
            base.ForceDraw = true;
            Type = 24;
            base.NonKillable = true;
        }

        private Rectangle TopRect {
            get { return new Rectangle(Bounds.Left + m_collisionBoxSize, Bounds.Top, this.Width - (m_collisionBoxSize * 2), m_collisionBoxSize); }
        }

        private Rectangle BottomRect {
            get { return new Rectangle(Bounds.Left + m_collisionBoxSize, Bounds.Bottom - m_collisionBoxSize, this.Width - (m_collisionBoxSize * 2), m_collisionBoxSize); }
        }

        private Rectangle LeftRect {
            get { return new Rectangle(Bounds.Left, Bounds.Top + m_collisionBoxSize, m_collisionBoxSize, this.Height - (m_collisionBoxSize * 2)); }
        }

        private Rectangle RightRect {
            get { return new Rectangle(Bounds.Right - m_collisionBoxSize, Bounds.Top + m_collisionBoxSize, m_collisionBoxSize, this.Height - (m_collisionBoxSize * 2)); }
        }

        private Rectangle TopLeftPoint {
            get { return new Rectangle(Bounds.Left, Bounds.Top, m_collisionBoxSize, m_collisionBoxSize); }
        }

        private Rectangle TopRightPoint {
            get { return new Rectangle(Bounds.Right - m_collisionBoxSize, Bounds.Top, m_collisionBoxSize, m_collisionBoxSize); }
        }

        private Rectangle BottomLeftPoint {
            get { return new Rectangle(Bounds.Left, Bounds.Bottom - m_collisionBoxSize, m_collisionBoxSize, m_collisionBoxSize); }
        }

        private Rectangle BottomRightPoint {
            get { return new Rectangle(Bounds.Right - m_collisionBoxSize, Bounds.Bottom - m_collisionBoxSize, m_collisionBoxSize, m_collisionBoxSize); }
        }

        protected override void InitializeEV() {
            base.LockFlip = true;
            base.IsWeighted = false;
            base.Name = "Sparky";
            MaxHealth = 20;
            base.Damage = 20;
            base.XPValue = 100;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 2;
            MoneyDropChance = 0.4f;
            base.Speed = 200f;
            this.TurnSpeed = 0.0275f;
            ProjectileSpeed = 525f;
            base.JumpHeight = 500f;
            CooldownTime = 2f;
            base.AnimationDelay = 0.1f;
            AlwaysFaceTarget = false;
            CanFallOffLedges = false;
            base.CanBeKnockedBack = false;
            base.IsWeighted = false;
            this.Scale = EnemyEV.Spark_Basic_Scale;
            base.ProjectileScale = EnemyEV.Spark_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Spark_Basic_Tint;
            MeleeRadius = 10;
            ProjectileRadius = 20;
            EngageRadius = 30;
            ProjectileDamage = base.Damage;
            base.KnockBack = EnemyEV.Spark_Basic_KnockBack;
            switch (base.Difficulty) {
                case GameTypes.EnemyDifficulty.BASIC:
                    break;
                case GameTypes.EnemyDifficulty.ADVANCED:
                    base.Name = "Mr. Spark";
                    MaxHealth = 20;
                    base.Damage = 28;
                    base.XPValue = 175;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    base.Speed = 300f;
                    this.TurnSpeed = 0.03f;
                    ProjectileSpeed = 525f;
                    base.JumpHeight = 500f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = false;
                    base.IsWeighted = false;
                    this.Scale = EnemyEV.Spark_Advanced_Scale;
                    base.ProjectileScale = EnemyEV.Spark_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Spark_Advanced_Tint;
                    MeleeRadius = 10;
                    EngageRadius = 30;
                    ProjectileRadius = 20;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Spark_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.EXPERT:
                    base.Name = "Grandpa Spark";
                    MaxHealth = 20;
                    base.Damage = 32;
                    base.XPValue = 350;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 3;
                    MoneyDropChance = 1f;
                    base.Speed = 375f;
                    this.TurnSpeed = 0.03f;
                    ProjectileSpeed = 525f;
                    base.JumpHeight = 500f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = false;
                    base.IsWeighted = false;
                    this.Scale = EnemyEV.Spark_Expert_Scale;
                    base.ProjectileScale = EnemyEV.Spark_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Spark_Expert_Tint;
                    MeleeRadius = 10;
                    ProjectileRadius = 20;
                    EngageRadius = 30;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Spark_Expert_KnockBack;
                    return;
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    base.Name = "Lord Spark, King of Sparkatonia";
                    MaxHealth = 500;
                    base.Damage = 45;
                    base.XPValue = 800;
                    MinMoneyDropAmount = 6;
                    MaxMoneyDropAmount = 10;
                    MoneyDropChance = 1f;
                    base.Speed = 380f;
                    this.TurnSpeed = 0.03f;
                    ProjectileSpeed = 0f;
                    base.JumpHeight = 500f;
                    CooldownTime = 2f;
                    base.AnimationDelay = 0.1f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = false;
                    base.CanBeKnockedBack = false;
                    base.IsWeighted = false;
                    this.Scale = EnemyEV.Spark_Miniboss_Scale;
                    base.ProjectileScale = EnemyEV.Spark_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Spark_Miniboss_Tint;
                    MeleeRadius = 10;
                    ProjectileRadius = 20;
                    EngageRadius = 30;
                    ProjectileDamage = base.Damage;
                    base.KnockBack = EnemyEV.Spark_Miniboss_KnockBack;
                    return;
                default:
                    return;
            }
        }

        protected override void InitializeLogic() {
            base.CurrentSpeed = base.Speed;
            base.InitializeLogic();
        }

        public void HookToGround() {
            m_hookedToGround = true;
            float num = 1000f;
            TerrainObj terrainObj = null;
            foreach (TerrainObj current in m_levelScreen.CurrentRoom.TerrainObjList) {
                if (current.Y >= base.Y && current.Y - base.Y < num && CollisionMath.Intersects(current.Bounds, new Rectangle((int)base.X, (int)(base.Y + (current.Y - base.Y) + 5f), this.Width, this.Height / 2))) {
                    num = current.Y - base.Y;
                    terrainObj = current;
                }
            }
            if (terrainObj != null)
                base.Y = terrainObj.Y - (float)(this.Height / 2) + 5f;
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
            if (!m_hookedToGround)
                HookToGround();
            CollisionCheckRight();
            if (!base.IsPaused)
                base.Position += base.Heading * (base.CurrentSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        private void CollisionCheckRight() {
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;
            bool flag5 = false;
            bool flag6 = false;
            bool flag7 = false;
            bool flag8 = false;
            float num = 0f;
            if (Bounds.Right >= m_levelScreen.CurrentRoom.Bounds.Right) {
                flag6 = true;
                flag4 = true;
                flag8 = true;
            }
            else if (Bounds.Left <= m_levelScreen.CurrentRoom.Bounds.Left) {
                flag5 = true;
                flag3 = true;
                flag7 = true;
            }
            if (Bounds.Top <= m_levelScreen.CurrentRoom.Bounds.Top) {
                flag6 = true;
                flag = true;
                flag5 = true;
            }
            else if (Bounds.Bottom >= m_levelScreen.CurrentRoom.Bounds.Bottom) {
                flag7 = true;
                flag2 = true;
                flag8 = true;
            }
            foreach (TerrainObj current in m_levelScreen.CurrentRoom.TerrainObjList) {
                Rectangle b = new Rectangle((int)current.X, (int)current.Y, current.Width, current.Height);
                if (CollisionMath.RotatedRectIntersects(TopLeftPoint, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero))
                    flag5 = true;
                if (CollisionMath.RotatedRectIntersects(TopRightPoint, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero))
                    flag6 = true;
                if (CollisionMath.RotatedRectIntersects(BottomRightPoint, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero)) {
                    flag8 = true;
                    if (current.Rotation != 0f) {
                        Vector2 vector = CollisionMath.RotatedRectIntersectsMTD(BottomRightPoint, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero);
                        if (vector.X < 0f && vector.Y < 0f)
                            num = -45f;
                    }
                }
                if (CollisionMath.RotatedRectIntersects(BottomLeftPoint, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero)) {
                    flag7 = true;
                    if (current.Rotation != 0f) {
                        Vector2 vector2 = CollisionMath.RotatedRectIntersectsMTD(BottomLeftPoint, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero);
                        if (vector2.X > 0f && vector2.Y < 0f)
                            num = 45f;
                    }
                }
                if (CollisionMath.RotatedRectIntersects(TopRect, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero))
                    flag = true;
                if (CollisionMath.RotatedRectIntersects(BottomRect, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero))
                    flag2 = true;
                if (CollisionMath.RotatedRectIntersects(LeftRect, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero))
                    flag3 = true;
                if (CollisionMath.RotatedRectIntersects(RightRect, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero))
                    flag4 = true;
            }
            if (flag8 && !flag6 && !flag4)
                base.Orientation = 0f;
            if (flag6 && flag8 && !flag5)
                base.Orientation = MathHelper.ToRadians(-90f);
            if (flag6 && flag5 && !flag7)
                base.Orientation = MathHelper.ToRadians(-180f);
            if (flag5 && flag3 && !flag2)
                base.Orientation = MathHelper.ToRadians(90f);
            if (flag6 && !flag && !flag4)
                base.Orientation = MathHelper.ToRadians(-90f);
            if (flag5 && !flag && !flag3)
                base.Orientation = MathHelper.ToRadians(-180f);
            if (flag7 && !flag3 && !flag4 && !flag2)
                base.Orientation = MathHelper.ToRadians(90f);
            if (flag8 && !flag2 && !flag4)
                base.Orientation = 0f;
            if (num != 0f && ((num < 0f && flag8 && flag4) || (num > 0f && !flag8)))
                base.Orientation = MathHelper.ToRadians(num);
            base.HeadingX = (float)Math.Cos((double)base.Orientation);
            base.HeadingY = (float)Math.Sin((double)base.Orientation);
        }
    }
}
