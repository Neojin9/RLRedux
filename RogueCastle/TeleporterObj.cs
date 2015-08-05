using System;
using DS2DEngine;
using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class TeleporterObj : PhysicsObj {
        private SpriteObj m_arrowIcon;

        public TeleporterObj() : base("TeleporterBase_Sprite", null) {
            base.CollisionTypeTag = 1;
            SetCollision(false);
            base.IsWeighted = false;
            Activated = false;
            base.OutlineWidth = 2;
            m_arrowIcon = new SpriteObj("UpArrowSquare_Sprite");
            m_arrowIcon.OutlineWidth = 2;
            m_arrowIcon.Visible = false;
        }

        public bool Activated { get; set; }

        public void SetCollision(bool collides) {
            base.CollidesTop = collides;
            base.CollidesBottom = collides;
            base.CollidesLeft = collides;
            base.CollidesRight = collides;
        }

        public override void Draw(Camera2D camera) {
            if (m_arrowIcon.Visible) {
                m_arrowIcon.Position = new Vector2((float)this.Bounds.Center.X, (this.Bounds.Top - 50) + (float)Math.Sin((Game.TotalGameTime * 20f)) * 2f);
                m_arrowIcon.Draw(camera);
                m_arrowIcon.Visible = false;
            }
            base.Draw(camera);
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType) {
            PlayerObj playerObj = otherBox.AbsParent as PlayerObj;
            if (!Game.ScreenManager.Player.ControlsLocked && playerObj != null && playerObj.IsTouchingGround)
                m_arrowIcon.Visible = true;
            base.CollisionResponse(thisBox, otherBox, collisionResponseType);
        }

        protected override GameObj CreateCloneInstance() {
            return new TeleporterObj();
        }

        protected override void FillCloneInstance(object obj) {
            base.FillCloneInstance(obj);
            TeleporterObj teleporterObj = obj as TeleporterObj;
            teleporterObj.Activated = Activated;
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                m_arrowIcon.Dispose();
                m_arrowIcon = null;
                base.Dispose();
            }
        }
    }
}
