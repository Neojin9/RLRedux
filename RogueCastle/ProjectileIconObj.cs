using DS2DEngine;
using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class ProjectileIconObj : GameObj {
        private ProjectileObj m_attachedProjectile;
        private SpriteObj m_iconBG;
        private int m_iconOffset = 60;
        private SpriteObj m_iconProjectile;

        public ProjectileIconObj() {
            base.ForceDraw = true;
            m_iconBG = new SpriteObj("ProjectileIcon_Sprite");
            m_iconBG.ForceDraw = true;
            m_iconProjectile = new SpriteObj("Blank_Sprite");
            m_iconProjectile.ForceDraw = true;
        }

        public ProjectileObj AttachedProjectile {
            get { return m_attachedProjectile; }
            set {
                m_attachedProjectile = value;
                if (value != null) {
                    m_iconProjectile.ChangeSprite(value.SpriteName);
                    m_iconProjectile.Scale = new Vector2(0.7f, 0.7f);
                }
            }
        }

        public void Update(Camera2D camera) {
            if (AttachedProjectile.X <= (float)(camera.Bounds.Left + m_iconOffset))
                base.X = (float)m_iconOffset;
            else if (AttachedProjectile.X > (float)(camera.Bounds.Right - m_iconOffset))
                base.X = (float)(1320 - m_iconOffset);
            else
                base.X = AttachedProjectile.X - camera.TopLeftCorner.X;
            if (AttachedProjectile.Y <= (float)(camera.Bounds.Top + m_iconOffset))
                base.Y = (float)m_iconOffset;
            else if (AttachedProjectile.Y > (float)(camera.Bounds.Bottom - m_iconOffset))
                base.Y = (float)(720 - m_iconOffset);
            else
                base.Y = AttachedProjectile.Y - camera.TopLeftCorner.Y;
            base.Rotation = CDGMath.AngleBetweenPts(camera.TopLeftCorner + base.Position, AttachedProjectile.Position);
            m_iconBG.Position = base.Position;
            m_iconBG.Rotation = base.Rotation;
            m_iconProjectile.Position = base.Position;
            m_iconProjectile.Rotation = AttachedProjectile.Rotation;
            m_iconProjectile.GoToFrame(AttachedProjectile.CurrentFrame);
        }

        public override void Draw(Camera2D camera) {
            if (base.Visible) {
                m_iconBG.Draw(camera);
                m_iconProjectile.Draw(camera);
            }
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_iconBG.Dispose();
                m_iconBG = null;
                m_iconProjectile.Dispose();
                m_iconProjectile = null;
                AttachedProjectile = null;
                base.Dispose();
            }
        }

        protected override GameObj CreateCloneInstance() {
            return new ProjectileIconObj();
        }

        protected override void FillCloneInstance(object obj) {
            base.FillCloneInstance(obj);
            ProjectileIconObj projectileIconObj = obj as ProjectileIconObj;
            projectileIconObj.AttachedProjectile = AttachedProjectile;
        }
    }
}
