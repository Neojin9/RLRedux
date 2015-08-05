using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace RogueCastle {
    public class NpcObj : ObjContainer {
        private SpriteObj m_talkIcon;

        public NpcObj(string spriteName) : base(spriteName) {
            CanTalk = true;
            m_talkIcon = new SpriteObj("ExclamationBubble_Sprite");
            m_talkIcon.Scale = new Vector2(2f, 2f);
            m_talkIcon.Visible = false;
            m_talkIcon.OutlineWidth = 2;
            base.OutlineWidth = 2;
        }

        public bool CanTalk { get; set; }

        public bool IsTouching {
            get { return m_talkIcon.Visible; }
        }

        public void Update(GameTime gameTime, PlayerObj player) {
            bool flag = false;
            if (this.Flip == SpriteEffects.None && player.X > base.X)
                flag = true;
            if (this.Flip != SpriteEffects.None && player.X < base.X)
                flag = true;
            if (player != null && CollisionMath.Intersects(player.TerrainBounds, new Rectangle(this.Bounds.X - 50, this.Bounds.Y, this.Bounds.Width + 100, this.Bounds.Height)) && flag && player.Flip != this.Flip && CanTalk)
                m_talkIcon.Visible = true;
            else
                m_talkIcon.Visible = false;
            if (this.Flip == SpriteEffects.None) {
                m_talkIcon.Position = new Vector2((float)this.Bounds.Left - m_talkIcon.AnchorX, (float)this.Bounds.Top - m_talkIcon.AnchorY + (float)Math.Sin((Game.TotalGameTime * 20f)) * 2f);
                return;
            }
            m_talkIcon.Position = new Vector2((float)this.Bounds.Right + m_talkIcon.AnchorX, (float)this.Bounds.Top - m_talkIcon.AnchorY + (float)Math.Sin((Game.TotalGameTime * 20f)) * 2f);
        }

        public override void Draw(Camera2D camera) {
            if (this.Flip == SpriteEffects.None)
                m_talkIcon.Flip = SpriteEffects.FlipHorizontally;
            else
                m_talkIcon.Flip = SpriteEffects.None;
            base.Draw(camera);
            m_talkIcon.Draw(camera);
        }

        protected override GameObj CreateCloneInstance() {
            return new NpcObj(base.SpriteName);
        }

        protected override void FillCloneInstance(object obj) {
            base.FillCloneInstance(obj);
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_talkIcon.Dispose();
                m_talkIcon = null;
                base.Dispose();
            }
        }
    }
}
