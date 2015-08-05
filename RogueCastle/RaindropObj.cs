using DS2DEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Tweener;


namespace RogueCastle {
    public class RaindropObj : SpriteObj {
        private bool m_isParticle;
        private bool m_isSnowflake;
        private float m_speedX;
        private float m_speedY;
        private bool m_splashing;
        private Vector2 m_startingPos;

        public RaindropObj(Vector2 startingPos) : base("Raindrop_Sprite") {
            ChangeToRainDrop();
            m_speedY = CDGMath.RandomFloat(MaxYSpeed.X, MaxYSpeed.Y);
            m_speedX = CDGMath.RandomFloat(MaxXSpeed.X, MaxXSpeed.Y);
            IsCollidable = true;
            m_startingPos = startingPos;
            base.Position = m_startingPos;
            base.AnimationDelay = 0.0333333351f;
            this.Scale = new Vector2(2f, 2f);
        }

        public bool IsCollidable { get; set; }
        public Vector2 MaxYSpeed { get; set; }
        public Vector2 MaxXSpeed { get; set; }

        public void ChangeToSnowflake() {
            this.ChangeSprite("Snowflake_Sprite");
            m_isSnowflake = true;
            m_isParticle = false;
            base.Rotation = 0f;
            MaxYSpeed = new Vector2(200f, 400f);
            MaxXSpeed = new Vector2(-200f, 0f);
            base.Position = m_startingPos;
            m_speedY = CDGMath.RandomFloat(MaxYSpeed.X, MaxYSpeed.Y);
            m_speedX = CDGMath.RandomFloat(MaxXSpeed.X, MaxXSpeed.Y);
        }

        public void ChangeToRainDrop() {
            m_isSnowflake = false;
            m_isParticle = false;
            MaxYSpeed = new Vector2(800f, 1200f);
            MaxXSpeed = new Vector2(-200f, -200f);
            base.Rotation = 5f;
            m_speedY = CDGMath.RandomFloat(MaxYSpeed.X, MaxYSpeed.Y);
            m_speedX = CDGMath.RandomFloat(MaxXSpeed.X, MaxXSpeed.Y);
        }

        public void ChangeToParticle() {
            m_isSnowflake = false;
            m_isParticle = true;
            MaxYSpeed = new Vector2(0f, 0f);
            MaxXSpeed = new Vector2(500f, 1500f);
            base.Rotation = -90f;
            m_speedY = CDGMath.RandomFloat(MaxYSpeed.X, MaxYSpeed.Y);
            m_speedX = CDGMath.RandomFloat(MaxXSpeed.X, MaxXSpeed.Y);
            float num = CDGMath.RandomFloat(2f, 8f);
            this.Scale = new Vector2(num, num);
        }

        public void Update(List<TerrainObj> collisionList, GameTime gameTime) {
            if (!m_splashing) {
                float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
                base.Y += m_speedY * num;
                base.X += m_speedX * num;
                if (IsCollidable) {
                    Rectangle bounds = this.Bounds;
                    foreach (TerrainObj current in collisionList) {
                        TerrainObj terrainObj = current;
                        Rectangle bounds2 = terrainObj.Bounds;
                        if (terrainObj.Visible && terrainObj.CollidesTop && terrainObj.Y > 120f && CollisionMath.Intersects(bounds, bounds2)) {
                            if (terrainObj.Rotation == 0f) {
                                if (!m_isSnowflake)
                                    base.Y = (float)(bounds2.Top - 10);
                                RunSplashAnimation();
                                break;
                            }
                            if (terrainObj.Rotation != 0f && CollisionMath.RotatedRectIntersects(bounds, 0f, Vector2.Zero, new Rectangle((int)terrainObj.X, (int)terrainObj.Y, terrainObj.Width, terrainObj.Height), terrainObj.Rotation, Vector2.Zero)) {
                                if (!m_isSnowflake)
                                    base.Y -= 12f;
                                RunSplashAnimation();
                                break;
                            }
                            break;
                        }
                    }
                }
                if (base.Y > 720f)
                    RunSplashAnimation();
            }
            if (!base.IsAnimating && m_splashing && !m_isSnowflake)
                KillDrop();
        }

        public void UpdateNoCollision(GameTime gameTime) {
            float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Y += m_speedY * num;
            base.X += m_speedX * num;
            if (base.X > m_startingPos.X + 4000f || base.X < m_startingPos.X - 4000f)
                KillDrop();
            if (base.Y > m_startingPos.Y + 4000f || base.Y < m_startingPos.Y - 4000f)
                KillDrop();
        }

        private void RunSplashAnimation() {
            m_splashing = true;
            base.Rotation = 0f;
            if (!m_isSnowflake) {
                base.PlayAnimation(2, base.TotalFrames, false);
                return;
            }
            Tween.To(this, 0.25f, new Easing(Tween.EaseNone), new[] {
                "Opacity",
                "0"
            });
            Tween.AddEndHandlerToLastTween(this, "KillDrop", new object[0]);
        }

        public void KillDrop() {
            m_splashing = false;
            base.GoToFrame(1);
            base.Rotation = 5f;
            base.X = m_startingPos.X;
            base.Y = (float)CDGMath.RandomInt(-100, 0);
            if (m_isParticle) {
                base.Y = m_startingPos.Y;
                base.Rotation = -90f;
            }
            m_speedY = CDGMath.RandomFloat(MaxYSpeed.X, MaxYSpeed.Y);
            m_speedX = CDGMath.RandomFloat(MaxXSpeed.X, MaxXSpeed.Y);
            base.Opacity = 1f;
        }
    }
}
